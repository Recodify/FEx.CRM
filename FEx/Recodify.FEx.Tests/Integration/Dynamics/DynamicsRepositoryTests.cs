using System;
using Moq;
using NUnit.Framework;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Repositories;
using Recodify.CRM.FEx.Tests.Framework;

namespace Recodify.CRM.FEx.Tests.Integration.Dynamics
{
	public class DynamicsRepositoryTests
	{
		private DynamicsRepository repo;

		[SetUp]
		public void SetUp()
		{
			var service = new OrganisationServiceFactory().Create();
			repo = new DynamicsRepository(service, new Mock<ILoggingService>().Object);
		}

		[Test]
		public void CanGetBaseCurrencyCode()
		{
			var result = repo.GetBaseCurrencyCode(new Guid("33b43081-458a-e611-80ec-c4346bac0ff4"));
			Assert.That(result, Is.EqualTo(CurrencyCode.GBP));
		}

		[Test]
		public void CanGetCurrencies()
		{
			var result = repo.GetCurrencies();
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Entities.Count, Is.GreaterThan(1));
		}

		[Test]
		public void CanOrganizationUniqueName()
		{
			var result = repo.GetUniqueName();
			Assert.That(result, Is.EqualTo("ingtysandbox"));
		}
	}
}