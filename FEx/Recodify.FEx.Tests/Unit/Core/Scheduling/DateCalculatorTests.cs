using System;
using Moq;
using NodaTime;
using NUnit.Framework;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Monitoring;
using Recodify.CRM.FEx.Core.Scheduling;

namespace Recodify.CRM.FEx.Tests.Unit
{
	public class DateCalculatorTests
	{
		private ILoggingService trace;

		[SetUp]
		public void SetUp()
		{
			trace = new Mock<ILoggingService>().Object;
		}

		/*
			The smallest interval should 61 minuets but using 65 to be safe.
			On CRM online when the depth reaches 16, the Infinite loop message is issued. On-premise the max depth is 8.
		*/

		[TestCase(7)]
		[TestCase(10)]
		[TestCase(16)]
		public void WhenLastRunStatusError_AndDepthGreaterThanMaxDepth_7_Returns65(int depth)
		{
			var currentDate = new DateTime(2017, 06, 1, 22, 1, 0, DateTimeKind.Utc);
			var date = new DateCalculator(currentDate, trace)
				.Calculate(Frequency.Daily, 10, 1.10M, RunStatus.Error, depth);

			var expectedDateTime = new ZonedDateTime(new LocalDateTime(2017, 06, 1, 22, 1, 0), DateTimeZone.Utc,
				Offset.FromHours(0));
			expectedDateTime = expectedDateTime.PlusMinutes(DateCalculator.MaxBackOffInterval);

			Assert.That(date.Value.UtcDateTime, Is.EqualTo(expectedDateTime.ToDateTimeUtc()));
		}

		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(6)]
		public void WhenLastRunStatusError_ReturnsDepthTimes10(int depth)
		{
			var currentDate = new DateTime(2017, 06, 1, 22, 1, 0, DateTimeKind.Utc);
			var date = new DateCalculator(currentDate, trace)
				.Calculate(Frequency.Daily, 10, 1.10M, RunStatus.Error, depth);

			var expectedDateTime = new ZonedDateTime(new LocalDateTime(2017, 06, 1, 22, 1, 0), DateTimeZone.Utc,
				Offset.FromHours(0));
			expectedDateTime = expectedDateTime.PlusMinutes(depth * DateCalculator.BackOffInterval);

			Assert.That(date.Value.UtcDateTime, Is.EqualTo(expectedDateTime.ToDateTimeUtc()));
		}

		[TestCase(RunStatus.Success)]
		[TestCase(RunStatus.Warning)]
		public void WhenDaily_AndCurrentScheduledInLastHour_ReturnsTomorrowAtScheduledTime(RunStatus lastRunStatus)
		{
			var currentDate = DateTime.UtcNow;
			var currentHour = currentDate.Hour;
			var currentMinute = 50;
			var scheduledTime = currentHour + (decimal) currentMinute / 100;
			var date = new DateCalculator(
					new DateTime(2017, 06, currentDate.Day, 22, currentDate.Hour, currentMinute - 10, DateTimeKind.Utc), trace)
				.Calculate(Frequency.Daily, 10, scheduledTime, RunStatus.Success, 1);
			Assert.That(date.Value.Month, Is.EqualTo(6));
			Assert.That(date.Value.Day, Is.EqualTo(currentDate.Day + 1));
			Assert.That(date.Value.Hour, Is.EqualTo(currentHour));
			Assert.That(date.Value.Minute, Is.EqualTo(currentMinute));
		}

		[TestCase(RunStatus.Success)]
		[TestCase(RunStatus.Warning)]
		public void WhenDaily_AndCurrentTimeAfterScheduleTime_ReturnsTodayAtScheduledTime(RunStatus lastRunStatus)
		{
			var date =
				new DateCalculator(new DateTime(2017, 06, 09, 22, 1, 1, DateTimeKind.Utc), trace).Calculate(Frequency.Daily, 10,
					21.30M, lastRunStatus, 1);
			Assert.That(date.Value.Month, Is.EqualTo(6));
			Assert.That(date.Value.Day, Is.EqualTo(10));
			Assert.That(date.Value.Hour, Is.EqualTo(21));
			Assert.That(date.Value.Minute, Is.EqualTo(30));
		}

		[TestCase(RunStatus.Success)]
		[TestCase(RunStatus.Warning)]
		public void WhenDaily_AndCurrentTimeBeforeScheduleTime_ReturnsTodayAtScheduledTime(RunStatus lastRunStatus)
		{
			var date = new DateCalculator(new DateTime(2017, 06, 09, 1, 1, 1, DateTimeKind.Utc), trace).Calculate(
				Frequency.Daily, 10, 21.30M, lastRunStatus, 1);
			Assert.That(date.Value.Month, Is.EqualTo(6));
			Assert.That(date.Value.Day, Is.EqualTo(9));
			Assert.That(date.Value.Hour, Is.EqualTo(21));
			Assert.That(date.Value.Minute, Is.EqualTo(30));
		}

