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

		public Number(Position position) : base(position)
		{
		}

		public NumberBase Base { get; set; } = NumberBase.Dec;

		public override Precedence Precedence => Precedence.Number;

		public Unit Unit { get; set; }

		public double Value { get; set; } = 0;

		internal override Number Evaluate(Calculator calc)
		{
			return this;
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

			number.Position.CalculateLength(p.Position);

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

			return Value.ToString();
		}
	}

	public enum NumberBase
	{
		Bin = 2,
		Dec = 10,
		Hex = 16,
	}
}
