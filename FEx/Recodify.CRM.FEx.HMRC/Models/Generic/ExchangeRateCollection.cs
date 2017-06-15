using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recodify.CRM.FEx.Rates.Models.Generic
{	
	public class ExchangeRateCollection
	{
		public List<ExchangeRate> Items { get; set; }
	
		public string Period { get; set; }

		public string Message { get; set; }
	}
}
