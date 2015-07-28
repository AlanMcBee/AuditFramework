using System.Collections.Generic;
using CodeCharm.Entity.AuditFramework;

namespace CodeCharm.Model.AuditFramework.T4Templates
{
    partial class SetBaseline
    {
        private readonly string _schema;
        private readonly string _tableName;
        private readonly TrackingTable _trackingTable;
        private readonly List<SchemaDataSet.ColumnsRow> _targetTableColumns;
        private readonly AuditUpdateParameters _params;

        internal SetBaseline(string schema, string tableName, TrackingTable trackingTable, List<SchemaDataSet.ColumnsRow> targetTableColumns, AuditUpdateParameters updateParameters)
        {
            _schema = schema;
            _tableName = tableName;
            _trackingTable = trackingTable;
            _targetTableColumns = targetTableColumns;
            _params = updateParameters;
        }
    }
}
