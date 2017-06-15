using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace Recodify.CRM.FEx.Core.Logging
{
	public class LoggingService : ILoggingService
	{
		private readonly ITracingService tracingService;

		public LoggingService(ITracingService tracingService)
		{
			this.tracingService = tracingService;
		}

		public void Trace(TraceEventType type, int id, string message)
		{
			tracingService.Trace($"{type.ToString().ToUpper()}: {message}");
		}
	}
}
