using System;
using System.Web;
using System.Web.Http;

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
				HttpContext.Current.Items.Add("RequestId", Guid.NewGuid());
		}
	}
}