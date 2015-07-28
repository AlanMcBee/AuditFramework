using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeCharm.Entity.AuditFramework;

namespace CodeCharm.Model.AuditFramework.T4Templates
{
    partial class CreateTrackingInsertProcedure
    {
        private readonly string _schema;
        private readonly string _auditedTable;
        private readonly string _trackingTableName;
        private readonly string _deltaTableName;
        private readonly List<SchemaDataSet.ColumnsRow> _targetTableColumns;
        private readonly AuditUpdateParameters _params;

        public CreateTrackingInsertProcedure(string schema, string auditedTable, string trackingTableName, string deltaTableName, List<SchemaDataSet.ColumnsRow> targetTableColumns, AuditUpdateParameters updateParameters)
        {
            _schema = schema;
            _auditedTable = auditedTable;
            _trackingTableName = trackingTableName;
            _deltaTableName = deltaTableName;
            _targetTableColumns = targetTableColumns;
            _params = updateParameters;
        }
    }
}
