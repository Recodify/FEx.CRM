using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace Recodify.CRM.FEx.Core.Logging
{
	public class GenericLoggingService : ILoggingService
	{		
		private readonly TraceSource traceSource;

		public bool HasWarnings { get; set; }

		public GenericLoggingService(string traceSourceName)
		{			
			this.traceSource = new TraceSource(traceSourceName);
		}

		public void Trace(TraceEventType type, int id, params object[] message)
		{
			if (type == TraceEventType.Warning)
			{
				HasWarnings = true;
			}
			
			traceSource.TraceData(type, id, message);
		}
	}

	public class DynamicsLoggingService : ILoggingService
	{
		private readonly ITracingService tracingService;
		private readonly TraceSource traceSource;		

		public bool HasWarnings { get; set; }

		public DynamicsLoggingService(ITracingService tracingService)
		{
			this.tracingService = tracingService;
			this.traceSource = new TraceSource("Details");
		}

		public void Trace(TraceEventType type, int id, params object[] message)
		{
			if (type == TraceEventType.Warning)
			{
				HasWarnings = true;
			}

			tracingService.Trace($"{type.ToString().ToUpper()}: {message}");
			traceSource.TraceData(type, id, message);
		}
	}
}
