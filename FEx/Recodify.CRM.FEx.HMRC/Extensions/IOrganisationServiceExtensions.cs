using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Recodify.CRM.FEx.Core.Models.Dynamics;

namespace Recodify.CRM.FEx.Core.Extensions
{
	public static class OrganisationServiceExtensions
	{
		public static FExConfig GetFExConfiguration(this IOrganizationService organisationService, Guid configId, string[] attributes)
		{
			var configEntity = organisationService.Retrieve(
				ConfigAttribute.ConfigEntityName,
				configId,
				new ColumnSet(attributes));

			if (configEntity == null)
			{
				throw new InvalidWorkflowException("Failed to retrieve FExConfig Entity.");
			}

			return new FExConfig(configEntity);
		}
	}
}
