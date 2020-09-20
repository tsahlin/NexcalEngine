// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

namespace Nexcal.Engine.Operators
{
	public abstract class BinaryOperator : Operator
	{
		public override Number Evaluate(Calculator calc)
		{
			var result = Evaluate(calc, LeftToken.Evaluate(calc), RightToken.Evaluate(calc));

			calc.Replace(LeftToken, RightToken, result);

			return result;
		}

		protected abstract Number Evaluate(Calculator calc, Number left, Number right);
	}
}
