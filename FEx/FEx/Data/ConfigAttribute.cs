using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recodify.CRM.FEx.Data
{
	public class ConfigAttribute
	{
		public const string ConfigEntityName = "recodify_fexconfig";
		public const string NextRun = "recodify_nextrun";
		public const string DataSource = "recodify_datasource";
		public const string BaseCurrencyId = "recodify_basecurrencyid";
		public const string Day = "recodify_day";
		public const string Time = "recodify_time";
		public const string Frequency = "recodify_frequency";

		public static string[] AllCustomAttriutes => new[]
		{
			NextRun,
			DataSource,
			BaseCurrencyId,
			Day,
			Time,
			Frequency
		};
	}
}
