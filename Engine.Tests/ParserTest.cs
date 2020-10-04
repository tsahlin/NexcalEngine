// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using Xunit;

namespace Nexcal.Engine.Tests
{
	public class ParserTest
	{
		[Theory]
		[InlineData("1+2-3", "Number,Add,Number,Subtract,Number", "1 + 2 - 3")]
		[InlineData("10m", "Number,Meter", "10m")]
		[InlineData("5 mod 3", "Number,Modulo,Number", "5 mod 3")]
		public void Parse(string expr, string names, string toStr)
		{
			var calculator	= new Calculator();
			var expression	= calculator.Parse(expr);

			Assert.Equal(names, expression.TokenNames);
			Assert.Equal(toStr, expression.ToString());
			Assert.Empty(calculator.Warnings);
		}
	}
}
