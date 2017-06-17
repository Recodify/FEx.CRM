using System;
using System.Activities;
using Recodify.CRM.FEx.Core.Extensions;
using Recodify.CRM.FEx.Core.Jobs;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Core.Monitoring;
using Recodify.CRM.FEx.Core.Repositories;

namespace Recodify.CRM.FEx.Dynamics.Activities
{
	public class PerformRateSyncActivity : FExCodeActivityBase
	{
		protected override void Execute(CodeActivityContext executionContext)
		{
			var tracingService = GetTraceService(executionContext);

			var workflowContext = GetWorkflowContext(executionContext);
			var organisationService = GetOrganizationService(workflowContext.UserId, executionContext);
			var trace = new DynamicsLoggingService(tracingService, organisationService.GetUniqueOrganisationName(),
				workflowContext.CorrelationId);
			var config = organisationService.GetFExConfiguration(workflowContext.PrimaryEntityId, ConfigAttribute.RunAttributes);

			try
			{
				var rateSyncJob = new RateSyncJob(new DynamicsRepository(organisationService, trace), organisationService, config,
					trace);
				rateSyncJob.Execute();
			}
			catch (Exception exp)
			{
				var msg = "Error has Occured Running PerformRateSync Activity. Seting Last Run Status to Error and logging.";
				HandleException(exp, msg, config, organisationService, trace);
			}
		}
	}
}