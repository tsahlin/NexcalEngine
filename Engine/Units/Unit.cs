// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;
using System.Collections.Generic;
using Nexcal.Engine.Errors;

namespace Nexcal.Engine.Units
{
	public abstract class Unit : Token
	{
		public Unit(Position position) : base(position)
		{
		}

		internal override Number Evaluate(Calculator calc)
		{
			var number = LeftToken.Evaluate(calc);

			if (number.Unit != null)
				throw new CalculatorException(this, CalculatorError.UnitAlreadyAssigned);

			number.Unit = this;

			calc.Replace(number, this, number);

			return number;
		}

		public abstract string Format(Number number);

		internal static void InitIdentifierMap(Dictionary<string, Type> map)
		{
			map["m"]	= typeof(Meter);
		}

		internal override Token PreProcess(Calculator calc)
		{
			VerifyLeftNumber();

			return this;
		}
	}
}
