// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using Nexcal.Engine.Errors;

namespace Nexcal.Engine
{
	public abstract class Token
	{
		public Token(Position position)
		{
			Position = position.Clone;
		}

		public Token LeftToken { get; internal set; }

		public string Name => GetType().Name;

		public Position Position { get; internal set; }

		public virtual Precedence Precedence => Precedence.None;

		public Token RightToken { get; internal set; }

		internal abstract Number Evaluate(Calculator calc);

		internal virtual Token PreProcess(Calculator calc)
		{
			return this;
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

		protected void VerifyRightNumber()
		{
			if (RightToken is Number || RightToken is Expression)
				return;

			throw new CalculatorException(this, CalculatorError.RightNumberRequired);
		}
	}
}
