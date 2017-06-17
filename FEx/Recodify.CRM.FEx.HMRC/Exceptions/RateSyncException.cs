using System;
using System.Runtime.Serialization;

namespace Recodify.CRM.FEx.Core.Exceptions
{
	public class RateSyncException : Exception
	{
		public RateSyncException()
		{
		}

		public RateSyncException(string message) : base(message)
		{
		}

		public RateSyncException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected RateSyncException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}