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
			// TODO: Kolla att left och right är valid operands (inte binaryoperator t.ex.)

			var result = Evaluate(calc, LeftToken.Evaluate(calc), RightToken.Evaluate(calc));

			calc.Replace(LeftToken, RightToken, result);

			return result;
		}

		protected abstract Number Evaluate(Calculator calc, Number left, Number right);
	}
}
