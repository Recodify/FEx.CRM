using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Recodify.CRM.FEx.Core.Exchange;
using Recodify.CRM.FEx.Core.Extensions;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Core.Monitoring;
using Recodify.CRM.FEx.Core.Repositories;

namespace Recodify.CRM.FEx.Core.Jobs
{
	public class RateSyncJob
	{
		private readonly DynamicsRepository repo;
		private readonly IOrganizationService organisationService;
		private readonly IFExConfig config;
		private readonly ILoggingService trace;
		private readonly RateService rateService;
		private readonly RateSyncer rateSyncer;

		public RateSyncJob(DynamicsRepository repository, IOrganizationService organisationService, IFExConfig config, ILoggingService trace)
		{
			this.organisationService = organisationService;
			this.config = config;
			this.trace = trace;
			repo = repository;
			rateService = new RateService(config);
			rateSyncer = new RateSyncer(config, trace);
		}

		public void Execute()
		{
			trace.Trace(TraceEventType.Information, (int)EventId.StartingRateSync, "Syncing Rates");

			trace.Trace(TraceEventType.Information, (int)EventId.GettingRatesFromApi, "Getting Rates from API");
			var rates = rateService.GetRates(GetOrganisationUniqueName());

			trace.Trace(TraceEventType.Information, (int)EventId.GettingCurrenciesFromCrm, "Getting Currencies from CRM");
			var currencies = repo.GetCurrencies();

			trace.Trace(TraceEventType.Information, (int)EventId.SyncingCurrencies, "Sync Currencies with Latest Rate Date");
			currencies = rateSyncer.Sync(currencies, rates);

			trace.Trace(TraceEventType.Information, (int)EventId.SavingCurrencies, "Saving updated currencies to CRM");
			repo.SaveCurrencies(currencies);

			new JobCompleter(organisationService, config, trace).Complete(trace.HasWarnings ? RunStatus.Warning : RunStatus.Success);
		}

		private string GetOrganisationUniqueName()
		{
			var name = new DynamicsRepository(organisationService).GetUniqueName();
			trace.Trace(TraceEventType.Information, (int)EventId.GettingUniqueOrganizationName, "Organisation unique name retreived as: " + name);
			return name;
		}		

		
	}

}
