namespace Recodify.CRM.FEx.Core.Logging
{
	public enum EventId
	{
		// Verbose Information
		NextRunDateApiRequest = 13001,
		GetRatesApiRequest = 13002,
		CurrencySyncSetExchangeRate = 13103,

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
		CalculatingFromSchedule = 1209,
		BackOffAndRetry = 1210,
		CompletedRateSyncSuccess = 1295,

		// Warning
		CompletedRateSyncWarning = 4295,
		CompletedNextRunDateCalculationWarning = 2195,

		// Error
		UnableToFindRateForCurrency = 5001,
		CompletedNextRunDateCalculationError = 5195,
		CompletedRateSyncError = 5495,
		ScheduleApiError = 5501
	}
}