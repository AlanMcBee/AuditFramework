using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace CodeCharm.Utility
{
    public class Iso8601Duration
    {
        public Iso8601Duration()
        {
        }

        public Iso8601Duration(TimeSpan duration)
        {
            if (duration.Ticks < 0)
            {
                Signum = -1;
            }
            Days = Math.Abs(duration.Days);
            Hours = Math.Abs(duration.Hours);
            Minutes = Math.Abs(duration.Minutes);
            Seconds = Math.Abs(duration.Seconds);
            FractionOfSecond = Math.Abs(duration.Milliseconds / 1000M);
        }

        private int _signum = 1;
        public int Signum
        {
            get
            {
                return _signum;
            }
            set
            {
                if (Math.Abs(value) != 1)
                {
                    throw new ArgumentOutOfRangeException("Must be either 1 or -1");
                }
                _signum = value;
            }
        }

        private int _years;
        public int Years
        {
            get
            {
                return _years;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Must be greater than or equal to 0");
                }
                _years = value;
            }
        }

        private int _months;
        public int Months
        {
            get
            {
                return _months;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Must be greater than or equal to 0");
                }
                _months = value;
            }
        }

        private int _weeks;
        public int Weeks
        {
            get
            {
                return _weeks;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Must be greater than or equal to 0");
                }
                _weeks = value;
            }
        }

        private int _days;
        public int Days
        {
            get
            {
                return _days;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Must be greater than or equal to 0");
                }
                _days = value;
            }
        }

        private int _hours;
        public int Hours
        {
            get
            {
                return _hours;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Must be greater than or equal to 0");
                }
                _hours = value;
            }
        }

        private int _minutes;
        public int Minutes
        {
            get
            {
                return _minutes;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Must be greater than or equal to 0");
                }
                _minutes = value;
            }
        }

        private int _seconds;
        public int Seconds
        {
            get
            {
                return _seconds;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Must be greater than or equal to 0");
                }
                _seconds = value;
            }
        }

        private decimal _fraction;
        public decimal FractionOfSecond
        {
            get
            {
                return _fraction;
            }
            set
            {
                if (value < 0M || value >= 1.0M)
                {
                    throw new ArgumentOutOfRangeException("value", "Must be less than 1.0M and greater than or equal to 0.0M");
                }
                _fraction = value;
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            if (Signum == -1)
            {
                builder.Append("-");
            }

            builder.Append("P");
            if (Years != 0)
            {
                builder.Append(Years.ToString());
                builder.Append("Y");
            }
            if (Months != 0)
            {
                builder.Append(Months.ToString());
                builder.Append("M");
            }
            if (Weeks != 0)
            {
                builder.Append(Weeks.ToString());
                builder.Append("W");
            }
            if (Days != 0 || (Years == 0 && Months == 0 && Weeks == 0 && Hours == 0 && Minutes == 0 && Seconds == 0 && FractionOfSecond == 0))
            {
                builder.Append(Days.ToString());
                builder.Append("D");
            }
            if (!(Hours == 0 && Minutes == 0 && Seconds == 0 && FractionOfSecond == 0))
            {
                builder.Append("T");
                if (Hours != 0)
                {
                    builder.Append(Hours.ToString());
                    builder.Append("H");
                }
                if (Minutes != 0)
                {
                    builder.Append(Minutes.ToString());
                    builder.Append("M");
                }
                if (Seconds != 0 || FractionOfSecond != 0M)
                {
                    builder.Append(Seconds.ToString());
                    if (FractionOfSecond != 0)
                    {
                        builder.Append(FractionOfSecond.ToString().Substring(1));
                    }
                    builder.Append("S");
                }
            }
            return builder.ToString();
        }

        public override bool Equals(object obj)
        {
            Iso8601Duration other = obj as Iso8601Duration;
            if (null == other)
            {
                return false;   
            }
            long otherTicks = other.ToTimeSpan().Ticks;
            long ticks = ToTimeSpan().Ticks;
            return otherTicks == ticks;
        }

        public override int GetHashCode()
        {
            return ToTimeSpan().GetHashCode();
        }

        private static Regex _regexDuration;
        private static Regex RegexDuration
        {
            get
            {
                if (null == _regexDuration)
                {
                    _regexDuration = new Regex(@"(?<sign>-)?
P
(?:(?<years>\d+)Y)?
(?:(?<months>\d+)M)?
(?:(?<weeks>\d+)W)?
(?:(?<days>\d+)D)?
(?:
T?
(?:(?<hours>\d+)H)?
(?:(?<minutes>\d+)M)?
(?:(?<seconds>\d+)(?:\.(?<fraction>\d+))?S)?
)", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline);
                }
                return _regexDuration;
            }
        }

        public static Iso8601Duration Parse(string duration)
        {
            MatchCollection matches = RegexDuration.Matches(duration);
            if (!(null != matches && matches.Count > 0))
            {
                return null;
            }

            Debug.Assert(matches.Count == 1);

            Match match = matches[0];
            if (!match.Success)
            {
                return null;
            }

            GroupCollection groups = match.Groups;
            if (!(null != groups && groups.Count > 0))
            {
                return null;
            }

            int signum = 1;
            int years = 0;
            int months = 0;
            int weeks = 0;
            int days = 0;
            int hours = 0;
            int minutes = 0;
            int seconds = 0;
            int fraction = 0;

            Group signGroup = groups["sign"];
            string signValue = signGroup.Value;
            if (!string.IsNullOrEmpty(signValue))
            {
                if (signValue.Equals("-"))
                {
                    signum = -1;
                }
            }

            Group yearsGroup = groups["years"];
            string yearsValue = yearsGroup.Value;
            if (!string.IsNullOrEmpty(yearsValue))
            {
                if (!int.TryParse(yearsValue, out years))
                {
                    throw new ArgumentException("Invalid years value in duration", "duration");
                }
            }

            Group monthsGroup = groups["months"];
            string monthsValue = monthsGroup.Value;
            if (!string.IsNullOrEmpty(monthsValue))
            {
                if (!int.TryParse(monthsValue, out months))
                {
                    throw new ArgumentException("Invalid months value in duration", "duration");
                }
            }

            Group weeksGroup = groups["weeks"];
            string weeksValue = weeksGroup.Value;
            if (!string.IsNullOrEmpty(weeksValue))
            {
                if (!int.TryParse(weeksValue, out weeks))
                {
                    throw new ArgumentException("Invalid weeks value in duration", "duration");
                }
            }

            Group daysGroup = groups["days"];
            string daysValue = daysGroup.Value;
            if (!string.IsNullOrEmpty(daysValue))
            {
                if (!int.TryParse(daysValue, out days))
                {
                    throw new ArgumentException("Invalid days value in duration", "duration");
                }
            }

            Group hoursGroup = groups["hours"];
            string hoursValue = hoursGroup.Value;
            if (!string.IsNullOrEmpty(hoursValue))
            {
                if (!int.TryParse(hoursValue, out hours))
                {
                    throw new ArgumentException("Invalid hours value in duration", "duration");
                }
            }

            Group minutesGroup = groups["minutes"];
            string minutesValue = minutesGroup.Value;
            if (!string.IsNullOrEmpty(minutesValue))
            {
                if (!int.TryParse(minutesValue, out minutes))
                {
                    throw new ArgumentException("Invalid minutes value in duration", "duration");
                }
            }

            Group secondsGroup = groups["seconds"];
            string secondsValue = secondsGroup.Value;
            if (!string.IsNullOrEmpty(secondsValue))
            {
                if (!int.TryParse(secondsValue, out seconds))
                {
                    throw new ArgumentException("Invalid seconds value in duration", "duration");
                }
            }

            Group fractionGroup = groups["fraction"];
            string fractionValue = fractionGroup.Value;
            if (!string.IsNullOrEmpty(fractionValue))
            {
                if (!int.TryParse(fractionValue, out fraction))
                {
                    throw new ArgumentException("Invalid seconds fraction value in duration", "duration");
                }
            }

            Iso8601Duration iso8601Duration = new Iso8601Duration();
            iso8601Duration.Signum = signum;
            iso8601Duration.Years = years;
            iso8601Duration.Months = months;
            iso8601Duration.Weeks = weeks;
            iso8601Duration.Days = days;
            iso8601Duration.Hours = hours;
            iso8601Duration.Minutes = minutes;
            iso8601Duration.Seconds = seconds;
            iso8601Duration.FractionOfSecond = decimal.Parse("0." + fraction.ToString());

            return iso8601Duration;
        }

        public TimeSpan ToTimeSpan()
        {
            decimal days = 0M;
            days += Years * 365.2524M;
            days += Months * 30.4377M;
            days += Weeks * 7M;
            days += Days;
            long ticks = TimeSpan.TicksPerDay * (long)days;
            ticks += TimeSpan.TicksPerHour * Hours;
            ticks += TimeSpan.TicksPerMinute * Minutes;
            ticks += TimeSpan.TicksPerSecond * Seconds;
            ticks += (long)(((decimal)TimeSpan.TicksPerSecond) * FractionOfSecond);
            ticks *= Signum;
            return new TimeSpan(ticks);
        }

        public static TimeSpan ToTimeSpan(Iso8601Duration duration)
        {
            return duration.ToTimeSpan();
        }
    }
}
