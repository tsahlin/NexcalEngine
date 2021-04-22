// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;

namespace Nexcal.Engine.Units.Angle
{
	public abstract class AngularUnit : Unit
	{
		public AngularUnit(Position position) : base(position)
		{
		}

		public override Type BaseUnit => typeof(Degree);
	}
}
