using System;
using System.Diagnostics;
using Microsoft.Xrm.Sdk;
using Recodify.CRM.FEx.Core.Extensions;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Core.Monitoring;

namespace Recodify.CRM.FEx.Core.Jobs
{
	public class JobCompleter
	{
		private readonly IFExConfig config;
		private readonly IOrganizationService organisationService;
		private readonly ILoggingService trace;

		public JobCompleter(IOrganizationService organisationService, IFExConfig config, ILoggingService trace)
		{
			this.organisationService = organisationService;
			this.config = config;
			this.trace = trace;
		}

		public void Complete(RunStatus runStatus)
		{
			config.LastRunStatus = runStatus;
			trace.Trace(TraceEventType.Information, (int) EventId.SavingLastSyncDate,
				"Set Last Sync Date to: " + config.LastSyncDate);

			var completeDate = DateTime.UtcNow;
			config.LastSyncDate = completeDate;
			trace.Trace(TraceEventType.Information, (int) EventId.SavingLastRunStatus, "Set Last Run Status to: " + runStatus);

			config.RemoveNonPersistableAttributes();
			organisationService.Update(config.Entity);

			trace.Trace(runStatus.ToTraceEventType(), runStatus.ToSyncEventId(),
				$"Completed Syncing Rate with Status: {runStatus} @ {completeDate}");
		}
	}
}