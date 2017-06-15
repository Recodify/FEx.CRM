using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Recodify.CRM.FEx.Core.Exchange;
using Recodify.CRM.FEx.Core.Scheduling;

namespace Recodify.CRM.FEx.Core.Models.Dynamics
{
	public class FExConfig : IFExConfig
	{
		private readonly Entity entity;

		public FExConfig(Entity entity)
		{
			this.entity = entity;
		}

		public string RecodifyFExUrl
		{
			get
			{
				var configUrl = ConfigurationManager.AppSettings["RecodifyFExUrl"];
				if (string.IsNullOrWhiteSpace(configUrl))
				{
					return $"http://fex.recodify.co.uk/";
				}

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
			get { return GetAttributeValue<DateTime>(ConfigAttribute.LastRunDate); }
			set { SetAttributeValue(ConfigAttribute.LastRunDate, value.ToUniversalTime()); }
		}

		public Entity Entity => entity;
		public Frequency Frequency => (Frequency) (GetAttributeValue<OptionSetValue>(ConfigAttribute.Frequency)).Value;
		public RateDataSource DataSource => (RateDataSource)(GetAttributeValue<OptionSetValue>(ConfigAttribute.DataSource)).Value;
		public int Day => GetAttributeValue<int>(ConfigAttribute.Day);
		public decimal Time => GetAttributeValue<decimal>(ConfigAttribute.Time);

		// It is only safe to persist the attributes not removed by this method as the others are editable by the user and will
		// cause another instance of the scheduler to be run.
		public void RemoveNonPersistableAttributes()
		{
			var attributesToRemove = ConfigAttribute.AllCustomAttributes.Where(x => !ConfigAttribute.PersistentAttributes.Contains(x));
			foreach (var a in attributesToRemove)
			{
				this.entity.Attributes.Remove(a);
			}
		}

		private void SetAttributeValue(string attributeName, object value)
		{
			if (entity.Attributes.ContainsKey(attributeName))
			{
				entity.Attributes[attributeName] = value;
			}
			else
			{
				entity.Attributes.Add(attributeName, value);
			}
		}

		private T GetAttributeValue<T>(string attributeName)
		{
			if (entity.Attributes.ContainsKey(attributeName))
			{
				return (T) entity.Attributes[attributeName];
			}

			throw new KeyNotFoundException("Unable to find attribute with key " + attributeName + ". Available keys are: " + Enumerable.Aggregate<string>(entity.Attributes.Keys, (c,n) => c + "," + n));
		}
	}
}