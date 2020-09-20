﻿namespace Nexcal.Engine
{
	public abstract class Token
	{
		public Token LeftToken { get; internal set; }

		public Position Position { get; internal set; }

		public virtual Precedence Precedence => Precedence.None;

		public Token RightToken { get; internal set; }

		public abstract Number Evaluate(Calculator calc);
	}
}
