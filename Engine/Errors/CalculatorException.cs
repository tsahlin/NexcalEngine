// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

namespace Nexcal.Engine.Errors
{
	public class CalculatorException : NexcalException
	{
		public CalculatorException(Token token, CalculatorError error)
		{
			Token	= token;
			Error	= error;
		}

		public CalculatorError Error { get; set; } = CalculatorError.Unknown;

		public Token Token { get; set; }
	}

	public enum CalculatorError
	{
		Unknown = 0,            // Default
		CannotAddUnits,
		EvaluationResidue,
		LeftNumberRequired,
		NonNumberResult,
		RightNumberRequired,
		UnitAlreadyAssigned
	}
}
