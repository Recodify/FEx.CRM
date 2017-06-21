using System.Collections.Generic;
using System.Xml.Serialization;

namespace Recodify.CRM.FEx.Rates.Models.HMRC
{	
	public class HmrcExchangeRate
	{
		[XmlElement("countryName")]
		public string CountryName { get; set; }

		[XmlElement("countryCode")]
		public string CountryCode { get; set; }

		[XmlElement("currencyCode")]
		public string CurrencyCode { get; set; }

		[XmlElement("rateNew")]
		public decimal RateNew { get; set; }
	}
}