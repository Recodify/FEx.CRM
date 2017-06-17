using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recodify.CRM.FEx.Core.Logging
{
	public class NullLoggingService : ILoggingService
	{
		public bool HasWarnings { get; set; }
		public void Trace(TraceEventType type, int id, params object[] message)
		{
			
		}
	}
}
