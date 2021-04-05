// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;

namespace Nexcal.Engine.Operators
{
	public class Modulo : BinaryOperator
	{
		public Modulo(Position position) : base(position)
		{
		}

		public override Precedence Precedence => Precedence.Multiplicative;

		protected override void Evaluate(Calculator calc, Number left, Number right, Number result)
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			return " mod ";
		}
	}
}
