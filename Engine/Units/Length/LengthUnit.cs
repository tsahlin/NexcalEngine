// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;

namespace Nexcal.Engine.Units.Length
{
	public abstract class LengthUnit : Unit
	{
		public LengthUnit(Position position) : base(position)
		{
		}

		public override Type BaseUnit => typeof(Meter);
	}
}
