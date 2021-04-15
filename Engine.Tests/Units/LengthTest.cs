// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using Xunit;

namespace Nexcal.Engine.Tests.Units
{
	public class LengthTest : TestBase
	{
		[Theory]
        [InlineData("1+2m", "1 + 2 m", "3 m")]
		[InlineData("2m+3", "2 m + 3", "5 m")]
        [InlineData(".5m + 1.1m", "0.5 m + 1.1 m", "1.6 m")]
		[InlineData("(1+2)m", "(1 + 2)m", "3 m")]
		[InlineData("2km + 300m", "2 km + 300 m", "2.3 km")]
		public void Addition(string input, string parsed, string result)
		{
            ParseAndCalculcate(input, parsed, result);
		}
	}
}
