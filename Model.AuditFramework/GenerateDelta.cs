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
    internal static class GenerateDelta
    {
        internal static void CreateDeltaProcedures(string schema, string tableName, TrackingTable trackingTable, List<SchemaDataSet.ColumnsRow> targetTableColumns, StringBuilder sqlBuilder, AuditUpdateParameters updateParameters, Action<string, float?> reportProgress)
        {
            GenerateBuildDeltaProcedures(schema, tableName, trackingTable, targetTableColumns, sqlBuilder, updateParameters);
            reportProgress(string.Format("Generated BuildDelta procedure for {0}º{1}", schema, tableName), null);
        }

        private static void GenerateBuildDeltaProcedures(string schema, string tableName, TrackingTable trackingTable, List<SchemaDataSet.ColumnsRow> targetTableColumns, StringBuilder sqlBuilder, AuditUpdateParameters updateParameters)
        {
            CreateUpdateDeltaTableProcedure createUpdateDeltaTableProcedureTemplate = new CreateUpdateDeltaTableProcedure(schema, tableName, trackingTable, targetTableColumns, updateParameters);
            string sql = createUpdateDeltaTableProcedureTemplate.TransformText();
            sqlBuilder.AppendLine(sql);

            SchemaDataSet schemaDataSet = updateParameters.SchemaDataSet;
            if (schemaDataSet.SchemaTables.TableExists(updateParameters.AuditingSchema, "Catalog"))
            {
                EntityConnectionStringBuilder connectionStringBuilder = Generator.GetEntityConnectionStringBuilder(updateParameters);
                AuditFrameworkEntities context = new AuditFrameworkEntities(connectionStringBuilder.ToString());
                IOrderedQueryable<Catalog> priorVersionTrackingTableQuery = from catalog in context.Catalogs
                                                                            where catalog.AuditedSchema.Equals(schema, StringComparison.OrdinalIgnoreCase)
                                                                            where catalog.AuditedTableName.Equals(tableName, StringComparison.OrdinalIgnoreCase)
                                                                            where catalog.Version < trackingTable.Version
                                                                            where !catalog.Archived
                                                                            orderby catalog.Version descending
                                                                            select catalog;
                foreach (Catalog catalog in priorVersionTrackingTableQuery)
                {
                    TrackingTable priorTrackingTable = TrackingTable.Parse(catalog.AuditingTableName);
                    CreateUpdateDeltaPriorTableProcedure createUpdateDeltaPriorTableProcedureTemplate =new CreateUpdateDeltaPriorTableProcedure(schema, tableName, trackingTable, priorTrackingTable, targetTableColumns, updateParameters);
                    sql = createUpdateDeltaPriorTableProcedureTemplate.TransformText();
                    sqlBuilder.AppendLine(sql);
                }
            }

        }

        internal static void GenerateBuildAllDeltaProcedures(StringBuilder sqlBuilder, AuditUpdateParameters updateParameters, Action<string, float?> reportProgress)
        {
            CreateBuildDeltaProcedure createBuildDeltaProcedureTemplate = new CreateBuildDeltaProcedure(updateParameters);
            string sql = createBuildDeltaProcedureTemplate.TransformText();
            sqlBuilder.AppendLine(sql);
        }

    }
}
