using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace CodeCharm.Utility
{
    public static class SystemXmlHelper
    {
        public static TimeSpan XmlSchemaDurationToTimeSpan(string duration)
        {
            // return SoapDuration.Parse(duration);
            //
            // This would be the .NET built-in helper
            // but I examined the code using Reflector and
            // I don't like it as much as mine.
            // For what it's worth, the SoapDuration Parse method
            // converts years to 360 days, 
            // months to 30 days, and ignores weeks

            Iso8601Duration iso8601Duration = Iso8601Duration.Parse(duration);
            return iso8601Duration.ToTimeSpan();
        }
    }
}
