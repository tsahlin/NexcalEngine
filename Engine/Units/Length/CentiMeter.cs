// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

namespace Nexcal.Engine.Units.Length
{
	public class CentiMeter : LengthUnit
	{
		public CentiMeter(Position position) : base(position)
		{
		}

        public override double Factor => 0.01;

		public override string ToString()
		{
			return "cm";
		}
	}
}
