// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;

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
			// TODO: Invert sign of right and then use operator add
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			return " - ";
		}
	}
}
