// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

namespace Nexcal.Engine.Operators
{
	public class Subtract : BinaryOperator
	{
		public Subtract(Position position) : base(position)
		{
		}

		public override Precedence Precedence => Precedence.Additive;

		protected override Number Evaluate(Calculator calc, Number left, Number right)
		{
			var result = left.Add(right.Negate());

			result.CheckResultRange(calc, Position);

			return result;
		}

		public override string ToString()
		{
			return " - ";
		}
	}
}
