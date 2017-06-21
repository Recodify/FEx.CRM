using System.Net.Http;
using System.Web.Http;
using MvcRouteTester;
using NUnit.Framework;
using Recodify.CRM.FEx.api;
using Recodify.CRM.FEx.api.Controllers;

namespace Recodify.CRM.FEx.Tests.Unit.Api.RouteTests
{
	public class RateRouteTests
	{
		private HttpConfiguration config;

		[SetUp]
		public void SetUp()
		{
			config = new HttpConfiguration();
			WebApiConfig.Register(config);
			config.EnsureInitialized();
		}

		[Test]
		public void GetRates()
		{
			config.ShouldMap("/api/rates?rateSource=Fixer&id=blah&baseCurrencyCode=GBP")
				.To<RatesController>(HttpMethod.Get, x => x.Get("blah", "Fixer", "GBP"));
		}
	}
}
