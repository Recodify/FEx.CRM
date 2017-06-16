using System.Diagnostics;

namespace Recodify.CRM.FEx.Core.Logging
{
	public interface ILoggingService
	{
		void Trace(TraceEventType type, int id, params object[] message);
		bool HasWarnings { get; set; }
	}
}