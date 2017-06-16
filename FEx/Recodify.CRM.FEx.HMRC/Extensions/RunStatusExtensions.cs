using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Monitoring;

namespace Recodify.CRM.FEx.Core.Extensions
{
	public static class RunStatusExtensions
	{
		public static TraceEventType ToTraceEventType(this RunStatus runStatus)
		{
			switch (runStatus)
			{
				case RunStatus.Success:
					return TraceEventType.Information;
				case RunStatus.Warning:
					return TraceEventType.Warning;
				default:
					return TraceEventType.Error;
			}
		}

		public static int ToSyncEventId(this RunStatus runStatus)
		{
			switch (runStatus)
			{
				case RunStatus.Success:
					return (int)EventId.CompletedRateSyncSuccess;
				case RunStatus.Warning:
					return (int)EventId.CompletedRateSyncWarning;
				default:
					return (int)EventId.CompletedRateSyncError;
			}
		}
	}
}
