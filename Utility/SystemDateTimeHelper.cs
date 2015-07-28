using System;
using System.Collections.Generic;
using System.Text;

namespace CodeCharm.Utility
{
    public class SystemDateTimeHelper
    {
        public class Standard
        {
            public const string ShortDate = "d";
            public const string LongDate = "D";
            public const string FullStandardDateTime = "f";
            public const string FullCustomDateTime = "F";
            public const string GeneralDateShortTime = "g";
            public const string GeneralDateLongTime = "G";
            public const string MonthDay = "M";
            public const string RoundTripDateTime = "o";
            public const string Rfc1123 = "R";
            public const string SortableIso8601DateTime = "s";
            public const string ShortTimePattern = "t";
            public const string LongTimePattern = "T";
            public const string UniversalLocalSortableDateTime = "u";
            public const string UniversalUtcSortableDateTime = "U";
            public const string YearMonth = "Y";
        }

        public class Custom
        {
            public const string DayOfMonthNoLeadZero = "d";
            public const string DayOfMonthLeadingZero = "dd";
            public const string DayOfWeekAbbreviation = "ddd";
            public const string DayOfWeekFull = "dddd";
            public const string SecondFraction1DigitTrailingZero = "f";
            public const string SecondFraction2DigitTrailingZero = "ff";
            public const string SecondFraction3DigitTrailingZero = "fff";
            public const string SecondFraction4DigitTrailingZero = "ffff";
            public const string SecondFraction5DigitTrailingZero = "fffff";
            public const string SecondFraction6DigitTrailingZero = "ffffff";
            public const string SecondFraction7DigitTrailingZero = "fffffff";
            public const string SecondFraction1DigitNoTrail = "F";
            public const string SecondFraction2DigitNoTrail = "FF";
            public const string SecondFraction3DigitNoTrail = "FFF";
            public const string SecondFraction4DigitNoTrail = "FFFF";
            public const string SecondFraction5DigitNoTrail = "FFFFF";
            public const string SecondFraction6DigitNoTrail = "FFFFFF";
            public const string SecondFraction7DigitNoTrail = "FFFFFFF";
            public const string Era = "gg";
            public const string Hour12NoLeadZero = "h";
            public const string Hour12LeadingZero = "hh";
            public const string Hour24NoLeadZero = "H";
            public const string Hour24LeadingZero = "HH";
            public const string Kind = "K";
            public const string MinuteNoLeadZero = "m";
            public const string MinuteLeadingZero = "mm";
            public const string MonthNumberNoLeadZero = "M";
            public const string MonthNumberLeadingZero = "MM";
            public const string MonthAbbreviation = "MMM";
            public const string MonthFull = "MMMM";
            public const string SecondNoLeadZero = "s";
            public const string SecondLeadingZero = "ss";
            public const string MeridiemDesignatorFirstLetter = "t";
            public const string MeridiemDesignatorAbbreviation = "tt";
            public const string YearNoLeadZero = "y";
            public const string Year2Digit = "yy";
            public const string Year3Digit = "yyy";
            public const string Year4Digit = "yyyy";
            public const string Year5Digit = "yyyyy";
            public const string UtcOffsetHourNoLeadZero = "z";
            public const string UtcOffsetHourLeadingZero = "zz";
            public const string UtcOffsetHourMinuteLeadingZero = "zzz";
            public const string TimeSeparator = ":";
            public const string DateSeparator = "/";
            public const string SingleFormatPrefix = "%";
        }
    }
}
