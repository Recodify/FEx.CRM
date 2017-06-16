using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Core.Repositories;

namespace Recodify.CRM.FEx.Tests.Framework
{
	public class AssertableDynamicsRepo : DynamicsRepository
	{
		public int SaveCurrenciesCallCount;
		public int SaveNextRunDateCallCount;

		public AssertableDynamicsRepo(IOrganizationService organisationService) : base(organisationService)
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
