// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

namespace Nexcal.Engine.Units.Angle
{
	public class Degree : AngularUnit
	{
		public Degree(Position position) : base(position)
		{
		}

		protected override string FormatString => "{0}{1}";

		public override string ToString()
		{
			return "°";
		}
	}
}
