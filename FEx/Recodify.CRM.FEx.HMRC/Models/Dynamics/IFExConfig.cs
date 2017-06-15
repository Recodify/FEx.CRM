﻿using System;
using Microsoft.Xrm.Sdk;
using Recodify.CRM.FEx.Core.Exchange;
using Recodify.CRM.FEx.Core.Monitoring;
using Recodify.CRM.FEx.Core.Scheduling;

namespace Recodify.CRM.FEx.Core.Models.Dynamics
{
	public interface IFExConfig
	{
		DateTimeOffset NextRunDate { get; set; }
		RateDataSource DataSource { get; }
		DateTime LastSyncDate { get; set; }		
		Frequency Frequency { get; }
		int Revision { get; }
		RunStatus LastRunStatus { get; }
		int Day { get; }
		decimal Time { get; }
		string RecodifyFExUrl { get; }
		Guid BaseCurrencyId { get; }
		Entity Entity { get; }
		void RemoveNonPersistableAttributes();
	}
}