using System;
using Quartz;
using Recodify.CRM.FEx.Exetensions;

namespace Recodify.CRM.FEx.Scheduling
{
	public class DateCalculator
	{
		private readonly DateTimeOffset? lastRunDate;
		private readonly DateTimeOffset currentDate;

		public DateCalculator(DateTimeOffset? lastRunDate, DateTimeOffset currentDate)
		{
			this.lastRunDate = lastRunDate;
			this.currentDate = currentDate;
		}

		public DateTimeOffset? Calculate(Frequency frequency, int day, decimal time)
		{
			var referenceTime = lastRunDate ?? currentDate;
			var hour = Math.Floor(time);
			var min = (time - hour) * 100;
			
			var expressionString = BuildExpressionString(frequency, day, min, hour);

			var expresion = new CronExpression(expressionString) {TimeZone = TimeZoneInfo.Utc};
			var nextDate = expresion.GetNextValidTimeAfter(referenceTime);
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
