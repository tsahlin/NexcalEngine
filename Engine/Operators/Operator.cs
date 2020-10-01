// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;
using System.Collections.Generic;
using Nexcal.Engine.Errors;

namespace Nexcal.Engine.Operators
{
	public abstract class Operator : Token
	{
		private const string chars = "+-*/";

		public Operator(Position position) : base(position)
		{
		}

		public static HashSet<char> Chars { get; private set; } = new HashSet<char>(chars.ToCharArray());

		internal static void InitIdentifierMap(Dictionary<string, Type> map)
		{
			map["mod"]	= typeof(Modulo);
		}

		public static Operator Parse(Parser p)
		{
			Operator op;

			switch (p.CurrentChar)
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
			op.Position.CalculateLength(p.Position);

			return op;
		}
	}
}
