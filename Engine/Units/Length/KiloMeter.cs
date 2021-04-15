// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

namespace Nexcal.Engine.Units.Length
{
	public class KiloMeter : LengthUnit
	{
		public KiloMeter(Position position) : base(position)
		{
		}

        public override double Factor => 1000;

		public override string ToString()
		{
			return "km";
		}
	}
}
