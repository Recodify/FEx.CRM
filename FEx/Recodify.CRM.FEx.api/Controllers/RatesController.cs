using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Recodify.CRM.FEx.Core.Exchange;
using Recodify.CRM.FEx.Rates;
using Recodify.CRM.FEx.Rates.Models.Generic;
using Recodify.CRM.FEx.Rates.Models.HMRC;

namespace Recodify.CRM.FEx.api.Controllers
{
	public class RatesController : ApiController
	{
		// GET api/rates/blah
		public HttpResponseMessage Get(string id, string rateSource)
		{
			if (rateSource.Equals(RateDataSource.Fixer.ToString(), StringComparison.InvariantCultureIgnoreCase))
			{
				return BuildSuccessResponse(new ExchangeRateCollection());
			}
			else if (rateSource.Equals(RateDataSource.Hmrc.ToString(), StringComparison.InvariantCultureIgnoreCase))
			{
				var service = new HmrcExchangeRateService();
				var rates = service.GetRates(ConfigurationManager.AppSettings["exchangerate:url"]);
				return BuildSuccessResponse(rates);
			}
			
			return Request.CreateResponse(HttpStatusCode.BadRequest, new ExchangeRateCollection{ Message ="Unknown Rate Source Specified"});			
		}

		private HttpResponseMessage BuildSuccessResponse(ExchangeRateCollection rates)
		{
			return Request.CreateResponse(HttpStatusCode.OK, rates);
		}
	}
}
