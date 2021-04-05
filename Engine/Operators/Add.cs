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

		protected override void Evaluate(Calculator calc, Number left, Number right, Number result)
		{
			result.Value += right.Value;
			// TODO: Hantera units och NumberBase
			// behåll samma bas på ett "intelligent" sätt
		}

		public override string ToString()
		{
			return " + ";
		}
	}
}
