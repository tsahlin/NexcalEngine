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
			var result	= left.Clone();

			result.Position.Length = RightToken.Position.Index + RightToken.Position.Length - LeftToken.Position.Index;

			Evaluate(calc, left, right, result);

			result.Replace(LeftToken, RightToken);

			return result;
		}

		protected abstract void Evaluate(Calculator calc, Number left, Number right, Number result);
	}
}
