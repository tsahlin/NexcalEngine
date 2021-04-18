// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using Nexcal.Engine.Errors;
using Xunit;

namespace Nexcal.Engine.Tests.Operators
{
	public class AddTest : TestBase
	{
		[Theory]
        [InlineData("0+0", "0 + 0", "0")]
		[InlineData(".005  +  .995", "0.005 + 0.995", "1")]
		public void Evaluate(string input, string parsed, string result)
		{
            ParseAndCalculcate(input, parsed, result);
		}

		[Fact]
		public void Overflow()
		{
			var warnings = CalculateWithWarnings($"{Number.MaxSafeInteger}+0",
				$"{Number.MaxSafeInteger} + 0", $"{Number.MaxSafeInteger}");

			Assert.Empty(warnings);

			warnings = CalculateWithWarnings($"{Number.MaxSafeInteger}+1",
				$"{Number.MaxSafeInteger} + 1", $"{Number.MaxSafeInteger + 1}");

			Assert.NotEmpty(warnings);
			Assert.Equal(WarningCode.ResultOutOfSafeRange, warnings[0].Code);
			Assert.Equal(16, warnings[0].Position.Index);
			Assert.Equal(1, warnings[0].Position.Length);
		}
	}
}
