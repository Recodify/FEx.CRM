using System;
using NodaTime;
using Quartz;
using Recodify.CRM.FEx.Core.Extensions;
using Recodify.CRM.FEx.Core.Monitoring;

namespace Recodify.CRM.FEx.Core.Scheduling
{
	public class DateCalculator
	{ 
		private readonly DateTimeOffset currentDate;
		private readonly int depth;
		public const int BackOffInterval = 10;
		public const int MaxBackOffInterval = 65;
		public const int MaxDepth = 7;

		public DateCalculator(DateTimeOffset currentDate, int depth)
		{
			this.currentDate = currentDate;
			this.depth = depth;
		}

		public DateTimeOffset? Calculate(Frequency frequency, int day, decimal time, RunStatus lastRunStatus)
		{			
			if (lastRunStatus != RunStatus.Error)
			{
				var hour = Math.Floor(time);
				var min = (time - hour) * 100;
				var expressionString = BuildExpressionString(frequency, day, min, hour);

				var expresion = new CronExpression(expressionString) { TimeZone = TimeZoneInfo.Utc };
				var nextDate = expresion.GetNextValidTimeAfter(currentDate);
				return nextDate;
			}

			var backOffDateTime = new ZonedDateTime(
				new LocalDateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour, currentDate.Minute),
				DateTimeZone.Utc, Offset.FromHours(0));

			if (depth >= MaxDepth)
			{
				return new DateTimeOffset(backOffDateTime.PlusMinutes(MaxBackOffInterval).ToDateTimeUtc());
			}
			else
			{
				return new DateTimeOffset(backOffDateTime.PlusMinutes(depth * BackOffInterval).ToDateTimeUtc());
			}

			

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
