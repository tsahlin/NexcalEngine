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
			Assert.Equal("<null>", expr.DebugList);

			expr.Add(new Number(new Position()));
			Assert.Equal("| <--- Anchor\n|\n| <--> Number\n|\n| ---> Anchor", expr.DebugList);

			expr.Add(new Add(new Position()));
			expr.Add(new Number(new Position()));
			Assert.Equal("| <--- Anchor\n|\n| <--> Number\n| <--> Add\n| <--> Number\n|\n| ---> Anchor", expr.DebugList);
		}
	}
}
