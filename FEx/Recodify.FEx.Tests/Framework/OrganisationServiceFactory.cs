using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;

namespace Recodify.CRM.FEx.Tests.Framework
{
	public class OrganisationServiceFactory
	{
		public IOrganizationService Create()
		{
			var config = new TestDynamicsConfiguration();
			return new CrmServiceClient(config.Username, CrmServiceClient.MakeSecureString(config.Password),
				config.DynamicsRegion, config.OrganisationName,
				false, true, isOffice365: true);
		}
	}
}