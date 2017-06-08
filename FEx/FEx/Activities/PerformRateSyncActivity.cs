using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recodify.CRM.FEx.Data;

namespace Recodify.CRM.FEx.Activities
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
				config.LastSyncDate = DateTime.UtcNow;
				tracingService.Trace("Set Last Sync Date to: " + config.LastSyncDate);
				config.RemoveAllUserTriggeringAttributes();
				organizationService.Update(config.Entity);
				tracingService.Trace("Synced Rates");
					
			}
			catch (Exception exp)
			{
				tracingService.Trace(exp.ToString());
				throw;
			}
		}
	}
}
