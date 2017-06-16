using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Recodify.CRM.FEx.Core.Monitoring;
using Recodify.CRM.FEx.Core.Scheduling;

namespace Recodify.CRM.FEx.api.Controllers
{
    public class ScheduleController : ApiController
    {
          
        public HttpResponseMessage Get(Frequency frequency, int day, decimal time, RunStatus lastRunStatus, int depth)
        {
			//var calculator = new DateCalculator(DateTime.UtcNow, )
			return null;
        }
    }
}
