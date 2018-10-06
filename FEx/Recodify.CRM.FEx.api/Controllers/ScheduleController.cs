using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Models.Generic;
using Recodify.CRM.FEx.Core.Monitoring;
using Recodify.CRM.FEx.Core.Scheduling;

namespace Recodify.CRM.FEx.api.Controllers
{
	public class ScheduleController : ApiController
	{
		public HttpResponseMessage Get(string id, Frequency frequency, int day, decimal time, RunStatus lastRunStatus,
			int depth)
		{
			var trace = new GenericLoggingService("Details");
			try
			{
				var calculator = new DateCalculator(DateTime.UtcNow, trace);
				var nextRunDate = calculator.Calculate(frequency, day, time, lastRunStatus, depth);

				if (!nextRunDate.HasValue)
				{
					return Request.CreateResponse(HttpStatusCode.BadRequest,
						new SchedulingResult
						{
							Message = "Unable to Calculate Next Run Date, ensure you have specified valid scheduling attributes"
						});
				}

				return Request.CreateResponse(HttpStatusCode.OK, new SchedulingResult {NextRunDate = nextRunDate.Value.UtcDateTime});
			}
			catch (Exception exp)
			{
				var message = $"Error calculating next run date. {exp.Message}";
				trace.Trace(TraceEventType.Error, (int) EventId.ScheduleApiError, message, exp);
				return Request.CreateResponse(HttpStatusCode.InternalServerError, new SchedulingResult {Message = message});
			}
		}
	}
}