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
			var parser = new Parser(str);
			var op = Operator.Parse(parser);

			Assert.Equal(type, op.GetType());
		}
	}
}
