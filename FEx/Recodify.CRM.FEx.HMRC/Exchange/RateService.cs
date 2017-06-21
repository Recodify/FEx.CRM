using System;
using System.Net;
using Recodify.CRM.FEx.Core.Exceptions;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Core.Web;
using Recodify.CRM.FEx.Rates.Models.Generic;
using RestSharp;

namespace Recodify.CRM.FEx.Core.Exchange
{
	public class RateService
	{
		private readonly ApiClient api;

		public RateService(IFExConfig config, ILoggingService trace, Guid correlationId)
		{					
			api = new ApiClient(config, trace, correlationId);
		}

		public ExchangeRateCollection GetRates(string crmUniqueName)
		{
			var response = api.GetRates(crmUniqueName);
			if (response.StatusCode != HttpStatusCode.OK)
			{
				var message = response?.Data?.Message;
				throw new RateSyncException(
					$"Error communicating with the rate api. Status Code: {response.StatusCode}. Message: {message}");
			}

			return response.Data;
		}
	}
}