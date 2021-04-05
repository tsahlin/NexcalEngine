// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

namespace Nexcal.Engine
{
	public class Position
	{
		/// <summary>Zero based index in the expression string</summary>
		public int Index { get; set; } = 0;

		/// <summary>Number of characters</summary>
		public int Length { get; set; } = 0;

		/// <summary>Line number starting at 1</summary>
		public int Line { get; set; } = 1;

		/// <summary>Column number starting at 1</summary>
		public int Column { get; set; } = 1;

		public Position Clone => (Position)MemberwiseClone();

		public static implicit operator int(Position pos)
		{
			return pos.Index;
		}

		public int Advance(int chars = 1)
		{
			Column += chars;

			return Index += chars;
		}

		public int NewLine()
		{
			Index++;
			Column = 0;

			return ++Line;
		}

		public void SetRange(Position start, Position end)
		{
			Index = start.Index;
			Column = start.Column;
			Line = start.Line;
			Length = end.Index + end.Length - Index;
		}

		public void SetStop(Position end)
		{
			Length = end.Index - Index;
		}

		public override string ToString()
		{
			return $"Index {Index}: Column {Column} on Line {Line}";
		}
	}
}
