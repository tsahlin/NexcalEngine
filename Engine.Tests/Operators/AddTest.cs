// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;
using Nexcal.Engine.Units;
using Xunit;

namespace Nexcal.Engine.Tests
{
	public class AddTest
	{
		[Theory]
        [InlineData("1+2+3", 6, null)]
		[InlineData("1m+2", 3, typeof(Meter))]
        [InlineData("5+3m", 8, typeof(Meter))]
		public void Parse(string expr, double result, Type type)
		{
            var calc	= new Calculator();
			var number	= calc.Calculate(expr);

			Assert.Equal(result, number.Value);
			Assert.Empty(calc.Warnings);

            if (type == null)
                Assert.Equal(null, number.Unit);
            else
                Assert.Equal(type, number.Unit.GetType());
		}
	}
}
