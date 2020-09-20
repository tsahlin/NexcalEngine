using System;

namespace Nexcal.Engine.Errors
{
	public class NexcalException : Exception
	{
		public NexcalException()
		{
		}

		public NexcalException(string message) : base(message)
		{
		}
	}
}
