// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Nexcal.Engine.Errors;
using Nexcal.Engine.Operators;

namespace Nexcal.Engine
{
	public class Parser
	{
		internal Parser(string expression)
		{
			ExpressionString = expression;
		}

		internal int CharsLeft => Math.Max(ExpressionString.Length - Position, 0);

		internal char CurrentChar => CharsLeft >= 1 ? ExpressionString[Position] : '\0';

		internal string ExpressionString { get; set; }

		internal char NextChar => CharsLeft >= 2 ? ExpressionString[Position + 1] : '\0';

		internal ParseResult Result { get; } = new ParseResult();

		internal Position Position { get; } = new Position();

		public static ParseResult Parse(string expression)
		{
			var parser = new Parser(expression);

			parser.Result.Expression = parser.ParseSubExpression();

			return parser.Result;
		}

		private Expression ParseSubExpression(string stop = "")
		{
			var stopChars	= new HashSet<char>(stop.ToCharArray());
			var expression	= new Expression(Position);

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
					token = Number.Parse(this);
				else if (Operator.Chars.Contains(chr))
					token = Operator.Parse(this);
				else
				{
					// TODO: Unsupported char
				}

				token.Position.CalculateLength(Position);

				expression.Add(token);
			}

			expression.Position.CalculateLength(Position);

			return expression;
		}

		public void Warning(Position position, ParseWarning warning)
		{
			Result.Warnings.Add(new ParseWarningItem { Position = position.Clone, Warning = warning });
		}
	}

	public class ParseResult
	{
		public Expression Expression { get; internal set; }

		public List<ParseWarningItem> Warnings { get; } = new List<ParseWarningItem>();
	}

	public class ParseWarningItem
	{
		public Position Position { get; internal set; }

		public ParseWarning Warning { get; internal set; }
	}
}
