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

		internal override Token PreProcess(Parser parser)
		{
			if (LeftToken is Anchor || LeftToken is BinaryOperator)
			{
				// Convert this binary subtract to a unary minus
				var minus = new UnaryMinus(Position);

				minus.Replace(this);

				return minus;
			}

			return this;
		}

		public override string ToString()
		{
			return " - ";
		}
	}
}
