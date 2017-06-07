using System;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;

namespace Recodify.CRM.FEx.Activities
{
    public class CalculateNextRunDateActivity : CodeActivity
    {
		[RequiredArgument]
		[Input("Counter")]
		[ReferenceTarget("c9_counter")]
		public InArgument<EntityReference> InputEntity { get; set; }

		protected override void Execute(CodeActivityContext executionContext)
		{
			var tracingService = GetTraceService(executionContext);	
			var workflowContext = GetWorkflowContext(executionContext, tracingService);
			var organizationService = GetOrganizationService(workflowContext.UserId, executionContext);
			var configEntity = GetFExConfiguration(executionContext, organizationService);

		}

	    private Entity GetFExConfiguration(CodeActivityContext executionContext, IOrganizationService organizationService)
	    {
		    var configEntity = organizationService.Retrieve("recodify_FExConfig",
			    this.InputEntity.Get(executionContext).Id, new ColumnSet(
				    "recodify_Frequency",
				    "recodify_Time",
				    "recodify_Day",
				    "recodify_BaseCurrencyId",
				    "recodify_NextRun",
				    "recodify_DataSource"));

			if (configEntity == null)
		    {
				throw new InvalidWorkflowException("Failed to retrieve FExConfig Entity.");
			}

		    return configEntity;
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
