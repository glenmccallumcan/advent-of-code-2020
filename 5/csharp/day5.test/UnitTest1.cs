using Xunit;

namespace day5.test
{
    public class Tests
    {
        [Theory]
        [InlineData("FBFBBFFRLR", 357)]
        [InlineData("BFFFBBFRRR", 567)]
        [InlineData("FFFBBBFRRR", 119)]
        [InlineData("BBFFBBFRLL", 820)]
        public void ComputesCorrectResponse(string input, int result)
        {
            Assert.Equal(expected: Program.ComputeSeatId(input), result);
        }
    }
}