using System;
using System.Collections.Generic;
using System.Text;

namespace CodeCharm.Utility
{
    public static class Extension
    {
        public static string ToStringEnglish(this TimeSpan timeSpan)
        {
            return SystemTimeSpanHelper.ToString(timeSpan);
        }
    }

    public class SystemTimeSpanHelper
	{
		public static string ToString(TimeSpan timeSpan)
		{
			List<string> intervalFields = new List<string>();
			if (timeSpan.Days > 0)
			{
				intervalFields.Add(timeSpan.Days.ToString() + " days");
			}
			if (timeSpan.Hours > 0)
			{
				intervalFields.Add(timeSpan.Hours.ToString() + " hours");
			}
			if (timeSpan.Minutes > 0)
			{
				intervalFields.Add(timeSpan.Minutes.ToString() + " minutes");
			}
			string seconds = timeSpan.Seconds.ToString();
			if (timeSpan.Milliseconds > 0)
			{
				seconds += "." + timeSpan.Milliseconds.ToString("000");
			}
			seconds += " seconds";
			intervalFields.Add(seconds);
			return string.Join(", ", intervalFields.ToArray());
		}
	}
  
}
