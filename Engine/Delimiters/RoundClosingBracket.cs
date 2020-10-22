// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using Nexcal.Engine.Errors;

namespace Nexcal.Engine.Delimiters
{
	public class RoundClosingBracket : Delimiter
	{
		public RoundClosingBracket(Position position) : base(position)
		{
		}

		public static RoundClosingBracket Parse(Parser p)
		{
			var bracket = new RoundClosingBracket(p.Position);

			if (p.CurrentChar == ')')
				p.Position.Advance();
			else
				throw new ParseException(p.Position, ParseExpectation.RoundClosingBracket);

			return bracket;
		}

		public override string ToString()
		{
			return ")";
		}
	}
}
