using System;
using System.Web;
using System.Web.Http;
using Recodify.Logging.Common;
using HttpContext = System.Web.HttpContext;

namespace Recodify.CRM.FEx.api
{
	public class WebApiApplication : HttpApplication
	{
		protected void Application_Start()
		{
			GlobalConfiguration.Configure(WebApiConfig.Register);
		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			if (HttpContext.Current != null)
			{
				HttpContext.Current.Items.Add(CustomHeader.RequestId, Guid.NewGuid());
				var correlationId = HttpContext.Current.Request.Headers[CustomHeader.CorrelationId];
				if (correlationId != null)
					HttpContext.Current.Items.Add(CustomHeader.CorrelationId, new Guid(correlationId));
			}
		}
	}
}