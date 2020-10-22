// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;

namespace Nexcal.Engine.Delimiters
{
	public abstract class Delimiter : Token
	{
		public Delimiter(Position position) : base(position)
		{
		}

		internal override Number Evaluate(Calculator calc)
		{
			throw new NotImplementedException();
		}
	}
}
