using System;
using System.Configuration;
using Microsoft.Xrm.Sdk;
using Recodify.CRM.FEx.Core.Exchange;
using Recodify.CRM.FEx.Core.Models.Dynamics;
using Recodify.CRM.FEx.Core.Monitoring;
using Recodify.CRM.FEx.Core.Scheduling;

namespace Recodify.CRM.FEx.Tests.Framework
{
	public class MockFExConfig : IFExConfig
	{
		public DateTimeOffset NextRunDate { get; set; }
		public RateDataSource DataSource { get; set; }
		public DateTime LastSyncDate { get; set; }
		public Frequency Frequency { get; set; }
		public int Day { get; set; }
		public decimal Time { get; set; }

		public string RecodifyFExUrl
		{
			get { return ConfigurationManager.AppSettings["RecodifyFExUrl"]; }
		}

		public Guid BaseCurrencyId { get; set; }
		public int Revision { get; set; }
		public RunStatus LastRunStatus { get; set; }

		public void RemoveNonPersistableAttributes()
		{
			throw new NotImplementedException();
		}

		public Entity Entity { get; }
	}
}