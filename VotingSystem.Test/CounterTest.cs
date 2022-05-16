using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace VotingSystem.Tests
{
    public class CounterTests
    {
        [Fact]
        public void HasName()
        {

            var name = "Counter Name";
            var counter = new Counter();
            counter.Name = name;

            Assert.Equal(name, counter.Name);
        }

        [Fact]
        public void GetStatistics_IncludesCounterName()
        {
            var name = "Counter Name";
            var counter = new Counter();
            counter.Name = name;

            var statistics = counter.GetStatistics();
            Assert.Equal(name, statistics.Name);
        }
    }

    public class Counter
    {
        public string Name { get; set;  }

        public Counter()
        {
        }

        internal Counter GetStatistics()
        {
            return this;
        }
    }
}