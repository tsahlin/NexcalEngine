using System;

namespace Nexcal.Engine.Operators
{
	public class Add : BinaryOperator
	{
		public override Precedence Precedence => Precedence.Additive;

		protected override Number Evaluate(Calculator calc, Number left, Number right)
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			return " + ";
		}
	}
}
