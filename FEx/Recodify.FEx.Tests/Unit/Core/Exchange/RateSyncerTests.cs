using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Moq;
using NUnit.Framework;
using Recodify.CRM.FEx.Core.Exchange;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Rates.Models.Generic;
using Recodify.CRM.FEx.Tests.Framework;

namespace Recodify.CRM.FEx.Tests.Unit
{
	public class RateSyncerTests
	{
		[Test]
		public void ShouldLogWarning_WhenRateDataNotAvaliable()
		{
			var loggingService = new Mock<ILoggingService>();
			var baseCurrencyId = Guid.NewGuid();
			var rateSyncer = new RateSyncer(new MockFExConfig {BaseCurrencyId = baseCurrencyId}, loggingService.Object);

			var currencies = CreateCurrencyCollection(baseCurrencyId);

			var rates = new ExchangeRateCollection
			{
				Items = new List<ExchangeRate>
				{
					new ExchangeRate {CurrencyCode = CurrencyCode.GBP, RateNew = 1},
					new ExchangeRate {CurrencyCode = CurrencyCode.USD, RateNew = 1.4M}
				}
			};

			rateSyncer.Sync(currencies, rates);
			loggingService.Verify(
				x =>
					x.Trace(TraceEventType.Warning, (int) EventId.UnableToFindRateForCurrency,
						It.Is<string>(y => y.Contains(CurrencyCode.EUR))), Times.Once);
		}

		[Test]
		public void ShouldUpdateExchangeRatesForPassedCurrencies()
		{
			var baseCurrencyId = Guid.NewGuid();
			var rateSyncer = new RateSyncer(new MockFExConfig {BaseCurrencyId = baseCurrencyId},
				new Mock<ILoggingService>().Object);
			var currencies = CreateCurrencyCollection(baseCurrencyId);

			var rates = new ExchangeRateCollection
			{
				Items = new List<ExchangeRate>
				{
					new ExchangeRate {CurrencyCode = CurrencyCode.GBP, RateNew = 1},
					new ExchangeRate {CurrencyCode = CurrencyCode.EUR, RateNew = 1.2M},
					new ExchangeRate {CurrencyCode = CurrencyCode.USD, RateNew = 1.4M}
				}
			};

			var result = rateSyncer.Sync(currencies, rates);

			var eur = result.Entities.Single(x => (string) x.Attributes[CurrencyAttribute.CurrencyCode] == CurrencyCode.EUR);
			var usd = result.Entities.Single(x => (string) x.Attributes[CurrencyAttribute.CurrencyCode] == CurrencyCode.USD);
			Assert.That((decimal) eur.Attributes[CurrencyAttribute.ExchangeRate], Is.EqualTo(1.2M));
			Assert.That((decimal) usd.Attributes[CurrencyAttribute.ExchangeRate], Is.EqualTo(1.4M));
		}

		[Test]
		public void ShouldNotReturnBaseCurrencies()
		{
			var baseCurencyId = Guid.NewGuid();
			var rateSyncer = new RateSyncer(new MockFExConfig {BaseCurrencyId = baseCurencyId},
				new Mock<ILoggingService>().Object);

			var currencies = CreateCurrencyCollection(baseCurencyId);

			var rates = new ExchangeRateCollection
			{
				Items = new List<ExchangeRate>
				{
					new ExchangeRate {CurrencyCode = CurrencyCode.GBP, RateNew = 99},
					new ExchangeRate {CurrencyCode = CurrencyCode.EUR, RateNew = 1.2M},
					new ExchangeRate {CurrencyCode = CurrencyCode.USD, RateNew = 1.4M}
				}
			};

			var result = rateSyncer.Sync(currencies, rates);

			var gbp =
				result.Entities.SingleOrDefault(x => (string) x.Attributes[CurrencyAttribute.CurrencyCode] == CurrencyCode.GBP);
			Assert.That(gbp, Is.Null);
		}

		[Test]
		public void ShouldNotLogWarning_WhenBaseCurrencyNotSupplied()
		{
			var loggingService = new Mock<ILoggingService>();
			var baseCurencyId = Guid.NewGuid();
			var rateSyncer = new RateSyncer(new MockFExConfig { BaseCurrencyId = baseCurencyId }, loggingService.Object);

			var currencies = CreateCurrencyCollection(baseCurencyId);

			var rates = new ExchangeRateCollection
			{
				Items = new List<ExchangeRate>
				{					
					new ExchangeRate {CurrencyCode = CurrencyCode.EUR, RateNew = 1.2M},
					new ExchangeRate {CurrencyCode = CurrencyCode.USD, RateNew = 1.4M}
				}
			};

			rateSyncer.Sync(currencies, rates);

			loggingService.Verify(
				x =>
					x.Trace(TraceEventType.Warning, (int)EventId.UnableToFindRateForCurrency,
						It.IsAny<string>()), Times.Never);
		}

		private EntityCollection CreateCurrencyCollection(Guid baseCurrencyId)
		{
			var currencies = new EntityCollection(new List<Entity>
			{
				CreateCurrencyEntity(CurrencyCode.GBP, baseCurrencyId),
				CreateCurrencyEntity(CurrencyCode.USD, Guid.NewGuid()),
				CreateCurrencyEntity(CurrencyCode.EUR, Guid.NewGuid())
			});
			return currencies;
		}

		private Entity CreateCurrencyEntity(string currencyCode, Guid currencyId)
		{
			var attributes = new AttributeCollection();
			attributes.Add(CurrencyAttribute.CurrencyCode, currencyCode);
			attributes.Add(CurrencyAttribute.ExchangeRate, 1M);
			return new Entity(CurrencyAttribute.EntityName)
			{
				Id = currencyId,
				Attributes = attributes
			};
		}
	}
}