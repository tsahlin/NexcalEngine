// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using Nexcal.Engine.Errors;
using System;
using System.Collections.Generic;

namespace Nexcal.Engine
{
	public abstract class Token
	{
		public Token(Position position)
		{
			Position = position.Clone;
		}

		public bool IsLinked => LeftToken != null || RightToken != null;

		public Token LeftToken { get; internal set; }

		public string Name => GetType().Name;

		public Position Position { get; internal set; }

		public virtual Precedence Precedence => Precedence.None;

		public Token RightToken { get; internal set; }

		internal Token CloneToken()
		{
			var clone = (Token)MemberwiseClone();

			clone.LeftToken = null;
			clone.RightToken = null;

			return clone;
		}

		protected static void DisposeTokens(List<Token> tokens)
		{
			foreach (var t in tokens)
			{
				t.LeftToken = null;
				t.RightToken = null;
			}
		}

		internal abstract Number Evaluate(Calculator calc);

		internal virtual Token PreProcess(Parser parser)
		{
			return this;
		}

		internal void Replace(Token token)
		{
			Replace(token, token);
		}

		internal void Replace(Token start, Token end)
		{
			Position.SetRange(start.Position, end.Position);

			// Collect the tokens that we are replacing, so that they can be disposed of
			var gc = new List<Token>();

			for (Token t = start; t != null; t = t.RightToken)
			{
				if (t != this)
					gc.Add(t);

				if (t == end)
					break;
			}

			// Replace tokens
			if (start.LeftToken != null)
				start.LeftToken.RightToken = this;

			if (end.RightToken != null)
				end.RightToken.LeftToken = this;

			LeftToken	= start.LeftToken;
			RightToken	= end.RightToken;

			// Unlink the replaced tokens
			DisposeTokens(gc);
		}

		protected Number RequireLeftNumber(Calculator calc)
		{
			VerifyLeftNumber();

			return LeftToken.Evaluate(calc);
		}

		protected Number RequireRightNumber(Calculator calc)
		{
			VerifyRightNumber();

			return RightToken.Evaluate(calc);
		}

		protected void VerifyLeftNumber()
		{
			if (LeftToken is Number || LeftToken is Expression)
				return;

			throw new CalculatorException(this, CalculatorError.LeftNumberRequired);
		}

		protected void VerifyRight<T>(CalculatorError error)
		{
			if (!(RightToken is T))
                throw new CalculatorException(this, error);
		}

		protected void VerifyRightNumber()
		{
			if (RightToken is Number || RightToken is Expression)
				return;

			throw new CalculatorException(this, CalculatorError.RightNumberRequired);
		}
	}
}
