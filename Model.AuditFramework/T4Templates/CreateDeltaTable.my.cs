using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeCharm.Entity.AuditFramework;

namespace CodeCharm.Model.AuditFramework.T4Templates
{
    partial class CreateDeltaTable
    {
        private readonly string _deltaTableName;
        private readonly List<SchemaDataSet.ColumnsRow> _targetTableColumns;
        private readonly AuditUpdateParameters _params;

        public CreateDeltaTable(string deltaTableName, List<SchemaDataSet.ColumnsRow> targetTableColumns, AuditUpdateParameters updateParameters)
        {
            _deltaTableName = deltaTableName;
            _targetTableColumns = targetTableColumns;
            _params = updateParameters;
        }
    }
}
