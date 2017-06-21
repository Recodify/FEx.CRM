using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recodify.CRM.FEx.Core.Extensions;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Core.Models.Generic;
using Recodify.CRM.FEx.Rates.Models.Generic;
using RestSharp;

namespace Recodify.CRM.FEx.Core.Web
{
	public class ApiClient
	{
		private readonly IFExConfig config;
		private readonly ILoggingService trace;		
		private readonly RequestFactory requestFactory;
		private readonly IRestClient requestClient;

		public ApiClient(IFExConfig config, ILoggingService trace, Guid correlationId)
		{
			this.config = config;
			this.trace = trace;
			requestFactory = new RequestFactory(correlationId);
			requestClient = new RestClient(config.RecodifyFExUrl);
		}

		public IRestResponse<ExchangeRateCollection> GetRates(string crmUniqueName)
		{
			var resource = $"api/rates?rateSource={config.DataSource}&id={crmUniqueName}&baseCurrencyCode={config.BaseCurrencyCode}";			
			var request = requestFactory.Create(resource, Method.GET);

			var url = request.GetRequestUrl(requestClient);
			trace.Trace(TraceEventType.Verbose, (int)EventId.GetRatesApiRequest, $"Making Rates Request to: {url}");

			return requestClient.Execute<ExchangeRateCollection>(request);
		}

		public IRestResponse<SchedulingResult> GetNextRunDate(string crmUniqueName, int depth)
		{
			var resource = $"api/schedule?id={crmUniqueName}&frequency={config.Frequency}&day={config.Day}&time={config.Time}&lastRunStatus={config.LastRunStatus}&depth={depth}";			
			
			var request = requestFactory.Create(resource, Method.GET);

			var url = request.GetRequestUrl(requestClient);
			trace.Trace(TraceEventType.Verbose, (int)EventId.NextRunDateApiRequest, $"Making Scheduling Request to: {url}");

			return requestClient.Execute<SchedulingResult>(request);
		}		
	}
}
