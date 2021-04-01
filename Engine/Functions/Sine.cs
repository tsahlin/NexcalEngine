// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;

namespace Nexcal.Engine.Functions
{
	public class Sine : FunctionCall
	{
		public Sine(Position position) : base(position)
		{
		}

		public override string ToString()
		{
			return $"sin({ArgString})";
		}
	}
}
