using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using NUnit.Framework;
using Recodify.CRM.FEx.Core.Repositories;
using Recodify.CRM.FEx.Tests.Framework;

namespace Recodify.CRM.FEx.Tests.Integration.Dynamics
{
	public class DynamicsRepositoryTests
	{
		private DynamicsRepository repo;

		[SetUp]
		private void SetUp()
		{
			var service = CreateOrganisationService();
			repo = new DynamicsRepository(service);
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

		private IOrganizationService CreateOrganisationService()
		{
			var config = new TestDynamicsConfiguration();
			return new CrmServiceClient(config.Username, CrmServiceClient.MakeSecureString(config.Password),
				config.DynamicsRegion, config.OrganisationName,
				false, true, isOffice365: true);
		}
	}
}
