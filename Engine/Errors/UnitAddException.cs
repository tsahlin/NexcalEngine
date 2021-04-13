// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using Nexcal.Engine.Units;

namespace Nexcal.Engine.Errors
{
	public class UnitAddException : CalculatorException
	{
		public UnitAddException(Unit left, Unit right) : base(left, CalculatorError.CannotAddUnits)
		{
            LeftUnit    = left;
            RightUnit   = right;
		}

        public Unit LeftUnit { get; set; }

        public Unit RightUnit { get; set; }
	}
}
