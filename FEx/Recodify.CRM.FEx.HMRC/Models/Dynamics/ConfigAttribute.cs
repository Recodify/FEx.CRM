namespace Recodify.CRM.FEx.Core.Models.Dynamics
{
	public class ConfigAttribute
	{
		public const string ConfigEntityName = "recodify_fexconfig";
		public const string NextRun = "recodify_nextrun";
		public const string DataSource = "recodify_datasource";
		public const string BaseCurrencyId = "_recodify_basecurrencyid_value";
		public const string Day = "recodify_day";
		public const string Time = "recodify_time";
		public const string Frequency = "recodify_frequency";
		public const string LastRunDate = "recodify_lastrundate";

		public static string[] AllCustomAttributes => new[]
		{
			LastRunDate,
			NextRun,
			DataSource,
			BaseCurrencyId,
			Day,
			Time,
			Frequency
		};

		public static string[] PersistentAttributes => new[]
		{
			LastRunDate,
			NextRun
		};

		public static string[] RunAttributes => new[]
		{
			LastRunDate,
			NextRun,
			DataSource,
			BaseCurrencyId,			
		};

		public static string[] SchedulingAttributes => new[]
		{
			Day,
			Time,
			Frequency
		};
	}
}
