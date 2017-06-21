using System;
using System.Diagnostics;
using NodaTime;
using Quartz;
using Recodify.CRM.FEx.Core.Extensions;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Monitoring;

namespace Recodify.CRM.FEx.Core.Scheduling
{
	public class DateCalculator
	{
		public const int BackOffInterval = 10;
		public const int MaxBackOffInterval = 65;
		public const int MaxDepth = 7;
		private readonly DateTimeOffset currentDate;
		private readonly ILoggingService trace;

		public DateCalculator(DateTimeOffset currentDate, ILoggingService trace)
		{
			this.currentDate = currentDate;
			this.trace = trace;
		}

		public DateTimeOffset? Calculate(Frequency frequency, int day, decimal time, RunStatus lastRunStatus, int depth)
		{
			if (lastRunStatus != RunStatus.Error)
				return CalculateFromSchedule(frequency, day, time, lastRunStatus);

			return BackoffAndRetry(lastRunStatus, depth);
		}

		private DateTimeOffset? BackoffAndRetry(RunStatus lastRunStatus, int depth)
		{
			trace.Trace(TraceEventType.Verbose, (int) EventId.BackOffAndRetry,
				$"Last Run Status was: {lastRunStatus}. Backing off and retrying.");

			var backOffDateTime = new ZonedDateTime(
				new LocalDateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, currentDate.Minute),
				DateTimeZone.Utc, Offset.FromHours(0));

			if (depth >= MaxDepth)
			{
				trace.Trace(TraceEventType.Verbose, (int) EventId.BackOffAndRetry,
					$"Current depth {depth} is equal to greater than Max depth {MaxDepth}. Using MaxBackOffInterval of {MaxBackOffInterval}");
				return new DateTimeOffset(backOffDateTime.PlusMinutes(MaxBackOffInterval).ToDateTimeUtc());
			}

			trace.Trace(TraceEventType.Verbose, (int) EventId.BackOffAndRetry,
				$"Backing off for {depth * BackOffInterval}");

			return new DateTimeOffset(backOffDateTime.PlusMinutes(depth * BackOffInterval).ToDateTimeUtc());
		}

		private DateTimeOffset? CalculateFromSchedule(Frequency frequency, int day, decimal time, RunStatus lastRunStatus)
		{
			trace.Trace(TraceEventType.Information, (int)EventId.CalculatingFromSchedule,
				$"Last Run Status was: {lastRunStatus}. Calcultaing from schedule.");
			var hour = Math.Floor(time);
			var min = (time - hour) * 100;
			var expressionString = BuildExpressionString(frequency, day, min, hour);

			var expresion = new CronExpression(expressionString) {TimeZone = TimeZoneInfo.Utc};
			var nextDate = expresion.GetNextValidTimeAfter(currentDate);
			return nextDate;
		}

		private string BuildExpressionString(Frequency frequency, int day, decimal min, decimal hour)
		{
			var expressionString = "";
			switch (frequency)
			{
				case Frequency.Weekly:
					expressionString = $"0 {min} {hour} ? * {day.ToDayOfWeekString()} *";
					break;
				case Frequency.Daily:
					expressionString = $"0 {min} {hour} 1/1 * ? *";
					break;
				default:
					expressionString = $"0 {min} {hour} {day} 1/1 ? *";
					break;
			}
			return expressionString;
		}
	}
}