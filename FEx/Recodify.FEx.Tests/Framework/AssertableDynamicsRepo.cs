using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Recodify.CRM.FEx.Core.Repositories;

namespace Recodify.CRM.FEx.Tests.Framework
{
	public class AssertableDynamicsRepo : DynamicsRepository
	{
		public int SaveCurrenciesCallCount;

		public AssertableDynamicsRepo(IOrganizationService organisationService) : base(organisationService)
		{
			
		}

		public override void SaveCurrencies(EntityCollection currencies)
		{			
			base.SaveCurrencies(currencies);
			SaveCurrenciesCallCount++;
		}
	}
}
