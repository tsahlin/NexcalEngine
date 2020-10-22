// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

using Nexcal.Engine.Errors;

namespace Nexcal.Engine.Delimiters
{
	public class RoundOpeningBracket : Delimiter
	{
		public RoundOpeningBracket(Position position) : base(position)
		{
		}

		public static RoundOpeningBracket Parse(Parser p)
		{
			var bracket = new RoundOpeningBracket(p.Position);

			if (p.CurrentChar == '(')
				p.Position.Advance();
			else
				throw new ParseException(p.Position, ParseExpectation.RoundOpeningBracket);

			return bracket;
		}

		public override string ToString()
		{
			return "(";
		}
	}
}
