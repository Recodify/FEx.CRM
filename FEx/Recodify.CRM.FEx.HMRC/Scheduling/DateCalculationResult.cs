using System;

namespace Recodify.CRM.FEx.Core.Scheduling
{
	public class DateCalculationResult
	{
		public DateCalculationResult(DateTimeOffset nextDate)
		{
			NextDate = nextDate;
		}

		public DateTimeOffset NextDate { get; }
	}
}