using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Models.Dynamics;

namespace Recodify.CRM.FEx.Core.Repositories
{
	public class DynamicsRepository
	{
		private readonly IOrganizationService organisationService;
		private readonly ILoggingService trace;
		private readonly FetchService fetchService;

		public DynamicsRepository(IOrganizationService organisationService, ILoggingService trace)
		{
			this.organisationService = organisationService;
			this.trace = trace;
			this.fetchService = new FetchService(organisationService);
		}

		public virtual void SaveCurrencies(EntityCollection currencies)
		{
			foreach (var cur in currencies.Entities)
			{
				organisationService.Update(cur);				
			}			
		}

		public virtual void SaveNextRunDate(IFExConfig config, DateTime nextRunDate)
		{
			config.NextRunDate = nextRunDate;
			config.RemoveNonPersistableAttributes();
			organisationService.Update(config.Entity);
		}

		public EntityCollection GetCurrencies()
		{
			var query = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
						  <entity name='transactioncurrency'>
							<attribute name='transactioncurrencyid' />							
							<attribute name='isocurrencycode' />							
							<attribute name='exchangerate' />							
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

			var name = fetchService.Fetch(query)?.Entities.FirstOrDefault()?.Attributes["name"] as string ?? string.Empty;
			trace.Trace(TraceEventType.Information, (int)EventId.GettingUniqueOrganizationName, "Organisation unique name retreived as: " + name);
			return name;
		}
	}
}
