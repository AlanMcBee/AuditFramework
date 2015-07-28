using System.Collections.Generic;
using CodeCharm.Entity.AuditFramework;

namespace CodeCharm.Model.AuditFramework.T4Templates
{
    partial class CreateUpdateDeltaPriorTableProcedure
    {
        private readonly string _schema;
        private readonly string _tableName;
        private readonly TrackingTable _currentTrackingTable;
        private readonly TrackingTable _priorTrackingTable;
        private readonly List<SchemaDataSet.ColumnsRow> _targetTableColumns;
        private readonly AuditUpdateParameters _params;

        internal CreateUpdateDeltaPriorTableProcedure(string schema, string tableName, TrackingTable currentTrackingTable, TrackingTable priorTrackingTable, List<SchemaDataSet.ColumnsRow> targetTableColumns, AuditUpdateParameters updateParameters)
        {
            _schema = schema;
            _tableName = tableName;
            _currentTrackingTable = currentTrackingTable;
            _priorTrackingTable = priorTrackingTable;
            _targetTableColumns = targetTableColumns;
            _params = updateParameters;
        }
    }
}
