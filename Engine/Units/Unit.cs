// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;
using System.Collections.Generic;

namespace Nexcal.Engine.Units
{
	public abstract class Unit : Token
	{
		public Unit(Position position) : base(position)
		{
		}

		internal override Number Evaluate(Calculator calc)
		{
			// Sätt left token Number.Unit = this
			// Ersätt left token + this med bara left token
			// Om number redan har en unit = error
			throw new NotImplementedException();
		}

		public abstract string Format(Number number);

		internal static void InitIdentifierMap(Dictionary<string, Type> map)
		{
			map["m"]	= typeof(Meter);
		}
	}
}
