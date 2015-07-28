using System;
using Microsoft.SqlServer.Management.Common;

namespace CodeCharm.Model.AuditFramework
{
    internal class TrackingTable
    {
        public string Schema { get; set; }
        public string TableName { get; set; }
        public int Version { get; set; }

        public TrackingTable(string schema, string tableName, int version)
        {
            Schema = schema;
            TableName = tableName;
            Version = version;
        }

        public static TrackingTable Parse(string trackingTableName)
        {
            string[] strings = trackingTableName.Split(new[] {'º'}, StringSplitOptions.RemoveEmptyEntries);
            if (strings.Length < 3)
            {
                throw new InvalidArgumentException("Must be delimited with º character in two places");
            }
            TrackingTable trackingTable = new TrackingTable(strings[0], strings[1], Convert.ToInt32(strings[2]));
            return trackingTable;
        }

        public override string ToString()
        {
            return string.Format("{0}º{1}º{2:X8}", Schema, TableName.Substring(0, Math.Min(TableName.Length, 118 - Schema.Length)), Version);
        }
    }
}
