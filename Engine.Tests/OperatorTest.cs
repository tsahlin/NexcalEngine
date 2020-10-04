// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;
using Nexcal.Engine.Operators;
using Xunit;

namespace Nexcal.Engine.Tests
{
	public class OperatorTest
	{
		[Theory]
		[InlineData("+", typeof(Add))]
		[InlineData("-", typeof(Subtract))]
		public void Parse(string str, Type type)
		{
			var calc = new Calculator();
			var expr = calc.Parse(str);

			Assert.Equal(type, expr.FirstToken.GetType());
		}
	}
}
