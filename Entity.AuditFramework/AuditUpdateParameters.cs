using System.Collections.Specialized;
using System.Data.SqlClient;
using System.IO;

namespace CodeCharm.Entity.AuditFramework
{
    public class AuditUpdateParameters
    {
        public AuditUpdateParameters()
        {
            SchemasToAudit = new StringCollection();
            TablesNotAudited = new StringCollection();
            ColumnsNotAudited = new StringCollection();
            ExecuteGrantees = new StringCollection();
            SchemaDataSet = new SchemaDataSet();
        }

        public string AuditingSchema
        {
            get;
            set;
        }
        public string AuditTableFormat
        {
            get;
            set;
        }
        public bool AlwaysRecreateFramework
        {
            get;
            set;
        }
        public string AuditingFileGroup
        {
            get;
            set;
        }
        public DirectoryInfo SqlFilePath
        {
            get;
            set;
        }
        public bool RemoveAll
        {
            get;
            set;
        }
        public bool AutoDeployToDatabase
        {
            get;
            set;
        }
        public DirectoryInfo OutputPath
        {
            get;
            set;
        }
        public string DatabaseName
        {
            get;
            set;
        }
        public bool TrackByPrimaryKeys
        {
            get;
            set;
        }
        public StringCollection SchemasToAudit
        {
            get;
            private set;
        }
        public StringCollection TablesNotAudited
        {
            get;
            private set;
        }
        public StringCollection ColumnsNotAudited
        {
            get;
            private set;
        }
        public StringCollection ExecuteGrantees
        {
            get;
            private set;
        }
        public SqlConnection AuditingDatabaseConnection
        {
            get;
            set;
        }
        public SchemaDataSet SchemaDataSet
        {
            get;
            private set;
        }

        public bool GenerateBaseline
        {
            get;
            set;
        }
    }
}
