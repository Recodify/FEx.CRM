using System;
using System.Diagnostics;
using System.Net;
using Recodify.CRM.FEx.Core.Exceptions;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Core.Models.Generic;
using Recodify.CRM.FEx.Core.Web;
using RestSharp;

namespace Recodify.CRM.FEx.Core.Scheduling
{
	public class SchedulingService
	{		
		private readonly ApiClient api;

		public SchedulingService(IFExConfig config, ILoggingService trace, Guid correlationId)
		{
			api = new ApiClient(config, trace, correlationId);
		}

		public DateTime GetNextRunDate(string crmUniqueName, int depth)
		{
			var response = api.GetNextRunDate(crmUniqueName, depth);
			if (response.StatusCode != HttpStatusCode.OK)
			{
				var message = response?.Data?.Message;
				throw new SchedulingException(
					$"Error communicating with the scheduling api. Status Code: {response.StatusCode}. Message: {message}");
			}

			return response.Data.NextRunDate;
		}
	}
}