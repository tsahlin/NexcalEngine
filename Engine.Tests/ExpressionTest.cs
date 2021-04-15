// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using Nexcal.Engine.Operators;
using Xunit;

namespace Nexcal.Engine.Tests
{
	public class ExpressionTest
	{
		[Fact]
		public void Add()
		{
			var expr = new Expression(new Position());
			Assert.Equal("<null>", expr.AnchorList);

			expr.Add(new Number(new Position()));
			Assert.Equal("| <--- Anchor\n|\n| <--> Number\n|\n| ---> Anchor", expr.AnchorList);

			expr.Add(new Add(new Position()));
			expr.Add(new Subtract(new Position()));
			Assert.Equal("| <--- Anchor\n|\n| <--> Number\n| <--> Add\n| <--> Subtract\n|\n| ---> Anchor", expr.AnchorList);
		}

		[Theory]
		[InlineData("1+2", 3)]
		[InlineData("0+(1+2)", 3)]
		//[InlineData("2*(-3+5)/-2", -2)]
		public void Evaluate(string expr, double result)
		{
			var calculator	= new Calculator();
			var number		= calculator.Calculate(expr);

			Assert.Equal(result, number.Value);
			Assert.Empty(calculator.Warnings);
		}

		[Fact]
		public void Prepend()
		{
			var expr = new Expression(new Position());
			Assert.Equal("<null>", expr.AnchorList);

			expr.Prepend(new Number(new Position()));
			Assert.Equal("| <--- Anchor\n|\n| <--> Number\n|\n| ---> Anchor", expr.AnchorList);

			expr.Prepend(new Add(new Position()));
			expr.Prepend(new Subtract(new Position()));
			Assert.Equal("| <--- Anchor\n|\n| <--> Subtract\n| <--> Add\n| <--> Number\n|\n| ---> Anchor", expr.AnchorList);
		}

		[Fact]
		public void ReplaceEnd()
		{
			var calc	= new Calculator();
            var expr    = calc.Parse("1+2+3");
			var three	= expr.LastToken;
			var two		= three.LeftToken.LeftToken;

			Assert.IsType<Number>(three);
			Assert.IsType<Number>(two);

			two.Replace(two, three);

			Assert.Equal("1 + 2", expr.ToString());
		}

		[Fact]
		public void ReplaceStart()
		{
			var calc	= new Calculator();
            var expr    = calc.Parse("1+2+3");
			var one		= expr.FirstToken;
			var two		= one.RightToken.RightToken;

			Assert.IsType<Number>(one);
			Assert.IsType<Number>(two);

			one.Replace(one, two);

			Assert.Equal("1 + 3", expr.ToString());
		}
	}
}
