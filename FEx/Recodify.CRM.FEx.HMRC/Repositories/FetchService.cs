using System;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;

namespace Recodify.CRM.FEx.Core.Repositories
{
	public class FetchService
	{
		private readonly IOrganizationService service;

		public FetchService(IOrganizationService service)
		{
			this.service = service;
		}

		public EntityCollection Fetch(string fetchXml)
		{
			var fetchCount = 5000;
			var pageNumber = 1;
			string pagingCookie = null;

			var result = new EntityCollection();
			while (true)
			{
				var xml = CreateXml(fetchXml, pagingCookie, pageNumber, fetchCount);
				var fetchRequest1 = new RetrieveMultipleRequest
				{
					Query = new FetchExpression(xml)
				};

				var returnCollection = ((RetrieveMultipleResponse)service.Execute(fetchRequest1)).EntityCollection;

				result.Entities.AddRange(returnCollection.Entities);
				if (returnCollection.MoreRecords)
				{
					pageNumber++;
					pagingCookie = returnCollection.PagingCookie;
				}
				else
				{
					break;
				}
			}

			return result;
		}

		private string CreateXml(string xml, string cookie, int page, int count)
		{
			var stringReader = new StringReader(xml);
			var reader = new XmlTextReader(stringReader);

			// Load document
			var doc = new XmlDocument();
			doc.Load(reader);

			return CreateXml(doc, cookie, page, count);
		}

		private string CreateXml(XmlDocument doc, string cookie, int page, int count)
		{
			var attrs = doc.DocumentElement.Attributes;

			if (cookie != null)
			{
				var pagingAttr = doc.CreateAttribute("paging-cookie");
				pagingAttr.Value = cookie;
				attrs.Append(pagingAttr);
			}

			var pageAttr = doc.CreateAttribute("page");
			pageAttr.Value = Convert.ToString(page);
			attrs.Append(pageAttr);

			var countAttr = doc.CreateAttribute("count");
			countAttr.Value = Convert.ToString(count);
			attrs.Append(countAttr);

			var sb = new StringBuilder(1024);
			var stringWriter = new StringWriter(sb);

			var writer = new XmlTextWriter(stringWriter);
			doc.WriteTo(writer);
			writer.Close();

			return sb.ToString();
		}
	}
}
