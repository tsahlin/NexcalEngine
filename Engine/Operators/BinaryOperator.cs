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
			var left	= RequireLeftNumber(calc);
			var right	= RequireRightNumber(calc);
			var result	= Evaluate(calc, left, right);

			calc.Replace(LeftToken, RightToken, result);

			return result;
		}

		protected abstract Number Evaluate(Calculator calc, Number left, Number right);
	}
}
