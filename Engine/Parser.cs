// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Nexcal.Engine.Errors;
using Nexcal.Engine.Functions;
using Nexcal.Engine.Operators;
using Nexcal.Engine.Units;

namespace Nexcal.Engine
{
	public class Parser
	{
		private Parser(Calculator calc, string expression)
		{
			Calculator			= calc;
			ExpressionString	= expression;
			Position			= new Position();
		}

		private Calculator Calculator { get; }

		internal int CharsLeft => Math.Max(ExpressionString.Length - Position, 0);

		internal char CurrentChar => CharsLeft >= 1 ? ExpressionString[Position] : '\0';

		internal string ExpressionString { get; private set; }

		private static Dictionary<string, Type> IdentifierMap { get; set; }

		internal char NextChar => CharsLeft >= 2 ? ExpressionString[Position + 1] : '\0';

		internal Position Position { get; private set; }

		internal static void Init()
		{
			IdentifierMap = new Dictionary<string, Type>();

			FunctionCall.InitIdentifierMap(IdentifierMap);
			Operator.InitIdentifierMap(IdentifierMap);
			Unit.InitIdentifierMap(IdentifierMap);
		}

		internal static Expression Parse(Calculator calc, string expression)
		{
			var p = new Parser(calc, expression);

			return p.ParseExpression();
		}

		internal Expression ParseExpression(string stop = "", Expression expression = null)
		{
			var stopChars = new HashSet<char>(stop.ToCharArray());

			if (expression == null)
				expression = new Expression(Position);

			while (CharsLeft > 0)
			{
				SkipWhiteSpace();

				char chr = CurrentChar;

				if (stopChars.Contains(chr))
					break;

				Token token = null;

				if (char.IsDigit(chr) || chr == '.')
					token = Number.Parse(this);
				else if (Operator.Chars.ContainsKey(chr))
					token = Operator.Parse(this);
				else if (char.IsLetter(chr))
					token = ParseIdentifier();
				else if (chr == '(')
					token = RoundBracketExpression.Parse(this);
				else
				{
					// TODO: Handle unsupported char
					System.Console.WriteLine((int)chr);
					throw new Exception($"Unsupported char: {chr}");
				}

				expression.Add(token, this);

				if (token is FunctionCall call)
				{
					call.ParseArguments(this);
					call.Position.SetStop(Position);
				}
			}

			expression.Position.SetStop(Position);

			return (Expression)expression.PreProcess(this);
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
