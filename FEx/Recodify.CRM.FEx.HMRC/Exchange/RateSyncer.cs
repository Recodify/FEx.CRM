using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Core.Repositories;
using Recodify.CRM.FEx.Rates.Models.Generic;

namespace Recodify.CRM.FEx.Core.Exchange
{	
	public class RateSyncer
	{
		private readonly IFExConfig config;
		private readonly ILoggingService trace;

		public RateSyncer(IFExConfig config, ILoggingService trace)
		{
			this.config = config;
			this.trace = trace;
		}

		public EntityCollection Sync(EntityCollection currencies, ExchangeRateCollection rates)
		{
			foreach (var cur in currencies.Entities.Where(x => x.Id != config.BaseCurrencyId))
			{
				var currencyCode = (string) cur.Attributes[CurrencyAttribute.CurrencyCode];
				var rate = rates.Items.SingleOrDefault(x => x.CurrencyCode.Equals(currencyCode, StringComparison.OrdinalIgnoreCase));
				if (rate == null)
				{
					trace.Trace(TraceEventType.Warning, (int) EventId.UnableToFindRateForCurrency,
						$"Unable to find rate for currency with code {currencyCode}. Consider chossing a different data source that supplies data for your currency set.");
				}
				else
				{
					cur.Attributes[CurrencyAttribute.ExchangeRate] = rate.RateNew;
				}				
			}

			return currencies;
		}
	}
}