		[TestCase(RunStatus.Success)]
		[TestCase(RunStatus.Warning)]
		public void WhenWeekly_AndDay10_ReturnsNextSunday(RunStatus lastRunStatus)
		{
			var date =
				new DateCalculator(new DateTime(2017, 06, 09, 1, 1, 1, DateTimeKind.Utc), trace).Calculate(Frequency.Weekly, 10,
					21.30M, lastRunStatus, 1);
			Assert.That(date.Value.Month, Is.EqualTo(6));
			Assert.That(date.Value.Day, Is.EqualTo(11));
			Assert.That(date.Value.Hour, Is.EqualTo(21));
			Assert.That(date.Value.Minute, Is.EqualTo(30));
		}

		[TestCase(RunStatus.Success)]
		[TestCase(RunStatus.Warning)]
		public void WhenWeekly_AndDay1_ReturnsNextMonday(RunStatus lastRunStatus)
		{
			var date =
				new DateCalculator(new DateTime(2017, 06, 09, 1, 1, 1, DateTimeKind.Utc), trace).Calculate(Frequency.Weekly, 1,
					21.30M, lastRunStatus, 1);
			Assert.That(date.Value.Month, Is.EqualTo(6));
			Assert.That(date.Value.Day, Is.EqualTo(12));
			Assert.That(date.Value.Hour, Is.EqualTo(21));
			Assert.That(date.Value.Minute, Is.EqualTo(30));
		}

		[TestCase(RunStatus.Success)]
		[TestCase(RunStatus.Warning)]
		public void WhenMontly_AndDay10_ButCurrentDay11_AndLastRunNull_ShouldReturnThisMonth_On10thDay(RunStatus lastRunStatus)
		{
			var date =
				new DateCalculator(new DateTime(2017, 06, 11, 1, 1, 1, DateTimeKind.Utc), trace).Calculate(Frequency.Monthly, 10,
					21.30M, lastRunStatus, 1);
			Assert.That(date.Value.Month, Is.EqualTo(7));
			Assert.That(date.Value.Day, Is.EqualTo(10));
			Assert.That(date.Value.Hour, Is.EqualTo(21));
			Assert.That(date.Value.Minute, Is.EqualTo(30));
		}

		[TestCase(RunStatus.Success)]
		[TestCase(RunStatus.Warning)]
		public void WhenMontly_AndDay10_AndLastRunNull_ShouldReturnThisMonth_On10thDay(RunStatus lastRunStatus)
		{
			var date =
				new DateCalculator(new DateTime(2017, 06, 09, 1, 1, 1, DateTimeKind.Utc), trace).Calculate(Frequency.Monthly, 10,
					21.30M, lastRunStatus, 1);
			Assert.That(date.Value.Month, Is.EqualTo(6));
			Assert.That(date.Value.Day, Is.EqualTo(10));
			Assert.That(date.Value.Hour, Is.EqualTo(21));
			Assert.That(date.Value.Minute, Is.EqualTo(30));
		}

		[TestCase(RunStatus.Success)]
		[TestCase(RunStatus.Warning)]
		public void WhenMontly_AndDay10_AndLastRunNotNull_ShouldReturnNextMonth_On10thDay(RunStatus lastRunStatus)
		{
			var date =
				new DateCalculator(new DateTime(2017, 06, 11, 1, 1, 1, DateTimeKind.Utc), trace).Calculate(Frequency.Monthly, 10,
					21.30M, lastRunStatus, 1);

			Assert.That(date.Value.Month, Is.EqualTo(7));
			Assert.That(date.Value.Day, Is.EqualTo(10));
			Assert.That(date.Value.Hour, Is.EqualTo(21));
			Assert.That(date.Value.Minute, Is.EqualTo(30));
		}

		[TestCase(RunStatus.Success)]
		[TestCase(RunStatus.Warning)]
		public void WhenMontly_AndDay31_AndNextMonthHas30Days_ShouldReturn30thDay(RunStatus lastRunStatus)
		{
			var date =
				new DateCalculator(new DateTime(2017, 05, 31, 22, 1, 1, DateTimeKind.Utc), trace).Calculate(Frequency.Monthly, 30,
					21.30M, lastRunStatus, 1);
			Assert.That(date.Value.Month, Is.EqualTo(6));
			Assert.That(date.Value.Day, Is.EqualTo(30));
			Assert.That(date.Value.Hour, Is.EqualTo(21));
			Assert.That(date.Value.Minute, Is.EqualTo(30));
		}
	}
}