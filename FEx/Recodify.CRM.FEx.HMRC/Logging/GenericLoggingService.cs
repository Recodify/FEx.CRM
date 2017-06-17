using System.Diagnostics;

namespace Recodify.CRM.FEx.Core.Logging
{
	public class GenericLoggingService : ILoggingService
	{
		private readonly TraceSource traceSource;

		public GenericLoggingService(string traceSourceName)
		{
			traceSource = new TraceSource(traceSourceName);
		}

		public bool HasWarnings { get; set; }

		public void Trace(TraceEventType type, int id, params object[] message)
		{
			if (type == TraceEventType.Warning)
				HasWarnings = true;

			traceSource.TraceData(type, id, message);
		}
	}
}