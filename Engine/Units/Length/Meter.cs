// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;

namespace Nexcal.Engine.Units.Length
{
	public class Meter : LengthUnit
	{
		public Meter(Position position) : base(position)
		{
		}

		public override string Format(Number number)
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			return "m";
		}
	}
}
