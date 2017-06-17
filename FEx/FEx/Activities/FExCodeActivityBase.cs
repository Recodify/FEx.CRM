using System;
using System.Activities;
using System.Diagnostics;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Recodify.CRM.FEx.Core.Jobs;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Core.Monitoring;

namespace Recodify.CRM.FEx.Dynamics.Activities
{
	public abstract class FExCodeActivityBase : CodeActivity
	{
		protected IOrganizationService GetOrganizationService(Guid userId, CodeActivityContext executionContext)
		{
			var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
			var service = serviceFactory.CreateOrganizationService(userId);
			if (service == null)
				throw new InvalidWorkflowException("Failed to create OrganizationService.");

			return service;
		}

		protected IWorkflowContext GetWorkflowContext(CodeActivityContext executionContext)
		{
			var context = executionContext.GetExtension<IWorkflowContext>();

			if (context == null)
				throw new InvalidWorkflowException("Failed to retrieve workflow context.");

			return context;
		}

		protected ITracingService GetTraceService(CodeActivityContext executionContext)
		{
			var tracingService = executionContext.GetExtension<ITracingService>();

			if (tracingService == null)
				throw new InvalidWorkflowException("Failed to retrieve tracing service.");

			tracingService.Trace("Entered CalculateNextRunDate.Execute(), Activity Instance Id: {0}, Workflow Instance Id: {1}",
				executionContext.ActivityInstanceId,
				executionContext.WorkflowInstanceId);

			return tracingService;
		}

		protected void HandleException(
			Exception exp,
			string message, 
			IFExConfig config, 
			IOrganizationService organisationService,
			ILoggingService trace)
		{			
			trace.Trace(TraceEventType.Error, 1, message);
			trace.Trace(TraceEventType.Error, 1, exp);
			new JobCompleter(organisationService, config, trace).Complete(RunStatus.Error);

			// In Dynamics you cannot throw custom exceptions as serilization of them requires reflection which
			// is not permitted in an exectuion sandbox.
			throw new InvalidPluginExecutionException(message);
		}
	}
}