using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Recodify.CRM.FEx.Scheduling;

namespace Recodify.CRM.FEx.Data
{
	public class FExConfig
	{
		private readonly Entity entity;

		public FExConfig(Entity entity)
		{
			this.entity = entity;
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
		public int Day => GetAttributeValue<int>(ConfigAttribute.Day);
		public decimal Time => GetAttributeValue<decimal>(ConfigAttribute.Time);

		public void RemoveAllUserTriggeringAttributes()
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

			throw new KeyNotFoundException("Unable to find attribute with key " + attributeName + ". Available keys are: " + entity.Attributes.Keys.Aggregate((c,n) => c + "," + n));
		}
	}
}
