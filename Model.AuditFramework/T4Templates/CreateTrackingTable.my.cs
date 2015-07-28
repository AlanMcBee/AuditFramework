using System.Collections.Generic;
using CodeCharm.Entity.AuditFramework;

namespace CodeCharm.Model.AuditFramework.T4Templates
{
    partial class CreateTrackingTable
    {
        private readonly string _schema;
        private readonly string _tableName;
        private readonly string _trackingTableName;
        private readonly List<SchemaDataSet.ColumnsRow> _targetTableColumns;
        private readonly AuditUpdateParameters _params;

        public CreateTrackingTable(string schema, string tableName, string trackingTableName, List<SchemaDataSet.ColumnsRow> targetTableColumns, AuditUpdateParameters updateParameters)
        {
            _schema = schema;
            _tableName = tableName;
            _trackingTableName = trackingTableName;
            _targetTableColumns = targetTableColumns;
            _params = updateParameters;
        }

    }
}
