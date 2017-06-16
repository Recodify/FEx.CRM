using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using Recodify.Logging.Common;
using Recodify.Logging.Trace;
using Recodify.Logging.Trace.Sanitisation;
using Recodify.Logging.WebApi;
using HttpContext = System.Web.HttpContext;

namespace Recodify.CRM.FEx.api
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			// Web API routes
			config.MapHttpAttributeRoutes();
			config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			AddLoggingHandlers(config);
		}

		private static void AddLoggingHandlers(HttpConfiguration config)
		{
			if ("true".Equals(ConfigurationManager.AppSettings["EnableLogging"], StringComparison.InvariantCultureIgnoreCase))
			{
				var requestTraceSource = new SanitisedTraceSource("Request", new WebDataEnricher(), new Sanitiser());
				var responseTraceSource = new SanitisedTraceSource("Response", new WebDataEnricher(), new Sanitiser());
				var logHandler = new LogHandler(requestTraceSource, responseTraceSource, new Recodify.Logging.Common.HttpContext(), new Options());
				config.MessageHandlers.Add(logHandler);
			}
		}
	}
}
