using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
