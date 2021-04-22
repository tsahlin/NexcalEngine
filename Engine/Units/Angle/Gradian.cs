// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

namespace Nexcal.Engine.Units.Angle
{
	public class Gradian : AngularUnit
	{
		public Gradian(Position position) : base(position)
		{
		}

        public override double Factor => 0.9;

		public override string ToString()
		{
			return "gon";
		}
	}
}
