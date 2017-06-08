using System;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Recodify.CRM.FEx.Core.Models.Dynamics;

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
				var config = GetFExConfiguration(workflowContext, organizationService, ConfigAttribute.RunAttributes);
				tracingService.Trace("Syncing Rates");
				
				SetLastSyncDate(tracingService, config, organizationService);
				tracingService.Trace("Synced Rates");
					
			}
			catch (Exception exp)
			{
				//TODO: Set last status to error.
				tracingService.Trace(exp.ToString());
				throw;
			}
		}

		private static void SetLastSyncDate(ITracingService tracingService, FExConfig config,
			IOrganizationService organizationService)
		{
			tracingService.Trace("Set Last Sync Date to: " + config.LastSyncDate);
			config.LastSyncDate = DateTime.UtcNow;
			config.RemoveNonPersistableAttributes();
			organizationService.Update(config.Entity);
		}
	}
}
