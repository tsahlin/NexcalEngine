// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;

namespace Nexcal.Engine.Units.Angle
{
	public class Radian : AngularUnit
	{
		public Radian(Position position) : base(position)
		{
		}

        public override double Factor => 180 / Math.PI;

		public override string ToString()
		{
			return "rad";
		}
	}
}
