using Recodify.CRM.FEx.Core.Extensions;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Rates;
using Recodify.CRM.FEx.Rates.Models.Generic;
using Recodify.CRM.FEx.Rates.Models.HMRC;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recodify.CRM.FEx.Rates
{
	public class FixerExchangeRateService : IExchangeRateService
	{
		private readonly ILoggingService trace;

		public FixerExchangeRateService(ILoggingService trace)
		{
			this.trace = trace;
		}

		public ExchangeRateCollection GetRates(string urlFormat, string baseCurrencyCode)
		{
			var baseUrl = string.Format(urlFormat, baseCurrencyCode);
			var client = new RestClient(baseUrl);
			var request = new RestRequest(Method.GET);

			var requestUrl = request.GetRequestUrl(client);
			trace.Trace(TraceEventType.Verbose, (int)EventId.GetRatesApiRequest, $"Making Rates Request to: {requestUrl}");

			return client.Execute<FixerExchangeRateCollection>(request).Data.ToGeneric();
		}
	}
}
