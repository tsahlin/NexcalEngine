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

			return Expression.Parse(this);
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
