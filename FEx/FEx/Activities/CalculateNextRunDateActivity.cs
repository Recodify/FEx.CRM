using System;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using Recodify.CRM.FEx.Data;
using Recodify.CRM.FEx.Scheduling;

namespace Recodify.CRM.FEx.Activities
{
    public class CalculateNextRunDateActivity : CodeActivity
    {
		protected override void Execute(CodeActivityContext executionContext)
		{
			var tracingService = GetTraceService(executionContext);
			try
			{
				
				var workflowContext = GetWorkflowContext(executionContext, tracingService);
				var organizationService = GetOrganizationService(workflowContext.UserId, executionContext);
				var config = GetFExConfiguration(workflowContext, organizationService);

				SetNextRunDate(config);
				Persist(organizationService, config, tracingService);
			}
			catch (Exception exp)
			{
				tracingService.Trace(exp.ToString());
				throw exp;
			}
		}

	    private static void Persist(IOrganizationService organizationService, FExConfig config, ITracingService tracingService)
	    {
		    organizationService.Update(config.Entity);
		    tracingService.Trace($"Successfully set the next run date to {config.NextRunDate}");
	    }

	    private static void SetNextRunDate(FExConfig config)
	    {
		    var calculator = new DateCalculator(config.NextRunDate,
			    new DateTimeOffset(DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc)));
		    var nextRunDate = calculator.Calculate(config.Frequency, config.Day, config.Time);
		    if (!nextRunDate.HasValue)
		    {
			    throw new InvalidWorkflowException(
				    "Null valued NextRunDate calculated, there is probably a problem with the derived CRON expression. Check your scheduling data for valid values.");
		    }

		    config.NextRunDate = nextRunDate.Value;
	    }

	    private FExConfig GetFExConfiguration(
			IWorkflowContext workflowContext,
			IOrganizationService organizationService)
	    {
			
		    var configEntity = organizationService.Retrieve(
				ConfigAttribute.ConfigEntityName, 
				workflowContext.PrimaryEntityId, 
				new ColumnSet(ConfigAttribute.AllCustomAttriutes));

			if (configEntity == null)
		    {
				throw new InvalidWorkflowException("Failed to retrieve FExConfig Entity.");
			}

		    return new FExConfig(configEntity);
	    }

	    private IOrganizationService GetOrganizationService(Guid userId, CodeActivityContext executionContext)
	    {
			IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
			var service = serviceFactory.CreateOrganizationService(userId);
		    if (service == null)
		    {
				throw new InvalidWorkflowException("Failed to create OrganizationService.");
			}

			return service;
		}

	    private IWorkflowContext GetWorkflowContext(CodeActivityContext executionContext, ITracingService tracingService)
	    {
			var context = executionContext.GetExtension<IWorkflowContext>();

			if (context == null)
			{
				throw new InvalidWorkflowException("Failed to retrieve workflow context.");
			}

			tracingService.Trace("CalculateNextRunDate.Execute(), Correlation Id: {0}, Initiating User: {1}",
				context.CorrelationId,
				context.InitiatingUserId);

			return context;
		}

	    private ITracingService GetTraceService(CodeActivityContext executionContext)
	    {
		    var tracingService = executionContext.GetExtension<ITracingService>();

		    if (tracingService == null)
		    {
			    throw new InvalidWorkflowException("Failed to retrieve tracing service.");
		    }

			tracingService.Trace("Entered CalculateNextRunDate.Execute(), Activity Instance Id: {0}, Workflow Instance Id: {1}",
				executionContext.ActivityInstanceId,
				executionContext.WorkflowInstanceId);

			return tracingService;
	    }
    }
}
