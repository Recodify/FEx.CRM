using System;
using System.Activities;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Core.Repositories;

namespace Recodify.CRM.FEx.Core.Extensions
{
	public static class OrganisationServiceExtensions
	{
		public static string GetUniqueOrganisationName(this IOrganizationService organisationService)
		{
			var query = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
						  <entity name='organization'>    
							<order attribute='name' descending='false' />
						  </entity>
						</fetch>";
			var fetchService = new FetchService(organisationService);
			return fetchService.Fetch(query)?.Entities.FirstOrDefault()?.Attributes["name"] as string ?? string.Empty;
		}

		public static FExConfig GetFExConfiguration(this IOrganizationService organisationService, Guid configId,
			string[] attributes)
		{
			var configEntity = organisationService.Retrieve(
				ConfigAttribute.ConfigEntityName,
				configId,
				new ColumnSet(attributes));

			if (configEntity == null)
				throw new InvalidWorkflowException("Failed to retrieve FExConfig Entity.");

			return new FExConfig(configEntity);
		}
	}
}