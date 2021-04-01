// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Nexcal.Engine.Delimiters;
using Nexcal.Engine.Errors;
using Nexcal.Engine.Functions;
using Nexcal.Engine.Operators;
using Nexcal.Engine.Units;

namespace Nexcal.Engine
{
	public class Parser
	{
		private static Dictionary<string, Type> identifierMap;

		internal Parser(Calculator calc)
		{
			Calculator = calc;
		}

		private Calculator Calculator { get; }

		internal int CharsLeft => Math.Max(ExpressionString.Length - Position, 0);

		internal char CurrentChar => CharsLeft >= 1 ? ExpressionString[Position] : '\0';

		internal string ExpressionString { get; private set; }

		private static Dictionary<string, Type> IdentifierMap
		{
			get
			{
				if (identifierMap == null)
				{
					identifierMap = new Dictionary<string, Type>();

					FunctionCall.InitIdentifierMap(identifierMap);
					Operator.InitIdentifierMap(identifierMap);
					Unit.InitIdentifierMap(identifierMap);
				}

				return identifierMap;
			}
		}

		internal char NextChar => CharsLeft >= 2 ? ExpressionString[Position + 1] : '\0';

		internal Position Position { get; private set; }

		public Expression Parse(string expression)
		{
			ExpressionString	= expression;
			Position			= new Position();

			return ParseExpression();
		}

		internal Expression ParseExpression(string stop = "")
		{
			var expression	= new Expression(Position);
			var stopChars	= new HashSet<char>(stop.ToCharArray());

			while (CharsLeft > 0)
			{
				SkipWhiteSpace();

				char chr = CurrentChar;

				if (stopChars.Contains(chr))
					break;

				Token token = null;

				if (char.IsDigit(chr) || chr == '.')
					token = Number.Parse(this);
				else if (Operator.Chars.Contains(chr))
					token = Operator.Parse(this);
				else if (char.IsLetter(chr))
					token = ParseIdentifier();
				else if (chr == '(')
					token = RoundOpeningBracket.Parse(this);
				else
				{
					// TODO: Unsupported char
				}

				expression.Add(token, this);

				if (token is FunctionCall call)
				{
					call.ParseArguments(this);
					call.Position.CalculateLength(Position);
				}
				else if (token is RoundOpeningBracket)
				{
					expression.Add(ParseExpression(")"), this);
					expression.Add(RoundClosingBracket.Parse(this), this);
				}
			}

			expression.Position.CalculateLength(Position);

			return expression;
		}

		internal Token ParseIdentifier()
		{
			var regex = new Regex(@"[a-z][0-9a-z]*", RegexOptions.IgnoreCase);
			var match = regex.Match(ExpressionString, Position);

			if (!match.Success)
				throw new ParseException(Position, ParseExpectation.Identifier);

			var startPos	= Position.Clone;
			var identifier	= match.Value;

			Position.Advance(match.Length);

			if (IdentifierMap.TryGetValue(identifier, out Type tokenType))
			{
				return (Token)Activator.CreateInstance(tokenType, new object[] { startPos });
			}

			throw new ParseException(startPos, ParseError.UnknownIdentifier);
		}

		internal void SkipWhiteSpace()
		{
			while (CharsLeft > 0)
			{
				char chr = CurrentChar;

				if (!char.IsWhiteSpace(chr))
					break;

				if (chr == '\n')
					Position.NewLine();
				else
					Position.Advance();
			}
		}

		internal void Warning(Position position, WarningCode warning)
		{
			Calculator?.Warning(position, warning);
		}

		internal void Warning(Token token, WarningCode warning)
		{
			Calculator?.Warning(token, warning);
		}
	}
}
