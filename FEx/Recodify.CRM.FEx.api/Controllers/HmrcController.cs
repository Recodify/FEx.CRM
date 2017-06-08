using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Recodify.CRM.FEx.Rates;
using Recodify.CRM.FEx.Rates.Models.Generic;
using Recodify.CRM.FEx.Rates.Models.HMRC;

namespace Recodify.CRM.FEx.api.Controllers
{
	public class HmrcController : ApiController
	{
		// GET api/hmrc/blah
		public ExchangeRateCollection Get(string id)
		{
			var service = new HmrcExchangeRateService();
			var rates = service.GetRates(ConfigurationManager.AppSettings["exchangerate:url"]);
			return rates;
		}
	}
}
