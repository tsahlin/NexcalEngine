// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

namespace Nexcal.Engine.Operators
{
	public abstract class BinaryOperator : Operator
	{
		internal BinaryOperator(Position position) : base(position)
		{
		}

		internal override Number Evaluate(Calculator calc)
		{
			calc.DebugEvaluate(this);
			
			var left	= RequireLeftNumber(calc);
			var right	= RequireRightNumber(calc);
			var result	= Evaluate(calc, left, right);

			result.Position.Length = right.Position.Index + right.Position.Length - left.Position.Index;
			result.Replace(left, right);

			return result;
		}

		protected abstract Number Evaluate(Calculator calc, Number left, Number right);
	}
}
