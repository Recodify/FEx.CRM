using System;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Recodify.CRM.FEx.Core.Extensions;
using Recodify.CRM.FEx.Core.Jobs;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Core.Repositories;
using Recodify.CRM.FEx.Core.Scheduling;

namespace Recodify.CRM.FEx.Dynamics.Activities
{
	public class CalculateNextRunDateActivity : FExCodeActivityBase
    {		
		[Output("Next Run Date")]		
		[AttributeTarget(ConfigAttribute.ConfigEntityName, ConfigAttribute.NextRun)]	
		public OutArgument<DateTime> NextRunDate { get; set; }

		[Output("Current Revision")]		
		public OutArgument<int> CurrentRevision { get; set; }

		protected override void Execute(CodeActivityContext executionContext)
		{
			var tracingService = GetTraceService(executionContext);
			var trace = new LoggingService(tracingService);
			try
			{						
				var workflowContext = GetWorkflowContext(executionContext, tracingService);				
				var organizationService = GetOrganizationService(workflowContext.UserId, executionContext);
				var config = organizationService.GetFExConfiguration(workflowContext.PrimaryEntityId, ConfigAttribute.SchedulingAttributes);
				var repo = new DynamicsRepository(organizationService);

				var nextRunDate = new CalculateNextRunDateJob(repo, config, trace, workflowContext.Depth).Execute();
				CurrentRevision.Set(executionContext, config.Revision);				
				NextRunDate.Set(executionContext, nextRunDate.NextDate.UtcDateTime);				
			}
			catch (Exception exp)
			{
				tracingService.Trace(exp.ToString());
				throw;
			}
		}
    }
}
