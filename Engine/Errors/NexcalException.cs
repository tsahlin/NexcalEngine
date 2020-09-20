// Nexcal math engine library
// MIT License - https://github.com/tsahlin/NexcalEngine

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
