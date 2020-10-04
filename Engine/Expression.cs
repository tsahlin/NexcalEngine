// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

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

		private SortedDictionary<Precedence, List<Token>> PrecedenceMap { get; set; }

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

		internal override Number Evaluate(Calculator calc)
		{
			foreach (var item in PrecedenceMap)
			{
				foreach (var token in item.Value)
					token.Evaluate(calc);
			}

			// TODO: Kolla att vi bara har EN token kvar, som ska vara Number
			// Kör calc.Replace(this, med den number token)

			return (Number)FirstToken;
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

		/// <summary>Pre processes and creates a map of tokens ordered by precedence</summary>
		internal override Token PreProcess(Calculator calc)
		{
			PrecedenceMap = new SortedDictionary<Precedence, List<Token>>();

			// TODO: Omvandla binary add/subtract till unary ifall de följer en annan operator t.ex.
			// eller om de är först i expression

			for (Token t = FirstToken; t != Anchor; t = t.RightToken)
				t = t.PreProcess(calc);

			for (Token t = FirstToken; t != Anchor; t = t.RightToken)
			{
				if (!PrecedenceMap.ContainsKey(t.Precedence))
					PrecedenceMap[t.Precedence] = new List<Token>();

				PrecedenceMap[t.Precedence].Add(t);
			}

			return this;
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
