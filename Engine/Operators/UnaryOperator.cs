// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

namespace Nexcal.Engine.Operators
{
	public abstract class UnaryOperator : Operator
	{
		internal UnaryOperator(Position position) : base(position)
		{
		}

		public override Precedence Precedence => Precedence.Unary;
	}
}
