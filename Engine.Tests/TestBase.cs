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

			AssertFiltered(parsed, expr.ToString());

			var number = calc.Calculate(expr);

            AssertFiltered(result, number.ToString());

			return calc.Warnings;
		}

		protected void AssertFiltered(string expected, string actual)
		{
			Assert.Equal(FilterString(expected), FilterString(actual));
		}

		protected static string FilterString(string input)
		{
			string output = input;

			output = output.Replace((char)8722, '-');

			return output;
		}

        protected void ParseAndCalculcate(string input, string parsed, string result)
		{
            var calc	= new Calculator();
            var expr    = calc.Parse(input);

			AssertFiltered(parsed, expr.ToString());

			var number = calc.Calculate(expr);

            AssertFiltered(result, number.ToString());
			Assert.Empty(calc.Warnings);
		}
	}
}
