using System;
using Moq;
using NUnit.Framework;
using Recodify.CRM.FEx.Core.Exceptions;
using Recodify.CRM.FEx.Core.Exchange;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Tests.Framework;

namespace Recodify.CRM.FEx.Tests.Integration.Core
{
	public class RateServiceTests
	{
		[Test]
		public void WhenDataSourceFixer_ReturnsRates()
		{
			var rateService = new RateService(new MockFExConfig { DataSource = RateDataSource.Fixer, BaseCurrencyCode = CurrencyCode.GBP }, new Mock<ILoggingService>().Object, Guid.NewGuid());
			var result = rateService.GetRates("blah");
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Items.Count, Is.GreaterThan(1));
			Assert.That(result.Period, Is.Not.Null);
		}

		[Test]
		public void WhenDataSourceHMRC_ReturnsRates()
		{
			var rateService = new RateService(new MockFExConfig {DataSource = RateDataSource.Hmrc}, new Mock<ILoggingService>().Object, Guid.NewGuid());
			var result = rateService.GetRates("blah");
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Items.Count, Is.GreaterThan(1));
			Assert.That(result.Period, Is.Not.Null);
		}

		[Test]
		public void WhenDataSourceInvalid_ThrowsRatesSyncException()
		{
			var rateService = new RateService(new MockFExConfig {DataSource = RateDataSource.Unset}, new Mock<ILoggingService>().Object, Guid.NewGuid());
			Assert.Throws<RateSyncException>(() => rateService.GetRates("blah"));
		}
	}
}