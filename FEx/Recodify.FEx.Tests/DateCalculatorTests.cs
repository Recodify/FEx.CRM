using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Recodify.CRM.FEx;
using Recodify.CRM.FEx.Core.Scheduling;

namespace Recodify.FEx.Tests
{
    public class DateCalculatorTests
    {
		[Test]
		public void WhenDaily_AndCurrentScheduledInLastHour_ReturnsTomorrowAtScheduledTime()
		{
			var currentDate = DateTime.UtcNow;
			var currentHour = currentDate.Hour;
			var currentMinute = currentDate.Minute;
			var scheduledTime = (decimal)(currentHour + (decimal)currentMinute / 100);
			var date = new DateCalculator(new DateTime(2017, 06, currentDate.Day, 22, currentDate.Hour, currentDate.Minute - 10, DateTimeKind.Utc))
				.Calculate(Frequency.Daily, 10, scheduledTime);
			Assert.That(date.Value.Month, Is.EqualTo(6));
			Assert.That(date.Value.Day, Is.EqualTo(currentDate.Day + 1));
			Assert.That(date.Value.Hour, Is.EqualTo(currentHour));
			Assert.That(date.Value.Minute, Is.EqualTo(currentMinute));
		}

		[Test]
		public void WhenDaily_AndCurrentTimeAfterScheduleTime_ReturnsTodayAtScheduledTime()
		{
			var date = new DateCalculator(new DateTime(2017, 06, 09, 22, 1, 1, DateTimeKind.Utc)).Calculate(Frequency.Daily, 10, 21.30M);
			Assert.That(date.Value.Month, Is.EqualTo(6));
			Assert.That(date.Value.Day, Is.EqualTo(10));
			Assert.That(date.Value.Hour, Is.EqualTo(21));
			Assert.That(date.Value.Minute, Is.EqualTo(30));
		}

		[Test]
		public void WhenDaily_AndCurrentTimeBeforeScheduleTime_ReturnsTodayAtScheduledTime()
		{
			var date = new DateCalculator(new DateTime(2017, 06, 09, 1, 1, 1, DateTimeKind.Utc)).Calculate(Frequency.Daily, 10, 21.30M);
			Assert.That(date.Value.Month, Is.EqualTo(6));
			Assert.That(date.Value.Day, Is.EqualTo(9));
			Assert.That(date.Value.Hour, Is.EqualTo(21));
			Assert.That(date.Value.Minute, Is.EqualTo(30));
		}

		[Test]
		public void WhenWeekly_AndDay10_ReturnsNextSunday()
		{
			var date = new DateCalculator(new DateTime(2017, 06, 09,1,1,1, DateTimeKind.Utc)).Calculate(Frequency.Weekly, 10, 21.30M);
			Assert.That(date.Value.Month, Is.EqualTo(6));
			Assert.That(date.Value.Day, Is.EqualTo(11));
			Assert.That(date.Value.Hour, Is.EqualTo(21));
			Assert.That(date.Value.Minute, Is.EqualTo(30));
		}

		[Test]
		public void WhenWeekly_AndDay1_ReturnsNextMonday()
		{
			var date = new DateCalculator(new DateTime(2017, 06, 09,1,1,1,DateTimeKind.Utc)).Calculate(Frequency.Weekly, 1, 21.30M);
			Assert.That(date.Value.Month, Is.EqualTo(6));
			Assert.That(date.Value.Day, Is.EqualTo(12));
			Assert.That(date.Value.Hour, Is.EqualTo(21));
			Assert.That(date.Value.Minute, Is.EqualTo(30));
		}

		[Test]
		public void WhenMontly_AndDay10_ButCurrentDay11_AndLastRunNull_ShouldReturnThisMonth_On10thDay()
		{
			var date = new DateCalculator(new DateTime(2017, 06, 11,1,1,1,DateTimeKind.Utc)).Calculate(Frequency.Monthly, 10, 21.30M);
			Assert.That(date.Value.Month, Is.EqualTo(7));
			Assert.That(date.Value.Day, Is.EqualTo(10));
			Assert.That(date.Value.Hour, Is.EqualTo(21));
			Assert.That(date.Value.Minute, Is.EqualTo(30));
		}

		[Test]
		public void WhenMontly_AndDay10_AndLastRunNull_ShouldReturnThisMonth_On10thDay()
		{
			var date = new DateCalculator(new DateTime(2017, 06, 09,1,1,1,DateTimeKind.Utc)).Calculate(Frequency.Monthly, 10, 21.30M);
			Assert.That(date.Value.Month, Is.EqualTo(6));
			Assert.That(date.Value.Day, Is.EqualTo(10));
			Assert.That(date.Value.Hour, Is.EqualTo(21));
			Assert.That(date.Value.Minute, Is.EqualTo(30));
		}

		[Test]
	    public void WhenMontly_AndDay10_AndLastRunNotNull_ShouldReturnNextMonth_On10thDay()
		{
			var date = new DateCalculator(new DateTime(2017, 06, 11, 1, 1, 1, DateTimeKind.Utc)).Calculate(Frequency.Monthly, 10, 21.30M);
			
			Assert.That(date.Value.Month, Is.EqualTo(7));
			Assert.That(date.Value.Day, Is.EqualTo(10));
			Assert.That(date.Value.Hour, Is.EqualTo(21));
			Assert.That(date.Value.Minute, Is.EqualTo(30));
		}

		[Test]
		public void WhenMontly_AndDay31_AndNextMonthHas30Days_ShouldReturn30thDay()
		{
			var date = new DateCalculator(new DateTime(2017, 05, 31, 22,1,1, DateTimeKind.Utc)).Calculate(Frequency.Monthly, 30, 21.30M);
			Assert.That(date.Value.Month, Is.EqualTo(6));
			Assert.That(date.Value.Day, Is.EqualTo(30));
			Assert.That(date.Value.Hour, Is.EqualTo(21));
			Assert.That(date.Value.Minute, Is.EqualTo(30));
		}
	}
}
