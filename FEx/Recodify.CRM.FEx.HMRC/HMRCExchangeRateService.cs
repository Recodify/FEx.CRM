using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Recodify.CRM.FEx.Rates.Models.Generic;
using Recodify.CRM.FEx.Rates.Models.HMRC;
using RestSharp;

namespace Recodify.CRM.FEx.Rates
{
	public interface IExchangeRateService
	{
		ExchangeRateCollection GetRates(string urlFormat);
	}

	public class HmrcExchangeRateService : IExchangeRateService
	{		
		public ExchangeRateCollection GetRates(string urlFormat)
		{
			var formattedDateString = GetDateString();
			var url = string.Format(urlFormat, formattedDateString);
			var client = new RestClient(url);
			var request = new RestRequest(Method.GET);
			var response = client.Execute(request);

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
