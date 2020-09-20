// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

namespace Nexcal.Engine.Errors
{
	public class ParserException : NexcalException
	{
		public ParserException(Position position, ParserError error)
		{
			Position	= position;
			Error		= error;
		}

		public ParserException(Position position, ParserExpectation expectation)
		{
			Position	= position;
			Expectation	= expectation;
		}

		public ParserError Error { get; set; } = ParserError.Syntax;

		public ParserExpectation Expectation { get; set; } = ParserExpectation.None;

		public Position Position { get; set; }
	}

	public enum ParserError
	{
		NumberTooBig,
		Syntax
	}

	public enum ParserExpectation
	{
		None,
		BinNumber,
		HexNumber,
		Number
	}
}
