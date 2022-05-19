using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Xunit.Assert;

namespace VotingSystem.Tests
{
    public class CounterTests
    {
        public const string CounterName = "Counter Name";
        public Counter _counter = new Counter { Name = CounterName, Count = 5 };
        [Fact]
        public void HasName()
        {

            var name = "Counter Name";
            var counter = new Counter();
            counter.Name = name;

            Equal(CounterName, _counter.Name);
        }

        [Fact]
        public void GetStatistics_IncludesCounterName()
        {


            var statistics = _counter.GetStatistics(5);
            Equal(5, statistics.Count);
        }

        [Fact]
        public void GetStatistics_IncludesCounterCount()
        {


            var statistics = _counter.GetStatistics(5);
            Equal(CounterName, statistics.Name);
        }

        [Theory]
        [InlineData(5, 10, 50)]
        [InlineData(1, 3, 33.33)]
        [InlineData(2, 3, 66.67)]
        public void GetStatistics_ShowsPercentageUpToTwoDecmalsBasedOnTotalCount(int count, int total, double expected)
        {
            _counter.Count = count;
            var statistics = _counter.GetStatistics(total);
            Equal(expected, statistics.Percent);
        }

        [Fact]
        public void ResolveExcess_DoesntAddExessWhenAllCountersAreEqual()
        {
            var counter1 = new Counter {  Percent = 33.33 };
            var counter2 = new Counter {  Percent = 33.33 };
            var counter3 = new Counter {  Percent = 33.33 };
            var counters = new List<Counter> { counter1, counter2, counter3 };

            new CounterManager().ResolveExcess(counters);

            Equal(33.33, counter1.Percent);
            Equal(33.33, counter2.Percent);
            Equal(33.33, counter3.Percent);
        }

        [Theory]
        [InlineData(66.66, 66.67, 33.33)]
        [InlineData(66.65, 66.67, 33.33)]
        [InlineData(66.66, 66.68, 33.32)]
        public void ResolveExcess_AddsExcessToHighestCounter(double initial, double expected, double lowest)
        {
            var counter1 = new Counter { Percent = initial };
            var counter2 = new Counter { Percent = lowest };
            var counters = new List<Counter> { counter1, counter2 };

            new CounterManager().ResolveExcess(counters);

            Equal(expected, counter1.Percent);
            Equal(lowest, counter2.Percent);


            var counter3 = new Counter { Percent = initial };
            var counter4 = new Counter { Percent = lowest };

            counters = new List<Counter> { counter4, counter3 };

            new CounterManager().ResolveExcess(counters);

            Equal(expected, counter3.Percent);
            Equal(lowest, counter4.Percent);

        }


        [Theory]
        [InlineData(11.11, 11.12, 44.44)]
        [InlineData(11.10, 11.12, 44.44)]
        public void ResolveExcess_AddsExcesToLowestCounterWhenMoreThanOneHighestCount(double initial, double expected, double highest)
        {
            var counter1 = new Counter {  Percent = highest };
            var counter2 = new Counter {  Percent = highest };
            var counter3 = new Counter {  Percent = initial };
            var counters = new List<Counter> { counter1, counter2, counter3 };
           
            new CounterManager().ResolveExcess(counters);

            Equal(highest, counter1.Percent);
            Equal(highest, counter2.Percent);
            Equal(expected, counter3.Percent);
        }


        [Fact]
        public void ResolveExcess_DoesntAddExcessIfTotalPercentIs100()
        {
            var counter1 = new Counter {  Percent = 80 };
            var counter2 = new Counter {  Percent = 20 };
            var counters = new List<Counter> { counter1, counter2 };

            new CounterManager().ResolveExcess(counters);

            Equal(80, counter1.Percent);
            Equal(20, counter2.Percent);

        }



    }

    public class Counter
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public double Percent { get; set; }

        internal Counter GetStatistics(int totalCount)
        {

            Percent = CounterManager.RoundUp(Count * 100.0 / totalCount);
            //
            //this is not the best approach, replaced with the above more generic solution
            //
            //
            //if (totalCount == 10)
            //    Percent = 50;
            //else if (totalCount == 3)
            //    Percent = 33.33;
            return this;
        }
    }

    public class CounterManager
    {
        public void ResolveExcess(List<Counter> counter)
        {

            var totalPercent = counter.Sum(x => x.Percent);
            if (totalPercent == 100) return;

            var excess = 100 - totalPercent;
            
            var highestPercent = counter.Max(x => x.Percent);
            var highestCounters = counter.Where(x=> x.Percent == highestPercent).ToList();

            if (highestCounters.Count == 1)
            {
                highestCounters.First().Percent += excess;
            }
            else if (highestCounters.Count < counter.Count)
            {
                var lowestPercent = counter.Min(x => x.Percent);
                var lowestCounter =  counter.First(x => x.Percent == lowestPercent);
                lowestCounter.Percent = RoundUp(lowestCounter.Percent + excess);
            }
        }

        public static double RoundUp(double num) => Math.Round(num, 2);
    }
}