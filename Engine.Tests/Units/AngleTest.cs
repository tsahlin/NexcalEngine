// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using Xunit;

namespace Nexcal.Engine.Tests.Units
{
	public class AngleTest : TestBase
	{
		[Theory]
        [InlineData("180 deg + 200 gon", "180° + 200 gon", "360°")]
		public void Addition(string input, string parsed, string result)
		{
            ParseAndCalculcate(input, parsed, result);
		}

		[Theory]
        [InlineData("1 deg as rad", "1° as rad", "0.0174532925199433 rad")]
        [InlineData("3.1415926535897931 rad as deg", "3.14159265358979 rad as °", "180°")]
        [InlineData("180 deg as rad", "180° as rad", "3.14159265358979 rad")]
        [InlineData("90 deg as gon", "90° as gon", "100 gon")]
        [InlineData("400 gon as deg", "400 gon as °", "360°")]
		public void Conversion(string input, string parsed, string result)
		{
            ParseAndCalculcate(input, parsed, result);
		}
	}
}
