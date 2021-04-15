// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using Nexcal.Engine.Units;

namespace Nexcal.Engine.Errors
{
	public class UnitException : CalculatorException
	{
		public UnitException(Unit left, Unit right, CalculatorError error) : base(left, error)
		{
            LeftUnit    = left;
            RightUnit   = right;
		}

        public Unit LeftUnit { get; set; }

        public Unit RightUnit { get; set; }
	}
}
