using System;
using System.Diagnostics;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Core.Repositories;
using Recodify.CRM.FEx.Core.Scheduling;

namespace Recodify.CRM.FEx.Core.Jobs
{
	public class CalculateNextRunDateJob
	{
		private readonly IFExConfig config;
		private readonly int depth;
		private readonly Guid correlationId;
		private readonly DynamicsRepository repo;
		private readonly ILoggingService trace;

		public CalculateNextRunDateJob(DynamicsRepository repo, IFExConfig config, ILoggingService trace, int depth, Guid correlationId)
		{
			this.repo = repo;
			this.config = config;
			this.trace = trace;
			this.depth = depth;
			this.correlationId = correlationId;
		}

		public DateCalculationResult Execute()
		{
			trace.Trace(TraceEventType.Information, (int) EventId.StartingNextRunDateCalculation, "Starting Calculate Next Run Date");
			var schedulingService = new SchedulingService(config, trace, correlationId);
			var nextRunDate = schedulingService.GetNextRunDate(repo.GetUniqueName(), depth);

			repo.SaveNextRunDate(config, nextRunDate);

			trace.Trace(TraceEventType.Information, (int) EventId.CompletedNextRunDateCalculationSuccess,
				$"Successfully set the next run date to: {nextRunDate}");

			return new DateCalculationResult(nextRunDate);
		}
	}
}