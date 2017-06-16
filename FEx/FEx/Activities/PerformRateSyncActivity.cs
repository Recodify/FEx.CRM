using System;
using System.Activities;
using Recodify.CRM.FEx.Core.Jobs;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Core.Repositories;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Extensions;
using Recodify.CRM.FEx.Core.Monitoring;

namespace Recodify.CRM.FEx.Dynamics.Activities
{
	public class PerformRateSyncActivity : FExCodeActivityBase
	{
		protected override void Execute(CodeActivityContext executionContext)
		{
			var tracingService = GetTraceService(executionContext);
			var trace = new LoggingService(tracingService);

			var workflowContext = GetWorkflowContext(executionContext, tracingService);
			var organisationService = GetOrganizationService(workflowContext.UserId, executionContext);			
			var config = organisationService.GetFExConfiguration(workflowContext.PrimaryEntityId, ConfigAttribute.RunAttributes);

			try
			{						
				var rateSyncJob = new RateSyncJob(new DynamicsRepository(organisationService), organisationService, config, trace);
				rateSyncJob.Execute();																
			}
			catch (Exception exp)
			{
				new JobCompleter(organisationService, config, trace).Complete(RunStatus.Error);
				tracingService.Trace(exp.ToString());
				throw;
			}
		}		
	}
}
