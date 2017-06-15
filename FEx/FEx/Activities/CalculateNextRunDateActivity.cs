using System;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Recodify.CRM.FEx.Core.Extensions;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Core.Scheduling;

namespace Recodify.CRM.FEx.Dynamics.Activities
{
	public class CalculateNextRunDateActivity : FExCodeActivityBase
    {		
		[Output("Next Run Date")]		
		[AttributeTarget(ConfigAttribute.ConfigEntityName, ConfigAttribute.NextRun)]	
		public OutArgument<DateTime> NextRunDate { get; set; }

		protected override void Execute(CodeActivityContext executionContext)
		{
			var tracingService = GetTraceService(executionContext);
			try
			{				
				var workflowContext = GetWorkflowContext(executionContext, tracingService);
				var organizationService = GetOrganizationService(workflowContext.UserId, executionContext);
				var config = organizationService.GetFExConfiguration(workflowContext.PrimaryEntityId, ConfigAttribute.SchedulingAttributes);
				SetNextRunDate(config, executionContext, organizationService, tracingService);				
			}
			catch (Exception exp)
			{
				tracingService.Trace(exp.ToString());
				throw;
			}
		}

	    private void SetNextRunDate(FExConfig config, CodeActivityContext context, IOrganizationService organizationService, ITracingService tracingService)
	    {
		    var calculator = new DateCalculator(new DateTimeOffset(DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)));
			// TODO: If last status is error, back off and retry, else calculate from schedule
			var nextRunDate = calculator.Calculate(config.Frequency, config.Day, config.Time);
		    if (!nextRunDate.HasValue)
		    {
			    throw new InvalidWorkflowException(
				    "Null valued NextRunDate calculated, there is probably a problem with the derived CRON expression. Check your scheduling data for valid values.");
		    }

		    config.NextRunDate = nextRunDate.Value;
		    this.NextRunDate.Set(context, nextRunDate.Value.UtcDateTime);
			config.RemoveNonPersistableAttributes();
			organizationService.Update(config.Entity);
			tracingService.Trace($"Successfully set the next run date to {nextRunDate}");
		}  
    }
}
