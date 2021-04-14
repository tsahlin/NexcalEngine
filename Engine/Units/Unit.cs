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

		public virtual double Factor => 1;

		protected virtual string FormatString => "{0} {1}";		// 0 = number, 1 = unit

		public override Precedence Precedence => Precedence.Unit;

		internal virtual Number Add(double value, Number operand)
		{
			if (!CanAdd(operand.Unit))
				throw new UnitAddException(this, operand.Unit);

			var left = ToBase(value);
			var right = operand.Unit.ToBase(operand.Value);

			return FromBase(left.Value + right.Value);
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
			var number = LeftToken.Evaluate(calc);

			if (number.Unit != null)
				throw new CalculatorException(this, CalculatorError.UnitAlreadyAssigned);

			number.Unit = this;

			number.Replace(number, this);

			return number;
		}

		public virtual string Format(Number number)
		{
			return string.Format(FormatString, number.ValueString, this);
		}

		public virtual Number FromBase(double value)
		{
			return new Number()
			{
				Value = GetType() == BaseUnit ? value : value / Factor,
				Unit = this
			};
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
			map["m"]	= typeof(Length.Meter);
		}

		internal override Token PreProcess(Parser parser)
		{
			VerifyLeftNumber();

			return this;
		}

		public virtual Number ToBase(double value)
		{
			if (GetType() == BaseUnit)
				return new Number() { Value = value, Unit = this };

			return new Number()
			{
				Value = value * Factor,
				Unit = (Unit)Activator.CreateInstance(BaseUnit)
			};
		}
	}
}
