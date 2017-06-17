namespace Recodify.CRM.FEx.Rates.Models.Generic
{
	public class ExchangeRate
	{
		public string CountryName { get; set; }
		public string CountryCode { get; set; }
		public string CurrencyCode { get; set; }
		public decimal RateNew { get; set; }
	}
}