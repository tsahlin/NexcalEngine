// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;
using System.Collections.Generic;
using Nexcal.Engine.Errors;

namespace Nexcal.Engine.Operators
{
	public abstract class Operator : Token
	{
		public Operator(Position position) : base(position)
		{
		}

		internal static Dictionary<char, char> Chars { get; private set; }

		internal static void InitIdentifierMap(Dictionary<string, Type> map)
		{
			Chars = new Dictionary<char, char>();

			Chars.Add('+', '+');
			Chars.Add('-', '-');
			Chars.Add((char)8722, '-');		// Unicode minus

			map["as"]	= typeof(As);
			map["mod"]	= typeof(Modulo);
		}

		public static Operator Parse(Parser p)
		{
			Operator op;

			if (!Chars.TryGetValue(p.CurrentChar, out char chr))
				chr = '\0';

			switch (chr)
			{
				case '+':
					op = new Add(p.Position);
					break;

				case '-':
					op = new Subtract(p.Position);
					break;

				default:
					throw new ParseException(p.Position, ParseExpectation.Operator);
			}

			p.Position.Advance();
			op.Position.SetStop(p.Position);

			return op;
		}
	}
}
