using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Recodify.CRM.FEx.Core.Logging;

namespace Recodify.CRM.FEx.Tests.Framework
{
	public class DebugLoggingService : ILoggingService
	{
		public DebugLoggingService()
		{
			Logs = new List<string>();
		}

		public List<string> Logs { get; }

		public void Trace(TraceEventType type, int id, params object[] message)
		{
			var formattedMessage = message.Select(x => x.ToString()).Aggregate((c, n) => c + "," + n);
			var msg = $"{type.ToString().ToUpper()}({id}): {formattedMessage}";
			Logs.Add(msg);
			Debug.WriteLine(msg);
		}

		public bool HasWarnings { get; set; }
	}
}