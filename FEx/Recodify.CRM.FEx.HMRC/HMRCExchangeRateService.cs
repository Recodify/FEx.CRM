using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Recodify.CRM.FEx.Core.Extensions;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Rates.Models.Generic;
using Recodify.CRM.FEx.Rates.Models.HMRC;
using RestSharp;

namespace Recodify.CRM.FEx.Rates
{
	public interface IExchangeRateService
	{
		ExchangeRateCollection GetRates(string urlFormat, string baseCurrencyCode);
	}

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

	public class HmrcExchangeRateService : IExchangeRateService
	{
		private readonly ILoggingService trace;

		public HmrcExchangeRateService(ILoggingService trace)
		{
			this.trace = trace;
		}

		public ExchangeRateCollection GetRates(string urlFormat, string baseCurrencyCode)
		{
			var formattedDateString = GetDateString();
			var baseUrl = string.Format(urlFormat, formattedDateString);
			var client = new RestClient(baseUrl);
			var request = new RestRequest(Method.GET);
			var response = client.Execute(request);

			var requestUrl = request.GetRequestUrl(client);
			trace.Trace(TraceEventType.Verbose, (int)EventId.GetRatesApiRequest, $"Making Rates Request to: {requestUrl}");

			var ser = new XmlSerializer(typeof(HmrcExchangeRateCollection));
			using (var reader = new StringReader(response.Content))
			{
				var hmrcExchangeRateCollection = ser.Deserialize(reader) as HmrcExchangeRateCollection;
				return hmrcExchangeRateCollection.ToGeneric();
			}
		}

		private string GetDateString()
		{
			var year = DateTime.UtcNow.ToString("yy");
			var date = $"{DateTime.Now.Month:d2}{year}";

			return date;
		}
	}
}