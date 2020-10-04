// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

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
	}
}
