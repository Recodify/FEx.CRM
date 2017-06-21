using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace Recodify.CRM.FEx.Core.Extensions
{
	public static class IRestRequestExtensions
	{
		public static string GetRequestUrl(this IRestRequest request, IRestClient client)
		{
			return $"{client.BaseUrl}/{request.Resource}";
		}
	}
}
