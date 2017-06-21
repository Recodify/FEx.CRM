using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recodify.Logging.Common;
using RestSharp;

namespace Recodify.CRM.FEx.Core.Web
{
	public class RequestFactory
	{
		private readonly Guid correlationId;

		public RequestFactory(Guid correlationId)
		{
			this.correlationId = correlationId;
		}

		public IRestRequest Create(string resource, Method method)
		{
			var request = new RestRequest(resource, method);
			request.AddHeader(CustomHeader.CorrelationId, correlationId.ToString());
			return request;
		}
	}
}
