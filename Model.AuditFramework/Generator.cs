using System;
using System.ComponentModel;
using System.Data;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using CodeCharm.Entity.AuditFramework;

namespace CodeCharm.Model.AuditFramework
{
    public class Generator
    {
        #region ReportProgress Event

        [Category("Action")]
        public event EventHandler<ReportProgressEventArgs> ReportProgress;

        protected virtual void OnReportProgress(ReportProgressEventArgs e)
        {
            EventHandler<ReportProgressEventArgs> handler = ReportProgress;
            if (null != handler)
            {
                handler(this, e);
            }
        }

        protected void OnReportProgress(string message, float? percentComplete)
        {
            ReportProgressEventArgs e = new ReportProgressEventArgs(message, percentComplete);
            OnReportProgress(e);
        }

        #endregion

        public string GenerateAuditingFrameworkUpdateSql(AuditUpdateParameters updateParameters)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            Action<string, float?> reportProgress = (message, percentComplete) =>
                                                        {
                                                            sqlBuilder.AppendLine();
                                                            sqlBuilder.AppendFormat("PRINT(N'{0}');", message);
                                                            sqlBuilder.AppendLine();
                                                            sqlBuilder.AppendLine("GO");

                                                            OnReportProgress(message, percentComplete);
                                                        };

            SqlConnection connection = updateParameters.AuditingDatabaseConnection;
            try
            {
                OnReportProgress("Started generating script", null);
                connection.Open();

                if (updateParameters.RemoveAll)
                {
                    GenerateRemoveAll(sqlBuilder, updateParameters);
                }
                else
                {
                    LoadSchemaDataSet(updateParameters);

                    GenerateDatabaseFiles.GenerateDatabaseSetup(sqlBuilder, updateParameters, reportProgress);
                    GenerateAuditingBase.CreateBaseAuditingTables(sqlBuilder, updateParameters, reportProgress);
                    GenerateAuditingBase.CreateBaseAuditingProcedures(sqlBuilder, updateParameters, reportProgress);
                    GenerateAuditingBase.CreateProcedureGrant(sqlBuilder, updateParameters, reportProgress);

                    GenerateSchemaAudit.GenerateAuditForAllSchemas(sqlBuilder, updateParameters, reportProgress);
                    GenerateDelta.GenerateBuildAllDeltaProcedures(sqlBuilder, updateParameters, reportProgress);
                }
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return sqlBuilder.ToString();
        }

        private static void GenerateRemoveAll(StringBuilder sqlBuilder, AuditUpdateParameters updateParameters)
        {
            throw new NotImplementedException();
        }

        private static void LoadSchemaDataSet(AuditUpdateParameters updateParameters)
        {
            SqlConnection connection = updateParameters.AuditingDatabaseConnection;
            SchemaDataSet schemaDataSet = updateParameters.SchemaDataSet;
            DataTable metaDataCollectionsTable = connection.GetSchema();
            schemaDataSet.MetaDataCollections.BeginLoadData();
            schemaDataSet.MetaDataCollections.Load(metaDataCollectionsTable.CreateDataReader());
            schemaDataSet.MetaDataCollections.EndLoadData();

            foreach (SchemaDataSet.MetaDataCollectionsRow metaDataCollectionsRow in schemaDataSet.MetaDataCollections)
            {
                try
                {
                    DataTable metaDataTable = connection.GetSchema(metaDataCollectionsRow.CollectionName);
                    string collectionName = metaDataCollectionsRow.CollectionName;
                    if (collectionName.Equals("MetaDataCollections", StringComparison.OrdinalIgnoreCase))
                        continue;

                    DataTable schemaTable = collectionName.Equals("Tables", StringComparison.OrdinalIgnoreCase)
                                                ? schemaDataSet.SchemaTables
                                                : schemaDataSet.Tables[collectionName];
                    schemaTable.BeginLoadData();
                    schemaTable.Load(metaDataTable.CreateDataReader());
                    schemaTable.EndLoadData();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Skipping {0}", metaDataCollectionsRow.CollectionName);
                }
            }

        }

        internal static EntityConnectionStringBuilder GetEntityConnectionStringBuilder(AuditUpdateParameters updateParameters)
        {
            return new EntityConnectionStringBuilder
                       {
                           ProviderConnectionString = updateParameters.AuditingDatabaseConnection.ConnectionString,
                           Provider = "System.Data.SqlClient",
                           Metadata = @"res://*/AuditFrameworkEntityDataModel.csdl|
                            res://*/AuditFrameworkEntityDataModel.ssdl|
                            res://*/AuditFrameworkEntityDataModel.msl"
                       };
        }
    }
}