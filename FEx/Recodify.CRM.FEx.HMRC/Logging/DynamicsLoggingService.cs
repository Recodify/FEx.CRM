using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Recodify.CRM.FEx.Core.Config;
using Recodify.Logging.Dynamics;

namespace Recodify.CRM.FEx.Core.Logging
{
	public class DynamicsLoggingService : ILoggingService
	{
		private readonly LogglyRestrictedDirectListener logglyTrace;
		private readonly string logglyUrl;
		private readonly TraceSource traceSource;
		private readonly ITracingService tracingService;
		private readonly string uniqueName;
		private readonly Guid workflowId;

		public DynamicsLoggingService(ITracingService tracingService, string uniqueName, Guid workflowId)
		{
			this.tracingService = tracingService;
			this.uniqueName = uniqueName;
			this.workflowId = workflowId;
			traceSource = new TraceSource("Details");
			logglyUrl = new PluginConfiguration().LogglyUrl;
			logglyTrace = new LogglyRestrictedDirectListener("Details", "FEx Dynamics Plugin", uniqueName, logglyUrl, workflowId);
		}

		public bool HasWarnings { get; set; }

		public void Trace(TraceEventType type, int id, params object[] message)
		{
			if (type == TraceEventType.Warning)
				HasWarnings = true;

			//var formattedMessage = message.Select(x => x.ToString()).Aggregate((c, n) => c + Environment.NewLine + n);
			//tracingService.Trace($"{type} ({id}): {formattedMessage}");
			logglyTrace.TraceData(type, id, message);
		}
	}
}