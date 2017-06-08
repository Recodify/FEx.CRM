using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recodify.CRM.FEx
{
	public static class IntegerExtensions
	{
		public static string ToDayOfWeekString(this int day)
		{
			switch (day)
			{
				case 1:
					return "MON";
				case 2:
					return "TUE";
				case 3:
					return "WED";
				case 4:
					return "THUR";
				case 5:
					return "FRI";
				case 6:
					return "SAT";
				default:
					return "SUN";
			}
		}
	}
}
