using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

namespace Recodify.CRM.FEx.Dynamics.Repositories
{
	public class OrganizationRepository
	{		
		private readonly FetchService fetchService;

		public OrganizationRepository(IOrganizationService organizationService)
		{			
			this.fetchService = new FetchService(organizationService);
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
