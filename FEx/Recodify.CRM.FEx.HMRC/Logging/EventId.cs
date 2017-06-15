using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recodify.CRM.FEx.Core.Logging
{
	public enum EventId
	{
		GettingRatesFromApi = 1001,
		GettingCurrenciesFromCrm = 1002,
		SyncingCurrencies = 1003,
		SavingCurrencies = 1004,
		GettingUniqueOrganizationName = 1005,
		SavingLastSyncDate = 1006,
		UnableToFindRateForCurrency = 5001

	}
}
