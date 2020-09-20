// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

namespace Nexcal.Engine
{
	public enum Precedence
	{
		/// <summary>Function calls, units</summary>
		Primary,

		/// <summary>+x, -x, x!</summary>
		Unary,

		/// <summary>x * y, x / y, x mod y</summary>
		Multiplicative,

		/// <summary>x + y, x – y</summary>
		Additive,

		/// <summary>x << y, x >> y</summary>
		Shift,

		/// <summary>x < y, x > y, x <= y, x >= y</summary>
		Relational,

		/// <summary>x == y, x != y, x <> y</summary>
		Equality,

		/// <summary>Numbers</summary>
		Number,

		None
	}
}
