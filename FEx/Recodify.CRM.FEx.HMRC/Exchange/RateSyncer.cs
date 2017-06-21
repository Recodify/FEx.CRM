using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Models.Dynamics;
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
			currencies.Entities.Remove(currencies.Entities.Single(x => x.Id == config.BaseCurrencyId));

			foreach (var cur in currencies.Entities)
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
					var msg = $"Setting currency {currencyCode} to {rate.RateNew}";
					trace.Trace(TraceEventType.Verbose, (int) EventId.CurrencySyncSetExchangeRate, msg);
				}
			}
			
			return currencies;
		}
	}
}