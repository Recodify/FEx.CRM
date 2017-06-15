﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recodify.CRM.FEx.Core.Exchange;
using Recodify.CRM.FEx.Core.Models.Dynamics;
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
		public string RecodifyFExUrl { get { return ConfigurationManager.AppSettings["RecodifyFExUrl"]; } }
	}
}