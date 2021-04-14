// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using Xunit;

namespace Nexcal.Engine.Tests
{
	public class TestBase
	{
        protected void ParseAndCalculcate(string input, string parsed, string result)
		{
            var calc	= new Calculator();
            var expr    = calc.Parse(input);
			var number	= calc.Calculate(expr);

            Assert.Equal(parsed, expr.ToString());
            Assert.Equal(result, number.ToString());
			Assert.Empty(calc.Warnings);
		}
	}
}
