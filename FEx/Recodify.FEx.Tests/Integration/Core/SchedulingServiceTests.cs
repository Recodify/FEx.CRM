using System;
using NUnit.Framework;
using Recodify.CRM.FEx.Core.Extensions;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Core.Scheduling;
using Recodify.CRM.FEx.Tests.Framework;

namespace Recodify.CRM.FEx.Tests.Integration.Core
{
	public class SchedulingServiceTests
	{
		[Test]
		public void CanTalkToSchedulingApi()
		{
			var service = new OrganisationServiceFactory().Create();
			var config = service.GetFExConfiguration(new Guid("dcdda8b0-a34b-e711-811a-e0071b65dea1"),
				ConfigAttribute.SchedulingAttributes);
			var debugLogger = new DebugLoggingService();
			
			var rateService = new SchedulingService(config, debugLogger, Guid.NewGuid());
			var result = rateService.GetNextRunDate("blah", 1);

			Assert.That(result, Is.Not.EqualTo(DateTime.MinValue));
		}
	}
}