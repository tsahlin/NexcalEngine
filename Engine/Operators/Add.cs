// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;

namespace Nexcal.Engine.Operators
{
	public class Add : BinaryOperator
	{
		public Add(Position position) : base(position)
		{
		}

		public override Precedence Precedence => Precedence.Additive;

		public static Number Calc(Calculator calc, Number left, Number right)
		{
			var result = left.Clone;

			result.Value += right.Value;

			return result;
			// TODO: Hantera units och NumberBase
			// behåll samma bas på ett "intelligent" sätt
		}

		protected override Number Evaluate(Calculator calc, Number left, Number right)
		{
			return Calc(calc, left, right);
		}

		public override string ToString()
		{
			return " + ";
		}
	}
}
