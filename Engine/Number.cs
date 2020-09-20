// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;
using System.Text.RegularExpressions;
using Nexcal.Engine.Errors;
using Nexcal.Engine.Units;

namespace Nexcal.Engine
{
	public class Number : Token
	{
		public const int MaxBinLength = 54;      // 100000000000000000000000000000000000000000000000000000
		public const int MaxHexLength = 14;      // 20000000000000

		public const long MaxSafeInteger = 9007199254740992;

		public Number()
		{
		}

		public NumberBase Base { get; set; } = NumberBase.Dec;

		public Unit Unit { get; set; }

		public double Value { get; set; } = 0;

		public override Number Evaluate(Calculator calculator)
		{
			return this;
		}

		public static Number Parse(Parser p, Position startPos)
		{
			var number = new Number() { Position = startPos };

			Regex regex;
			Match match;

			if (p.CurrentChar == '0' && (p.NextChar == 'x' || p.NextChar == 'X'))     // Hex number
			{
				p.Position.Advance(2);

				if (p.CharsLeft < 1)
					throw new ParserException(p.Position, ParserExpectation.HexNumber);

				regex = new Regex(@"[0-9a-f]+", RegexOptions.IgnoreCase);
				match = regex.Match(p.ExpressionString, p.Position, Math.Min(p.CharsLeft, MaxHexLength + 1));

				if (!match.Success)
					throw new ParserException(p.Position, ParserExpectation.HexNumber);

				if (match.Length > MaxHexLength)
					throw new ParserException(p.Position, ParserError.NumberTooBig);

				number.SetValue(NumberBase.Hex, match.Value);
			}
			else if (p.CurrentChar == '0' && p.NextChar == 'b')    // Binary number
			{
				p.Position.Advance(2);

				if (p.CharsLeft < 1)
					throw new ParserException(p.Position, ParserExpectation.BinNumber);

				regex = new Regex(@"[01]+", RegexOptions.IgnoreCase);
				match = regex.Match(p.ExpressionString, p.Position, Math.Min(p.CharsLeft, MaxBinLength + 1));

				if (!match.Success)
					throw new ParserException(p.Position, ParserExpectation.BinNumber);

				if (match.Length > MaxBinLength)
					throw new ParserException(p.Position, ParserError.NumberTooBig);

				number.SetValue(NumberBase.Bin, match.Value);
			}
			else
			{

			}

			return number;
		}

		public void SetValue(NumberBase numBase, string str)
		{
			if (numBase == NumberBase.Dec)
				Value = double.Parse(str);
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
