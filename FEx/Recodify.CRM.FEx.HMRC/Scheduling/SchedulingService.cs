using System;
using System.Diagnostics;
using System.Net;
using Recodify.CRM.FEx.Core.Exceptions;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Core.Models.Generic;
using RestSharp;

namespace Recodify.CRM.FEx.Core.Scheduling
{
	public class SchedulingService
	{
		private readonly IFExConfig config;
		private readonly ILoggingService trace;

		public SchedulingService(IFExConfig config, ILoggingService trace)
		{
			this.config = config;
			this.trace = trace;
		}

		public DateTime GetNextRunDate(string crmUniqueName, int depth)
		{
			var client = new RestClient(config.RecodifyFExUrl);
			var request = new RestRequest(BuildResourceUrl(crmUniqueName, depth), Method.GET);
			var url = $"{client.BaseUrl}/{request.Resource}";
			trace.Trace(TraceEventType.Verbose, (int) EventId.NextRunDateOutput, $"Making Scheduling Request to: {url}");
			var response = client.Execute<SchedulingResult>(request);

			if (response.StatusCode != HttpStatusCode.OK)
			{
				var message = response?.Data?.Message;
				throw new SchedulingException(
					$"Error communicating with the scheduling api. Status Code: {response.StatusCode}. Message: {message}");
			}

			return response.Data.NextRunDate;
		}

		private string BuildResourceUrl(string crmUniqueName, int depth)
		{
			return
				$"api/schedule?id={crmUniqueName}&frequency={config.Frequency}&day={config.Day}&time={config.Time}&lastRunStatus={config.LastRunStatus}&depth={depth}";
		}
	}
}