namespace Nexcal.Engine.Errors
{
	public class Warning
	{
		public Warning(Position position, WarningCode code)
		{
			Position	= position.Clone;
			Code		= code;
		}

		public WarningCode Code { get; set; }

		public Position Position { get; set; }
	}

	public enum WarningCode
	{
		NumberOutOfSafeRange,
		ResultOutOfSafeRange
	}
}
