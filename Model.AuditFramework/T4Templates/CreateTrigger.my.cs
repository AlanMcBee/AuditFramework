using CodeCharm.Entity.AuditFramework;

namespace CodeCharm.Model.AuditFramework.T4Templates
{
    partial class CreateTrigger
    {
        private readonly string _schema;
        private readonly string _tableName;
        private readonly AuditUpdateParameters _params;

        public CreateTrigger(string schema, string tableName, AuditUpdateParameters updateParameters)
        {
            _schema = schema;
            _tableName = tableName;
            _params = updateParameters;
        }

    }
}
