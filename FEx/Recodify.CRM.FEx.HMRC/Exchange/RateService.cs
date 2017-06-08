using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Rates.Models.Generic;

namespace Recodify.CRM.FEx.Core.Exchange
{
	public class RateService
	{
		private readonly FExConfig config;

		public RateService(FExConfig config)
		{
			this.config = config;
		}

		public ExchangeRateCollection GetRates()
		{
			return null;
		}
	}	
}
