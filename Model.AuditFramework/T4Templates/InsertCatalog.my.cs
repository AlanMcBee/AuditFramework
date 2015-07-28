namespace CodeCharm.Model.AuditFramework.T4Templates
{
    partial class InsertCatalog
    {
        private readonly string _schema;
        private readonly string _tableName;
        private readonly int _version;
        private readonly string _auditSchema;
        private readonly string _trackingTableName;

        public InsertCatalog(string schema, string tableName, int version, string auditSchema, string trackingTableName)
        {
            _schema = schema;
            _tableName = tableName;
            _version = version;
            _auditSchema = auditSchema;
            _trackingTableName = trackingTableName;
        }
    }
}
