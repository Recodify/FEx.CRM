using System;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using Recodify.CRM.FEx.Core.Models.Dynamics;

namespace Recodify.CRM.FEx.Dynamics.Activities
{
	public abstract class FExCodeActivityBase : CodeActivity
	{
		protected IOrganizationService GetOrganizationService(Guid userId, CodeActivityContext executionContext)
		{
			IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
			var service = serviceFactory.CreateOrganizationService(userId);
			if (service == null)
			{
				throw new InvalidWorkflowException("Failed to create OrganizationService.");
			}
			
			return service;
		}

		protected IWorkflowContext GetWorkflowContext(CodeActivityContext executionContext, ITracingService tracingService)
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

		protected ITracingService GetTraceService(CodeActivityContext executionContext)
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