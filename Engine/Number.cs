// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Nexcal.Engine.Errors;
using Nexcal.Engine.Units;

namespace Nexcal.Engine
{
	public class Number : Token
	{
		public const int MaxBinLength = 53;      // 11111111111111111111111111111111111111111111111111111
		public const int MaxHexLength = 14;      // 1FFFFFFFFFFFFF

		public const long MaxSafeInteger = 9007199254740991;
		public const long MinSafeInteger = -9007199254740991;

		public Number() : base(Position.Unknown)
		{
		}

		public Number(Position position) : base(position)
		{
		}

		public NumberBase Base { get; set; } = NumberBase.Dec;

		public Number Clone => (Number)CloneToken();

		public bool IsInteger => Math.Floor(Value) == Math.Ceiling(Value);

		public override Precedence Precedence => Precedence.Number;

		public Unit Unit { get; set; }

		public double Value { get; set; } = 0;

		public string ValueString
		{
			get
			{
				return Value.ToString(IsInteger ? "G17" : "G15", CultureInfo.InvariantCulture);
			}
		}

		public Number Add(Number operand)
		{
			// TODO: if (operand.Unit is Percent) - need special handling

			if (Unit == null && operand.Unit != null)
				return operand.Add(this);

			var result = Clone;

			if (operand.Unit == null)
				result.Value += operand.Value;
			else
				result.Value = Unit.Add(Value, operand);

			return result;
		}

		public bool CheckResultRange(Calculator calc, Position pos)
		{
			if (Value > MaxSafeInteger || Value < MinSafeInteger)
			{
				calc.Warning(pos, WarningCode.ResultOutOfSafeRange);

				return false;
			}

			return true;
		}

		public Number ConvertTo(Unit unit)
		{
			if (Unit != null)
			{
				if (Unit.BaseUnit != unit.BaseUnit)
					throw new UnitException(Unit, unit, CalculatorError.CannotConvertUnits);

				Value = unit.FromBase(Unit.ToBase(Value));
            }

			Unit = unit;

			return this;
		}

		internal override Number Evaluate(Calculator calc)
		{
			calc.DebugEvaluate(this);
			
			return this;
		}

		public Number Negate()
		{
			var result = Clone;

			result.Value = -Value;

			return result;
		}

		public static Number Parse(Parser p)
		{
			var number = new Number(p.Position);

			Regex regex;
			Match match;

			if (p.CurrentChar == '0' && (p.NextChar == 'x' || p.NextChar == 'X'))     // Hex number
			{
				p.Position.Advance(2);

				if (p.CharsLeft < 1)
					throw new ParseException(p.Position, ParseExpectation.HexNumber);

				regex = new Regex(@"[0-9a-f]+", RegexOptions.IgnoreCase);
				match = regex.Match(p.ExpressionString, p.Position, Math.Min(p.CharsLeft, MaxHexLength + 1));

				if (!match.Success)
					throw new ParseException(p.Position, ParseExpectation.HexNumber);

				p.Position.Advance(match.Length);

				if (match.Length > MaxHexLength)
				{
					number.Position.Length = match.Length;
					throw new ParseException(number.Position, ParseError.NumberTooBig);
				}

				number.SetValue(NumberBase.Hex, match.Value);
			}
			else if (p.CurrentChar == '0' && p.NextChar == 'b')    // Binary number
			{
				p.Position.Advance(2);

				if (p.CharsLeft < 1)
					throw new ParseException(p.Position, ParseExpectation.BinNumber);

				regex = new Regex(@"[01]+", RegexOptions.IgnoreCase);
				match = regex.Match(p.ExpressionString, p.Position, Math.Min(p.CharsLeft, MaxBinLength + 1));

				if (!match.Success)
					throw new ParseException(p.Position, ParseExpectation.BinNumber);

				p.Position.Advance(match.Length);

				if (match.Length > MaxBinLength)
				{
					number.Position.Length = match.Length;
					throw new ParseException(number.Position, ParseError.NumberTooBig);
				}

				number.SetValue(NumberBase.Bin, match.Value);
			}
			else                // Decimal number
			{
				regex = new Regex(@"([0-9]+(\.[0-9]+)?)|\.[0-9]+", RegexOptions.IgnoreCase);
				match = regex.Match(p.ExpressionString, p.Position, p.CharsLeft);

				if (!match.Success)
					throw new ParseException(p.Position, ParseExpectation.Number);

				p.Position.Advance(match.Length);

				number.SetValue(NumberBase.Dec, match.Value);
			}

			number.Position.SetStop(p.Position);

			if (number.Value > MaxSafeInteger || number.Value < MinSafeInteger)
				p.Warning(number, WarningCode.NumberOutOfSafeRange);

			return number;
		}

		public void SetValue(NumberBase numBase, string str)
		{
			if (numBase == NumberBase.Dec)
			{
				if (double.TryParse(str, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double value))
					Value = value;
				else
					throw new ParseException(Position, ParseError.InvalidNumber);
			}
			else
				Value = Convert.ToUInt64(str, (int)numBase);

			Base = numBase;
		}

		public override string ToString()
		{
			if (Unit != null)
				return Unit.Format(this);

			return ValueString;
		}
	}

	public enum NumberBase
	{
		Bin = 2,
		Dec = 10,
		Hex = 16,
	}
}
