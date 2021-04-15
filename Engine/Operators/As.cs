// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using Nexcal.Engine.Errors;
using Nexcal.Engine.Units;

namespace Nexcal.Engine.Operators
{
	public class As : Operator
	{
		public As(Position position) : base(position)
		{
		}

		public override Precedence Precedence => Precedence.Additive;

        internal Unit Unit { get; set; }

        internal override Number Evaluate(Calculator calc)
		{
			calc.DebugEvaluate(this);
			
			var number = RequireLeftNumber(calc);
            number.ConvertTo(Unit).Replace(number, this);

			return number;
		}

        internal override Token PreProcess(Parser parser)
		{
            VerifyRight<Unit>(CalculatorError.RightUnitRequired);

            // We consume the unit here so it's not evaluated on its own
            Unit = (Unit)RightToken;
            Replace(this, Unit);

			return this;
		}

		public override string ToString()
		{
			return $" as {Unit}";
		}
	}
}
