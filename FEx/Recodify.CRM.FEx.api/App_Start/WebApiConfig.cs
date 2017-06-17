using System;
using System.Configuration;
using System.Net.Http.Headers;
using System.Web.Http;
using Recodify.Logging.Common;
using Recodify.Logging.Trace;
using Recodify.Logging.Trace.Sanitisation;
using Recodify.Logging.WebApi;

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
				"DefaultApi",
				"api/{controller}/{id}",
				new {id = RouteParameter.Optional}
			);

			AddLoggingHandlers(config);
		}

		private static void AddLoggingHandlers(HttpConfiguration config)
		{
			if ("true".Equals(ConfigurationManager.AppSettings["EnableLogging"], StringComparison.InvariantCultureIgnoreCase))
			{
				var requestTraceSource = new SanitisedTraceSource("Request", new WebDataEnricher(), new Sanitiser());
				var responseTraceSource = new SanitisedTraceSource("Response", new WebDataEnricher(), new Sanitiser());
				var logHandler = new LogHandler(requestTraceSource, responseTraceSource, new HttpContext(), new Options());
				config.MessageHandlers.Add(logHandler);
			}
		}
	}
}