// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System.Collections.Generic;
using System.Text;
using Nexcal.Engine.Delimiters;
using Nexcal.Engine.Functions;
using Nexcal.Engine.Operators;

namespace Nexcal.Engine
{
	public class Expression : Token
	{
		public Expression(Position position) : base(position)
		{
		}

		public Anchor Anchor { get; internal set; }

		public string AnchorList
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

		public string TreeList => GetTreeList(0);

		public void Add(Token token, Parser parser = null)
		{
			if (parser != null)
				token.Position.CalculateLength(parser.Position);

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

		private string GetTreeList(int indentation)
		{
			var indent = new string(' ', indentation * 2);
			var sb = new StringBuilder();

			for (Token t = FirstToken; t != Anchor; t = t.RightToken)
			{
				if (t is Expression expr)
					sb.Append(expr.GetTreeList(indentation + 1));
				else
				{
					var str = t.ToString().Trim();
					sb.Append($"{indent}{str}".PadRight(20)).AppendLine(t.Position.ToString());
				}
			}

			return sb.ToString();
		}

		public static Expression Parse(Parser p, string stop = "")
		{
			var expression	= new Expression(p.Position);
			var stopChars	= new HashSet<char>(stop.ToCharArray());

			while (p.Position < p.ExpressionString.Length)
			{
				char chr = p.CurrentChar;

				if (stopChars.Contains(chr))
					break;
				else if (char.IsWhiteSpace(chr))
				{
					if (chr == '\n')
						p.Position.NewLine();
					else
						p.Position.Advance();

					continue;
				}

				Token token = null;

				if (char.IsDigit(chr) || chr == '.')
					token = Number.Parse(p);
				else if (Operator.Chars.Contains(chr))
					token = Operator.Parse(p);
				else if (char.IsLetter(chr))
					token = p.ParseIdentifier();
				else if (chr == '(')
					token = RoundOpeningBracket.Parse(p);
				else
				{
					// TODO: Unsupported char
				}

				expression.Add(token, p);

				if (token is FunctionCall call)
				{
					call.ParseArguments(p);
					call.Position.CalculateLength(p.Position);
				}
				else if (token is RoundOpeningBracket)
				{
					expression.Add(Parse(p, ")"), p);
					expression.Add(RoundClosingBracket.Parse(p), p);
				}
			}

			expression.Position.CalculateLength(p.Position);

			return expression;
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
