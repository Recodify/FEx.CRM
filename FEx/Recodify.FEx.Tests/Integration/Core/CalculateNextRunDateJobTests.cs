using System;
using Moq;
using NUnit.Framework;
using Recodify.CRM.FEx.Core.Extensions;
using Recodify.CRM.FEx.Core.Jobs;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Tests.Framework;

namespace Recodify.CRM.FEx.Tests.Integration.Core
{
	public class CalculateNextRunDateJobTests
	{
		[Test]
		public void CanCalculateNextRunDate()
		{
			var trace = new Mock<ILoggingService>().Object;
			var service = new OrganisationServiceFactory().Create();
			var config = service.GetFExConfiguration(new Guid("dcdda8b0-a34b-e711-811a-e0071b65dea1"),
				ConfigAttribute.SchedulingAttributes, trace);
			var repo = new AssertableDynamicsRepo(service);

			var job = new CalculateNextRunDateJob(repo, config, trace, 1, Guid.NewGuid());
			job.Execute();

			Assert.AreEqual(repo.SaveNextRunDateCallCount, 1);
		}
	}
}