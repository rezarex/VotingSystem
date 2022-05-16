using Xunit;

namespace VotingSystem.Test
{
    public class MathOne
    {
        public readonly ITestOne testOne;

        public MathOne(ITestOne testOne)
        {
            this.testOne = testOne;
        }

        public int Add(int a, int b) => testOne.Add(a, b);
    }
    public interface ITestOne
    {
        public int Add(int a, int b);
    }

    public class TestOne : ITestOne
    {
        public int Add(int a, int b) => a + b;
    }

   

    public class TestOneTests
    {
        [Fact]
        public void Add_AddsTwoNumbersTogether()
        {
            var result = new TestOne().Add(1, 1);
            Assert.Equal(2, result);
        }

        [Theory]


        [InlineData(1, 1,2)]
        [InlineData(1, 2, 3)]
        [InlineData(1, 0, 1)]
        [InlineData(0, 1, 1)]
        public void Add_AddsTwoNumbersTogether_Theory(int a, int b, int expected)
        {
            var result = new TestOne().Add(a, b);
            Assert.Equal(expected, result);
        }
    }
}