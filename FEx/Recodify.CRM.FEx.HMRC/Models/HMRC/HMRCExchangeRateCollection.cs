using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Recodify.CRM.FEx.Rates.Models.Generic;

namespace Recodify.CRM.FEx.Rates.Models.HMRC
{
	[XmlRoot("exchangeRateMonthList")]
	public class HmrcExchangeRateCollection
	{
		[XmlElement("exchangeRate")]
		public List<HmrcExchangeRate> Items { get; set; }

		[XmlAttribute("Period")]
		public string Period { get; set; }

		internal ExchangeRateCollection ToGeneric()
		{
			return new ExchangeRateCollection
			{
				Period = Period,
				Items = Items.Select(x => new ExchangeRate
				{
					CountryCode = x.CountryCode,
					CountryName = x.CountryName,
					CurrencyCode = x.CurrencyCode,
					RateNew = x.RateNew
				}).ToList()
			};
		}
	}
}