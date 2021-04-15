// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

namespace Nexcal.Engine.Operators
{
	public class Add : BinaryOperator
	{
		public Add(Position position) : base(position)
		{
		}

		public override Precedence Precedence => Precedence.Additive;

		protected override Number Evaluate(Calculator calc, Number left, Number right)
		{
			var result = left.Add(right);

			// TODO: Check for overflow and issue warning

			return result;
		}

		public override string ToString()
		{
			return " + ";
		}
	}
}
