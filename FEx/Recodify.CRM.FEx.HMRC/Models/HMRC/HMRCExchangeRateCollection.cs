using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Recodify.CRM.FEx.Rates.Models.Generic;

namespace Recodify.CRM.FEx.Rates.Models.HMRC
{
	public class FixerExchangeRateCollection
	{
		public FixerExchangeRateCollection()
		{
			Rates = new Dictionary<string, decimal>();
		}

		public string Base { get; set; }
		public DateTime Date { get; set; }
		public Dictionary<string,decimal> Rates { get; set; }

		internal ExchangeRateCollection ToGeneric()
		{
			return new ExchangeRateCollection
			{
				Period = Date.ToShortTimeString(),
				Items = Rates.Select(x => new ExchangeRate
				{					
					CurrencyCode = x.Key,
					RateNew = x.Value
				}).ToList()
			};
		}
	}

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
				Base = "GBP",
				Items = Items.Select(x => new ExchangeRate
				{				
					CurrencyCode = x.CurrencyCode,
					RateNew = x.RateNew
				}).ToList()
			};
		}
	}
}