using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Recodify.CRM.FEx.Core.Config;
using Recodify.CRM.FEx.Core.Exchange;
using Recodify.CRM.FEx.Core.Monitoring;
using Recodify.CRM.FEx.Core.Scheduling;

namespace Recodify.CRM.FEx.Core.Models.Dynamics
{
	public class FExConfig : IFExConfig
	{
		public FExConfig(Entity entity)
		{
			Entity = entity;
		}

		public string RecodifyFExUrl
		{
			get
			{
				var configUrl = ConfigurationManager.AppSettings["RecodifyFExUrl"];
				if (string.IsNullOrWhiteSpace(configUrl))
					return new PluginConfiguration().RecodifyFExUrl;

				return configUrl;
			}
		}

		public DateTimeOffset NextRunDate
		{
			get { return new DateTimeOffset(GetAttributeValue<DateTime>(ConfigAttribute.NextRun)); }
			set { SetAttributeValue(ConfigAttribute.NextRun, value.UtcDateTime); }
		}

		public DateTime LastSyncDate
		{
			get
			{
				var lastDate = default(DateTime);

				if (TryGetAttributeValue(ConfigAttribute.LastRunDate, out lastDate))
					return lastDate;

				return DateTime.UtcNow;
			}
			set { SetAttributeValue(ConfigAttribute.LastRunDate, value.ToUniversalTime()); }
		}

		public Entity Entity { get; }

		public int Revision => GetAttributeValue<int>(ConfigAttribute.Revision);

		public RunStatus LastRunStatus
		{
			get
			{
				OptionSetValue status;

				if (TryGetAttributeValue(ConfigAttribute.LastRunStatus, out status))
					return (RunStatus) status.Value;

				return RunStatus.Success;
			}
			set { SetAttributeAttributeValue(ConfigAttribute.LastRunStatus, () => new OptionSetValue((int) value)); }
		}

		public Frequency Frequency => (Frequency) GetAttributeValue<OptionSetValue>(ConfigAttribute.Frequency).Value;

		public RateDataSource DataSource
			=> (RateDataSource) GetAttributeValue<OptionSetValue>(ConfigAttribute.DataSource).Value;

		public Guid BaseCurrencyId => GetAttributeValue<EntityReference>(ConfigAttribute.BaseCurrencyId).Id;

		public int Day => GetAttributeValue<int>(ConfigAttribute.Day);

		public decimal Time => GetAttributeValue<decimal>(ConfigAttribute.Time);

		// It is only safe to persist the attributes not removed by this method as the others are editable by the user and will
		// cause another instance of the scheduler to be run.
		public void RemoveNonPersistableAttributes()
		{
			var attributesToRemove =
				ConfigAttribute.AllCustomAttributes.Where(x => !ConfigAttribute.PersistentAttributes.Contains(x));
			foreach (var a in attributesToRemove)
				Entity.Attributes.Remove(a);
		}

		private void SetAttributeValue(string attributeName, object value)
		{
			SetAttributeAttributeValue(attributeName, () => value);
		}

		private void SetAttributeAttributeValue(string attributeName, Func<object> valueBuilder)
		{
			if (Entity.Attributes.ContainsKey(attributeName))
				Entity.Attributes[attributeName] = valueBuilder();
			else
				Entity.Attributes.Add(attributeName, valueBuilder());
		}

		private bool TryGetAttributeValue<T>(string attributeName, out T value)
		{
			if (Entity.Attributes.ContainsKey(attributeName))
			{
				value = (T) Entity.Attributes[attributeName];
				return true;
			}

			value = default(T);
			return false;
		}

		private T GetAttributeValue<T>(string attributeName)
		{
			var value = default(T);

			if (TryGetAttributeValue(attributeName, out value))
				return value;

			throw new KeyNotFoundException("Unable to find attribute with key " + attributeName + ". Available keys are: " +
			                               Entity.Attributes.Keys.Aggregate((c, n) => c + "," + n));
		}
	}
}