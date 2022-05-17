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
        public Counter _counter = new Counter { Name = CounterName, Count=5};
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
            

            var statistics = _counter.GetStatistics();
            Equal(5, statistics.Count);
        }

        [Fact]
        public void GetStatistics_IncludesCounterCount()
        {


            var statistics = _counter.GetStatistics();
            Equal(CounterName, statistics.Name);
        }
    }

    public class Counter
    {
        public string Name { get; set;  }
        public int Count { get; set; }

        internal Counter GetStatistics()
        {
            return this;
        }
    }
}