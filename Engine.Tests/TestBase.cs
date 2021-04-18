// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using Nexcal.Engine.Errors;
using System.Collections.Generic;
using Xunit;

namespace Nexcal.Engine.Tests
{
	public class TestBase
	{
		protected List<Warning> CalculateWithWarnings(string input, string parsed, string result)
		{
            var calc	= new Calculator();
            var expr    = calc.Parse(input);

			Assert.Equal(parsed, expr.ToString());

			var number = calc.Calculate(expr);

            Assert.Equal(result, number.ToString());

			return calc.Warnings;
		}

        protected void ParseAndCalculcate(string input, string parsed, string result)
		{
            var calc	= new Calculator();
            var expr    = calc.Parse(input);

			Assert.Equal(parsed, expr.ToString());

			var number = calc.Calculate(expr);

            Assert.Equal(result, number.ToString());
			Assert.Empty(calc.Warnings);
		}
	}
}
