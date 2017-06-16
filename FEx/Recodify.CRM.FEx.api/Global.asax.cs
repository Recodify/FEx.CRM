using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Recodify.CRM.FEx.api
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{			
			GlobalConfiguration.Configure(WebApiConfig.Register);			
		}

		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			if (HttpContext.Current != null)
			{
				HttpContext.Current.Items.Add("RequestId", Guid.NewGuid());
			}
		}
	}
}
