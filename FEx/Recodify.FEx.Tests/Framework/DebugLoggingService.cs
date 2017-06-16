using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recodify.CRM.FEx.Core.Logging;

namespace Recodify.CRM.FEx.Tests.Framework
{
	public class DebugLoggingService : ILoggingService
	{
		public List<string> Logs { get;}

		public DebugLoggingService()
		{
			Logs = new List<string>();
		}

		public void Trace(TraceEventType type, int id, params object[] message)
		{
			var msg = $"{type.ToString().ToUpper()}({id}): {message.Aggregate((c, n) => c + "," + n)}";
			Logs.Add(msg);
			Debug.WriteLine(msg);
		}

		public bool HasWarnings { get; set; }
	}
}
