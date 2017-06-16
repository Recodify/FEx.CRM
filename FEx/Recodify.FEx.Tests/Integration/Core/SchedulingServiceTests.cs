using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Recodify.CRM.FEx.Core.Exchange;
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
			var config = service.GetFExConfiguration(new Guid("dcdda8b0-a34b-e711-811a-e0071b65dea1"), ConfigAttribute.SchedulingAttributes);
			var debugLogger = new DebugLoggingService();

			var result = DateTime.MinValue;
			try
			{
				var rateService = new SchedulingService(config, debugLogger);
				result = rateService.GetNextRunDate("blah", 1);

			}
			catch (Exception exp)
			{
				
			}

			Assert.That(result, Is.Not.EqualTo(DateTime.MinValue));

		}
	}
}
