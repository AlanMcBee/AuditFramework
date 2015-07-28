using CodeCharm.Entity.AuditFramework;

namespace CodeCharm.Model.AuditFramework.T4Templates
{
    partial class CreateInstantTrackIdProcedure
    {
        private readonly string _schema;
        private readonly string _targetTableName;
        private readonly AuditUpdateParameters _params;

        public CreateInstantTrackIdProcedure(string schema, string targetTableName, AuditUpdateParameters updateParameters)
        {
            _schema = schema;
            _targetTableName = targetTableName;
            _params = updateParameters;
        }
    }
}
