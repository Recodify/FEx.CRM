using System;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Recodify.CRM.FEx.Core.Jobs;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Core.Repositories;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Extensions;

namespace Recodify.CRM.FEx.Dynamics.Activities
{
	public class PerformRateSyncActivity : FExCodeActivityBase
	{
		protected override void Execute(CodeActivityContext executionContext)
		{
			var tracingService = GetTraceService(executionContext);
			try
			{
				var workflowContext = GetWorkflowContext(executionContext, tracingService);
				var organizationService = GetOrganizationService(workflowContext.UserId, executionContext);
				var config = organizationService.GetFExConfiguration(workflowContext.PrimaryEntityId, ConfigAttribute.RunAttributes);
				tracingService.Trace("Syncing Rates");

				var rateSyncJob = new RateSyncJob(new DynamicsRepository(organizationService), organizationService, config, new LoggingService(tracingService));
				rateSyncJob.Execute();
								
				tracingService.Trace("Synced Rates");
					
			}
			catch (Exception exp)
			{
				//TODO: Set last status to error.
				tracingService.Trace(exp.ToString());
				throw;
			}
		}		
	}
}
