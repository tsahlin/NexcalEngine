// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

namespace Nexcal.Engine
{
	public class Anchor : Token
	{
		public Anchor(Position position) : base(position)
		{
		}

		internal override Number Evaluate(Calculator calc)
		{
			return (Number)LeftToken;
		}
	}
}
