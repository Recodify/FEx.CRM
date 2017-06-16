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

		public bool HasWarnings { get; set; }

		public LoggingService(ITracingService tracingService)
		{
			this.tracingService = tracingService;
		}

		public void Trace(TraceEventType type, int id, string message)
		{
			if (type == TraceEventType.Warning)
			{
				HasWarnings = true;
			}

			tracingService.Trace($"{type.ToString().ToUpper()}: {message}");
		}
	}
}
