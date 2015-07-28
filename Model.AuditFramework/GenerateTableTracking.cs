using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Linq;
using System.Text;
using CodeCharm.Data.AuditFramework;
using CodeCharm.Entity.AuditFramework;
using CodeCharm.Model.AuditFramework.T4Templates;

namespace CodeCharm.Model.AuditFramework
{
    internal static class GenerateTableTracking
    {
        internal static TrackingTable GenerateNewTrackingTable(string schema, string tableName, List<SchemaDataSet.ColumnsRow> targetTableColumns, AuditUpdateParameters updateParameters, StringBuilder sqlBuilder, Action<string, float?> reportProgress)
        {
            int version;
            SchemaDataSet schemaDataSet = updateParameters.SchemaDataSet;
            if (schemaDataSet.SchemaTables.TableExists(updateParameters.AuditingSchema, "Catalog"))
            {
                EntityConnectionStringBuilder connectionStringBuilder = Generator.GetEntityConnectionStringBuilder(updateParameters);
                AuditFrameworkEntities context = new AuditFrameworkEntities(connectionStringBuilder.ToString());
                var catalogQuery = from catalog in context.Catalogs
                                   where catalog.AuditedSchema.Equals(schema, StringComparison.OrdinalIgnoreCase)
                                         && catalog.AuditedTableName.Equals(tableName, StringComparison.OrdinalIgnoreCase)
                                   orderby catalog.Version descending
                                   select catalog;
                var latestCatalog = catalogQuery.FirstOrDefault();
                if (null == latestCatalog)
                {
                    version = 0;
                }
                else
                {
                    version = latestCatalog.Version + 1;
                }
            }
            else
            {
                version = 0;
            }
            TrackingTable trackingTable = new TrackingTable(schema, tableName, version);
            string trackingTableName = trackingTable.ToString();

            InsertCatalog insertCatalogTemplate = new InsertCatalog(schema, tableName, version, updateParameters.AuditingSchema, trackingTableName);
            string sql = insertCatalogTemplate.TransformText();
            sqlBuilder.AppendLine(sql);

            CreateTrackingTable createTrackingTableTemplate = new CreateTrackingTable(schema, tableName, trackingTableName, targetTableColumns, updateParameters);
            sql = createTrackingTableTemplate.TransformText();
            sqlBuilder.AppendLine(sql);

            return trackingTable;
        }

        internal static void GenerateAuditSupportForTable(string schema, string tableName, TrackingTable trackingTable, StringBuilder sqlBuilder, List<SchemaDataSet.ColumnsRow> targetTableColumns, AuditUpdateParameters updateParameters, Action<string, float?> reportProgress)
        {
            string trackingTableName = trackingTable.ToString();
            string deltaTableName = trackingTableName + "Δ";

            CreateDeltaTable createDeltaTableTemplate = new CreateDeltaTable(trackingTableName + "Δ", targetTableColumns, updateParameters);
            string sql = createDeltaTableTemplate.TransformText();
            sqlBuilder.AppendLine(sql);

            GenerateAuditTableInsertProcedures(schema, tableName, trackingTableName, deltaTableName, targetTableColumns, sqlBuilder, updateParameters, reportProgress);
            if (updateParameters.GenerateBaseline)
            {
                Baseline(schema, tableName, trackingTable, targetTableColumns, sqlBuilder, updateParameters, reportProgress);
            }
            GenerateAuditTableTriggers(schema, tableName, sqlBuilder, updateParameters, reportProgress);
        }

        internal static TrackingTable GetCurrentTrackingTable(string schema, string tableName, AuditUpdateParameters updateParameters)
        {
            SchemaDataSet schemaDataSet = updateParameters.SchemaDataSet;
            if (schemaDataSet.SchemaTables.TableExists(updateParameters.AuditingSchema, "Catalog"))
            {
                EntityConnectionStringBuilder connectionStringBuilder = Generator.GetEntityConnectionStringBuilder(updateParameters);
                AuditFrameworkEntities context = new AuditFrameworkEntities(connectionStringBuilder.ToString());
                var catalogQuery = from catalog in context.Catalogs
                                   where catalog.AuditedSchema.Equals(schema, StringComparison.OrdinalIgnoreCase)
                                         && catalog.AuditedTableName.Equals(tableName, StringComparison.OrdinalIgnoreCase)
                                   orderby catalog.Version descending
                                   select catalog;
                Catalog catalogRow = catalogQuery.FirstOrDefault();
                if (null == catalogRow)
                {
                    return null;
                }
                TrackingTable trackingTable = TrackingTable.Parse(catalogRow.AuditingTableName);
                return trackingTable;
            }
            return null;
        }

        private static void Baseline(string schema, string tableName, TrackingTable trackingTable, List<SchemaDataSet.ColumnsRow> targetTableColumns, StringBuilder sqlBuilder, AuditUpdateParameters updateParameters, Action<string, float?> reportProgress)
        {
            reportProgress(string.Format("Starting to set baseline records for {0}.{1}", schema, tableName), null);
            SetBaseline performBaseLineTemplate = new SetBaseline(schema, tableName, trackingTable, targetTableColumns, updateParameters);
            string sql = performBaseLineTemplate.TransformText();
            sqlBuilder.AppendLine(sql);
            reportProgress(string.Format("Finished setting baseline records for {0}.{1}", schema, tableName), null);
        }

        private static void GenerateAuditTableInsertProcedures(string schema, string tableName, string trackingTableName, string deltaTableName, List<SchemaDataSet.ColumnsRow> targetTableColumns, StringBuilder sqlBuilder, AuditUpdateParameters updateParameters, Action<string, float?> reportProgress)
        {
            reportProgress(string.Format("Generating TrackingInsert Procedure for {0}.{1}", schema, tableName), null);
            CreateTrackingInsertProcedure createTrackingInsertProcedureTemplate = new CreateTrackingInsertProcedure(schema, tableName, trackingTableName, deltaTableName, targetTableColumns, updateParameters);
            string sql = createTrackingInsertProcedureTemplate.TransformText();
            sqlBuilder.AppendLine(sql);
            reportProgress(string.Format("Generated TrackingInsert Procedure for {0}.{1}", schema, tableName), null);
        }

        private static void GenerateAuditTableTriggers(string schema, string tableName, StringBuilder sqlBuilder, AuditUpdateParameters updateParameters, Action<string, float?> reportProgress)
        {
            reportProgress(string.Format("Generating Triggers for {0}.{1}", schema, tableName), null);

            CreateTrigger triggerTemplate = new CreateTrigger(schema, tableName, updateParameters);
            string sql = triggerTemplate.TransformText();
            sqlBuilder.AppendLine(sql);

            reportProgress(string.Format("Generated Triggers for {0}.{1}", schema, tableName), null);
        }

        public static void GenerateInstantSupportForTable(string schema, string tableName, StringBuilder sqlBuilder, AuditUpdateParameters updateParameters)
        {
            CreateInstantTrackIdProcedure createInstantTrackIdProcedureTemplate = new CreateInstantTrackIdProcedure(schema, tableName, updateParameters);
            string sql = createInstantTrackIdProcedureTemplate.TransformText();
            sqlBuilder.AppendLine(sql);
        }
    }
}
