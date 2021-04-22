// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;
using System.Collections.Generic;
using Nexcal.Engine.Errors;

namespace Nexcal.Engine.Units
{
	public abstract class Unit : Token
	{
		public Unit() : base(Position.Unknown)
		{
		}

		public Unit(Position position) : base(position)
		{
		}

		public virtual Type BaseUnit => null;

		/// <summary>1 of this equals Factor of BaseUnit</summary>
		public virtual double Factor => 1;

		protected virtual string FormatString => "{0} {1}";		// 0 = number, 1 = unit

		public override Precedence Precedence => Precedence.Unit;

		internal virtual double Add(double value, Number operand)
		{
			if (!CanAdd(operand.Unit))
				throw new UnitException(this, operand.Unit, CalculatorError.CannotAddUnits);

			return FromBase(ToBase(value) + operand.Unit.ToBase(operand.Value));
		}

		protected Number ApplyToNumber(Number number)
		{
			if (number.Unit != null)
				throw new CalculatorException(this, CalculatorError.UnitAlreadyAssigned);

			number.Unit = this;

			number.Replace(number, this);

			return number;
		}

		public bool CanAdd(Unit unit)
		{
			return GetSumUnit(unit) != null;
		}

		public bool CanMultiply(Unit unit)
		{
			return GetProductUnit(unit) != null;
		}

		internal override Number Evaluate(Calculator calc)
		{
			return ApplyToNumber(LeftToken.Evaluate(calc));
		}

		public virtual string Format(Number number)
		{
			return string.Format(FormatString, number.ValueString, this);
		}

		public virtual double FromBase(double value)
		{
			return GetType() == BaseUnit ? value : value / Factor;
		}

		public virtual Type GetProductUnit(Unit unit)
		{
			return null;
		}

		public virtual Type GetSumUnit(Unit unit)
		{
			return BaseUnit == unit.BaseUnit ? GetType() : null;
		}

		internal static void InitIdentifierMap(Dictionary<string, Type> map)
		{
			map["gon"]	= typeof(Angle.Gradian);
			map["°"]	= typeof(Angle.Degree);
			map["deg"]	= typeof(Angle.Degree);
			map["rad"]	= typeof(Angle.Radian);

			map["cm"]	= typeof(Length.CentiMeter);
			map["m"]	= typeof(Length.Meter);
			map["km"]	= typeof(Length.KiloMeter);
		}

		internal override Token PreProcess(Parser parser)
		{
			// When the left token is a simple number, we apply the unit during pre-processing
			// to enable better string formatting. If the left token is an expression, the unit
			// is applied during evaluation instead.

			if (LeftToken is Number n)
				return ApplyToNumber(n);
				
			VerifyLeftNumber();

			return this;
		}

		public virtual double ToBase(double value)
		{
			return GetType() == BaseUnit ? value : value * Factor;
		}
	}
}
