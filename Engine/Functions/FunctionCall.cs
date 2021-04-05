// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;
using System.Collections.Generic;
using System.Text;
using Nexcal.Engine.Delimiters;
using Nexcal.Engine.Errors;

namespace Nexcal.Engine.Functions
{
	public abstract class FunctionCall : Token
	{
		public FunctionCall(Position position) : base(position)
		{
		}

		protected List<Token> Args { get; } = new List<Token>();

		protected string ArgString
		{
			get
			{
				var sb = new StringBuilder();

				foreach (var arg in Args)
					sb.Append(arg.ToString()).Append(", ");

				if (sb.Length > 0)
					sb.Length -= 2;

				return sb.ToString();
			}
		}

		public override Precedence Precedence => Precedence.Primary;

		internal override Number Evaluate(Calculator calc)
		{
			calc.DebugEvaluate(this);
			
			throw new NotImplementedException();
		}

		internal static void InitIdentifierMap(Dictionary<string, Type> map)
		{
			map["sin"]	= typeof(Sine);
		}

		internal void ParseArguments(Parser p)
		{
			p.SkipWhiteSpace();
			RoundOpeningBracket.Parse(p);

			while (p.CharsLeft > 0)
			{
				var arg = p.ParseExpression(",)");

				if (arg.IsEmpty)
				{
					if (Args.Count > 0 || p.CurrentChar == ',')
						throw new ParseException(p.Position, ParseExpectation.Expression);

					break;
				}

				Args.Add(arg);

				if (p.CurrentChar != ',')
					break;
				else if (p.CharsLeft == 1)			// , is the last character
					throw new ParseException(p.Position, ParseExpectation.Expression);

				p.Position.Advance();
			}

			RoundClosingBracket.Parse(p);
		}
	}
}
