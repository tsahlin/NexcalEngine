// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using System;
using System.Collections.Generic;
using Nexcal.Engine.Errors;

namespace Nexcal.Engine
{
	public class Calculator
	{
		static Calculator()
		{
			Parser.Init();
		}

		public Calculator()
		{
		}

		public Action<string> DebugLogger { get; set; }

		public List<Warning> Warnings { get; } = new List<Warning>();

		public Number Calculate(Expression expression)
		{
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

		internal void DebugEvaluate(Token token)
		{
			if (DebugLogger == null)
				return;

			DebugLogger($"Evaluating {token.Name} token at {token.Position}: {token}");
		}

		public Expression Parse(string expression)
		{
			return Parser.Parse(this, expression);
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
