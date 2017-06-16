using System;
using System.Activities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Core.Repositories;
using Recodify.CRM.FEx.Core.Scheduling;

namespace Recodify.CRM.FEx.Core.Jobs
{
	public class CalculateNextRunDateJob
	{
		private readonly DynamicsRepository repo;
		private readonly IFExConfig config;
		private readonly ILoggingService trace;
		private readonly int depth;

		public CalculateNextRunDateJob(DynamicsRepository repo, IFExConfig config, ILoggingService trace, int depth)
		{
			this.repo = repo;
			this.config = config;
			this.trace = trace;
			this.depth = depth;
		}

		public DateCalculationResult Execute()
		{
			trace.Trace(TraceEventType.Information, (int)EventId.StartingNextRunDateCalculation, $"Starting Calculate Next Run Date");

			var calculator = new DateCalculator(new DateTimeOffset(DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)), trace);			
			var nextRunDate = calculator.Calculate(config.Frequency, config.Day, config.Time, config.LastRunStatus, depth);
			if (!nextRunDate.HasValue)
			{
				throw new InvalidWorkflowException(
					"Null valued NextRunDate calculated, there is probably a problem with the derived CRON expression. Check your scheduling data for valid values.");
			}

			repo.SaveNextRunDate(config, nextRunDate.Value.UtcDateTime);
			
			trace.Trace(TraceEventType.Information, (int)EventId.CompletedNextRunDateCalculationSuccess, $"Successfully set the next run date to: {nextRunDate}");

			return new DateCalculationResult(nextRunDate.Value);
		}
	}
}
