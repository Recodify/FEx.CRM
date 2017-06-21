using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Recodify.CRM.FEx.Core.Web;
using Recodify.Logging.Common;
using RestSharp;

namespace Recodify.CRM.FEx.Tests.Unit.Core.Web
{
	public class RequestFactoryTests
	{
		[Test]
		public void ShouldSetCorrelationId()
		{
			var correlationId = Guid.NewGuid();
			var requestFactory = new RequestFactory(correlationId);
			var result = requestFactory.Create("blah", Method.GET);

			var correlationIdHeader = result.Parameters.SingleOrDefault(x => x.Name == CustomHeader.CorrelationId);
			Assert.That(correlationIdHeader, Is.Not.Null);
			Assert.That(correlationIdHeader.Value, Is.EqualTo(correlationId.ToString()));
		}
	}
}
