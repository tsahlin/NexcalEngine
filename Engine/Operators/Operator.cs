// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

namespace Nexcal.Engine.Operators
{
	public abstract class Operator : Token
	{
		public Operator(Position position) : base(position)
		{
		}
	}
}
