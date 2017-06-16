using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
