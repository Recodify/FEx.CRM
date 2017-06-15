using System.Linq;
using Microsoft.Xrm.Sdk;

namespace Recodify.CRM.FEx.Core.Repositories
{
	public class DynamicsRepository
	{		
		private readonly FetchService fetchService;

		public DynamicsRepository(IOrganizationService organizationService)
		{			
			this.fetchService = new FetchService(organizationService);
		}

		public EntityCollection GetCurrencies()
		{
			var query = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
						  <entity name='transactioncurrency'>
							<attribute name='transactioncurrencyid' />
							<attribute name='currencyname' />
							<attribute name='isocurrencycode' />
							<attribute name='currencysymbol' />
							<attribute name='exchangerate' />
							<attribute name='currencyprecision' />
							<order attribute='currencyname' descending='false' />
						  </entity>
						</fetch>";

			return fetchService.Fetch(query);
		}

		public string GetUniqueName()
		{
			var query = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
						  <entity name='organization'>    
							<order attribute='name' descending='false' />
						  </entity>
						</fetch>";

			return fetchService.Fetch(query)?.Entities.FirstOrDefault()?.Attributes["name"] as string ?? string.Empty;
		}
	}
}
