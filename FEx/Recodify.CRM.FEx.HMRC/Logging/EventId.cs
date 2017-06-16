﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recodify.CRM.FEx.Core.Logging
{
	public enum EventId
	{
		// Information
		StartingNextRunDateCalculation = 1101,
		CompletedNextRunDateCalculationSuccess = 1195,

		StartingRateSync = 1201,
		GettingRatesFromApi = 1202,
		GettingCurrenciesFromCrm = 1203,
		SyncingCurrencies = 1204,
		SavingCurrencies = 1205,
		GettingUniqueOrganizationName = 1206,
		SavingLastSyncDate = 1207,
		SavingLastRunStatus = 1208,
		CompletedRateSyncSuccess = 1295,

		// Warning
		CompletedRateSyncWarning = 4295,
		CompletedNextRunDateCalculationWarning = 2195,

		// Error
		UnableToFindRateForCurrency = 5001,
		CompletedNextRunDateCalculationError = 5195,
		CompletedRateSyncError = 5495

	}
}
