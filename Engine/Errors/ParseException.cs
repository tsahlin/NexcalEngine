// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

namespace Nexcal.Engine.Errors
{
	public class ParseException : NexcalException
	{
		public ParseException(Position position, ParseError error)
		{
			Position	= position.Clone;
			Error		= error;
		}

		public ParseException(Position position, ParseExpectation expectation)
		{
			Position	= position.Clone;
			Expectation	= expectation;
		}

		public ParseError Error { get; set; } = ParseError.Syntax;

		public ParseExpectation Expectation { get; set; } = ParseExpectation.None;

		public Position Position { get; set; }
	}

	public enum ParseError
	{
		InvalidNumber,
		NumberTooBig,
		Syntax,
		UnknownIdentifier
	}

	public enum ParseExpectation
	{
		None,
		BinNumber,
		HexNumber,
		Identifier,
		Number,
		Operator
	}
}
