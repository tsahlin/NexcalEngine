// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using Nexcal.Engine.Errors;
using Xunit;

namespace Nexcal.Engine.Tests.Operators
{
	public class SubtractTest : TestBase
	{
		[Theory]
        [InlineData("0-0", "0 - 0", "0")]
		[InlineData("1  -  .995", "1 - 0.995", "0.005")]
		public void Evaluate(string input, string parsed, string result)
		{
            ParseAndCalculcate(input, parsed, result);
		}

		[Fact]
		public void Overflow()
		{
            // TODO: Requires unary minus support
			var warnings = CalculateWithWarnings($"{Number.MinSafeInteger}-0",
				$"{Number.MinSafeInteger} - 0", $"{Number.MinSafeInteger}");

			Assert.Empty(warnings);

			warnings = CalculateWithWarnings($"{Number.MinSafeInteger}-1",
				$"{Number.MinSafeInteger} - 1", $"{Number.MinSafeInteger - 1}");

			Assert.NotEmpty(warnings);
			Assert.Equal(WarningCode.ResultOutOfSafeRange, warnings[0].Code);
			Assert.Equal(16, warnings[0].Position.Index);
			Assert.Equal(1, warnings[0].Position.Length);
		}
	}
}
