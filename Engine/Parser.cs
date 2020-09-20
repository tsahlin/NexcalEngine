// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Nexcal.Engine.Errors;

namespace Nexcal.Engine
{
	public class Parser
	{
		internal Parser(string expression)
		{
			ExpressionString = expression;
		}

		public int CharsLeft => Math.Max(ExpressionString.Length - Position, 0);

		public char CurrentChar => CharsLeft >= 1 ? ExpressionString[Position] : '\0';

		public string ExpressionString { get; set; }

		public char NextChar => CharsLeft >= 2 ? ExpressionString[Position + 1] : '\0';

		public Position Position { get; } = new Position();

		public static Expression Parse(string expression)
		{
			var parser = new Parser(expression);

			return parser.ParseSubExpression();
		}

		private Expression ParseSubExpression(string stop = "")
		{
			var stopChars	= new HashSet<char>(stop.ToCharArray());
			var expression	= new Expression() { Position = Position };

			while (Position < ExpressionString.Length)
			{
				char chr = CurrentChar;

				if (stopChars.Contains(chr))
					break;
				else if (char.IsWhiteSpace(chr))
				{
					if (chr == '\n')
						Position.NewLine();
					else
						Position.Advance();

					continue;
				}

				Token token = null;

				if (char.IsDigit(chr) || chr == '.')
					token = Number.Parse(this, Position.Clone);
				else
				{
					// Unsupported char
				}

				token.Position.Length = Position - token.Position;

				expression.Add(token);
			}

			expression.Position.Length = Position - expression.Position;

			return expression;
		}
	}
}
