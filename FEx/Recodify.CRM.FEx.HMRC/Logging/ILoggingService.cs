using System.Diagnostics;

namespace Recodify.CRM.FEx.Core.Logging
{
	public interface ILoggingService
	{
		bool HasWarnings { get; set; }
		void Trace(TraceEventType type, int id, params object[] message);
	}
}