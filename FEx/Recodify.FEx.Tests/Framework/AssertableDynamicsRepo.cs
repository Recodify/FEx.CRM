using System;
using Microsoft.Xrm.Sdk;
using Moq;
using Recodify.CRM.FEx.Core.Logging;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Core.Repositories;

namespace Recodify.CRM.FEx.Tests.Framework
{
	public class AssertableDynamicsRepo : DynamicsRepository
	{
		public int SaveCurrenciesCallCount;
		public int SaveNextRunDateCallCount;

		public AssertableDynamicsRepo(IOrganizationService organisationService)
			: base(organisationService, new Mock<ILoggingService>().Object)
		{
		}

		public override void SaveNextRunDate(IFExConfig config, DateTime nextRunDate)
		{
			base.SaveNextRunDate(config, nextRunDate);
			SaveNextRunDateCallCount++;
		}

		public override void SaveCurrencies(EntityCollection currencies)
		{
			base.SaveCurrencies(currencies);
			SaveCurrenciesCallCount++;
		}
	}
}