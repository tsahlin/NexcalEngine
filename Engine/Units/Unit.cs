using System;

namespace Nexcal.Engine.Units
{
	public abstract class Unit : Token
	{
		public override Number Evaluate(Calculator calc)
		{
			// Sätt left token Number.Unit = this
			// Ersätt left token + this med bara left token
			// Om number redan har en unit = error
			throw new NotImplementedException();
		}

		public abstract string Format(Number number);
	}
}
