using System;
using System.Activities;
using System.Diagnostics;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Recodify.CRM.FEx.Core.Extensions;
using Recodify.CRM.FEx.Core.Jobs;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Core.Monitoring;
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
			// TODO not very happy about these being outside try catch but need to think of way
			// of retrying a workflow even when we don't have a context and org service!

			var workflowContext = GetWorkflowContext(executionContext);
			var organisationService = GetOrganizationService(workflowContext.UserId, executionContext);
			var tracingService = GetTraceService(executionContext);
			var trace = new DynamicsLoggingService(tracingService, organisationService.GetUniqueOrganisationName(),
				workflowContext.CorrelationId);			

			var config = organisationService.GetFExConfiguration(workflowContext.PrimaryEntityId,
				ConfigAttribute.SchedulingAttributes);
			try
			{
				var repo = new DynamicsRepository(organisationService, trace);
				var nextRunDate = new CalculateNextRunDateJob(repo, config, trace, workflowContext.Depth).Execute();
				CurrentRevision.Set(executionContext, config.Revision);
				NextRunDate.Set(executionContext, nextRunDate.NextDate.UtcDateTime);
			}
			catch (Exception exp)
			{
				var msg = "Error has Occured Running CalculateNextRunDate Activity. Seting Last Run Status to Error and logging.";
				HandleException(exp, msg, config, organisationService, trace);
			}
		}
	}
}