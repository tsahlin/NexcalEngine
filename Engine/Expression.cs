// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;
using System.Collections.Generic;
using System.Text;

namespace Nexcal.Engine
{
	public class Expression : Token
	{
		public Expression(Position position) : base(position)
		{
		}

		public Anchor Anchor { get; internal set; }

		public string DebugList
		{
			get
			{
				var sb = new StringBuilder();

				if (Anchor == null)
				{
					sb.Append("<null>");

					return sb.ToString();
				}
				else if (Anchor.LeftToken == null)
				{
					sb.Append($"<null> <--- {Anchor.Name} ---> ");
					sb.Append(Anchor.RightToken == null ? "<null>" : Anchor.Name);

					return sb.ToString();
				}

				sb.AppendLine($"| <--- {Anchor.Name}");
				sb.AppendLine("|");

				Token token		= Anchor.LeftToken;
				Token lastToken = Anchor;

				while (token != Anchor)
				{
					if (token.LeftToken == lastToken)
						sb.Append("| <--> ");
					else
						sb.Append("| <error> ");

					sb.AppendLine(token.Name);

					lastToken	= token;
					token		= token.RightToken;
				}

				sb.AppendLine("|");
				sb.Append($"| ---> {lastToken.RightToken.Name}");

				return sb.ToString();
			}
		}

		public Token FirstToken => Anchor?.LeftToken;

		public Token LastToken => Anchor?.RightToken;

		public string TokenNames
		{
			get
			{
				var sb = new StringBuilder();

				for (Token t = FirstToken; t != Anchor; t = t.RightToken)
					sb.Append(t.Name).Append(",");

				if (sb.Length > 0)
					sb.Length--;

				return sb.ToString();
			}
		}

		public void Add(Token token)
		{
			if (Anchor == null)
			{
				Anchor = new Anchor(Position);

				token.LeftToken		= Anchor;
				Anchor.LeftToken	= token;
			}
			else
			{
				token.LeftToken			= LastToken;
				LastToken.RightToken	= token;
			}

			token.RightToken	= Anchor;
			Anchor.RightToken	= token;
		}

		public override Number Evaluate(Calculator calc)
		{
			throw new NotImplementedException();
		}

		public List<Token> GetList()
		{
			var list = new List<Token>();

			for (Token t = FirstToken; t != Anchor; t = t.RightToken)
			{
				list.Add(t);
			}

			return list;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			for (Token t = FirstToken; t != Anchor; t = t.RightToken)
				sb.Append(t.ToString());

			return sb.ToString();
		}
	}

	/*
	- List med tokens
	- SortedDictionary med prio => Lista med tokens att utvärdera (operator, sub-expression, unit)
	- Number innehåller value och unit
	- BinaryOperator (1+2), PreOperator (ex -5), PostOperator (ex fakultet 7!, enhet 5m)
	- Enheter ses som UnaryOperator. Deras operation är att tilldela unit till dess Number.
		Efter utförande ersätts number+unit tokens av endast number token
	- Token properties
		Position - rad och kolumn i det ursprungliga uttrycket, sätts av parsern
		LeftNumber - tal eller utvärderat uttryck till vänster
		RightNumber - tal eller utvärderat uttryck till höger
	*/
}
