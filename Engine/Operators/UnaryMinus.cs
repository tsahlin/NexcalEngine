// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

namespace Nexcal.Engine.Operators
{
	public class UnaryMinus : UnaryOperator
	{
		internal UnaryMinus(Position position) : base(position)
		{
		}

		internal override Number Evaluate(Calculator calc)
		{
			calc.DebugEvaluate(this);
			
			var number	= RequireRightNumber(calc);
            var result  = number.Negate();

			result.Replace(this, number);

			return result;
		}

        public override string ToString()
		{
			return "-";
		}
	}
}
