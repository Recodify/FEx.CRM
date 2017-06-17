using System.Collections.Generic;

namespace Recodify.CRM.FEx.Rates.Models.Generic
{
	public class ExchangeRateCollection
	{
		public List<ExchangeRate> Items { get; set; }

		public string Period { get; set; }

		public string Message { get; set; }
	}
}