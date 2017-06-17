using System;
using System.Runtime.Serialization;

namespace Recodify.CRM.FEx.Core.Exceptions
{
	public class SchedulingException : Exception
	{
		public SchedulingException()
		{
		}

		public SchedulingException(string message) : base(message)
		{
		}

		public SchedulingException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected SchedulingException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}