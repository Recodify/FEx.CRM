using System.Net;
using Recodify.CRM.FEx.Core.Exceptions;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Rates.Models.Generic;
using RestSharp;

namespace Recodify.CRM.FEx.Core.Exchange
{
	public class RateService
	{
		private readonly IFExConfig config;

		public RateService(IFExConfig config)
		{
			this.config = config;
		}

		public ExchangeRateCollection GetRates(string crmUniqueName)
		{
			var client = new RestClient(config.RecodifyFExUrl);
			var request = new RestRequest(BuildResourceUrl(crmUniqueName), Method.GET);
			var response = client.Execute<ExchangeRateCollection>(request);

			if (response.StatusCode != HttpStatusCode.OK)
			{
				var message = response?.Data?.Message;
				throw new RateSyncException(
					$"Error communicating with the rate api. Status Code: {response.StatusCode}. Message: {message}");
			}

			return response.Data;
		}

		private string BuildResourceUrl(string crmUniqueName)
		{
			return $"api/rates?rateSource={config.DataSource}&id={crmUniqueName}";
		}
	}
}