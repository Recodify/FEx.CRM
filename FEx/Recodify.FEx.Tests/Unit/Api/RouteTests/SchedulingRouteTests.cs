using System.Net.Http;
using System.Web.Http;
using MvcRouteTester;
using NUnit.Framework;
using Recodify.CRM.FEx.api;
using Recodify.CRM.FEx.api.Controllers;

namespace Recodify.CRM.FEx.Tests.Unit.Api.RouteTests
{
	public class SchedulingRouteTests
	{
		public HttpConfiguration config;

		[SetUp]
		public void SetUp()
		{
			config = new HttpConfiguration();
			WebApiConfig.Register(config);
			config.EnsureInitialized();
		}

		[Test]
		public void GetNextRunDate_WithBaseCurrencyCode()
		{
			config.ShouldMap("/api/schedule?id=blah&frequency=Daily&day=1&time=10.2&lastRunStatus=Success&depth=1")
				.To<ScheduleController>(HttpMethod.Get,
					x => x.Get("blah", FEx.Core.Scheduling.Frequency.Daily, 1, 10.2M, FEx.Core.Monitoring.RunStatus.Success, 1));
		}
	}
}
