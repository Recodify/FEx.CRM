using NUnit.Framework;
using Recodify.CRM.FEx.Core.Exceptions;
using Recodify.CRM.FEx.Core.Exchange;
using Recodify.CRM.FEx.Tests.Framework;

namespace Recodify.CRM.FEx.Tests.Integration.Core
{
	public class RateServiceTests
	{
		[Test]
		public void WhenDataSourceHMRC_ReturnsRates()
		{
			var rateService = new RateService(new MockFExConfig {DataSource = RateDataSource.Hmrc});
			var result = rateService.GetRates("blah");
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Items.Count, Is.GreaterThan(1));
			Assert.That(result.Period, Is.Not.Null);
		}

		[Test]
		public void WhenDataSourceInvalid_ThrowsRatesSyncException()
		{
			var rateService = new RateService(new MockFExConfig {DataSource = RateDataSource.Unset});
			Assert.Throws<RateSyncException>(() => rateService.GetRates("blah"));
		}
	}
}