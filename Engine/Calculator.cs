// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System.Collections.Generic;
using Nexcal.Engine.Errors;

namespace Nexcal.Engine
{
	public class Calculator
	{
		public Calculator()
		{
		}

		public List<Warning> Warnings { get; } = new List<Warning>();

		public Number Calculate(Expression expression)
		{
			expression.PreProcess(this);

			return expression.Evaluate(this);
		}

		public Number Calculate(string expression)
		{
			return Calculate(Parse(expression));
		}

		public void Clear()
		{
			Warnings.Clear();
		}

		public Expression Parse(string expression)
		{
			var parser = new Parser(this);

			return parser.Parse(expression);
		}

		internal void Replace(Token token, Token replacement)
		{
			Replace(token, token, replacement);
		}

		internal void Replace(Token start, Token end, Token replacement)
		{
			if (start.LeftToken != null)
				start.LeftToken.RightToken = replacement;

			if (end.RightToken != null)
				end.RightToken.LeftToken = replacement;

			replacement.LeftToken	= start.LeftToken;
			replacement.RightToken	= end.RightToken;
		}

		internal void Warning(Position position, WarningCode warning)
		{
			Warnings.Add(new Warning(position, warning));
		}

		internal void Warning(Token token, WarningCode warning)
		{
			Warning(token.Position, warning);
		}
	}
}
