using System;
using System.Data;
using System.Linq;
using System.Text;
using CodeCharm.Data.AuditFramework;
using CodeCharm.Entity.AuditFramework;

namespace CodeCharm.Model.AuditFramework
{
    internal static class GenerateSchemaAudit
    {
        internal static void GenerateAuditForAllSchemas(StringBuilder sqlBuilder, AuditUpdateParameters updateParameters, Action<string, float?> reportProgress)
        {
            foreach (string schema in updateParameters.SchemasToAudit)
            {
                GenerateAuditForSchema(schema, sqlBuilder, updateParameters, reportProgress);
            }
        }

        private static void GenerateAuditForSchema(string schema, StringBuilder sqlBuilder, AuditUpdateParameters updateParameters, Action<string, float?> reportProgress)
        {
            reportProgress(string.Format("Generating audit for schema {0}", schema), null);

            SchemaDataSet schemaDataSet = updateParameters.SchemaDataSet;
            SchemaDataSet.SchemaPrimaryKeysDataTable primaryKeysDataTable = schemaDataSet.SchemaPrimaryKeys;
            primaryKeysDataTable.FillPrimaryKeysBySchema(schema, updateParameters.AuditingDatabaseConnection);

            SchemaDataSet.SchemaTablesDataTable schemaTables = schemaDataSet.SchemaTables;
            var schemaTablesRows = schemaTables.AsEnumerable();
            var schemaTablesQuery = from schemaTableRow in schemaTablesRows
                                    where schemaTableRow.TABLE_SCHEMA.Equals(schema)
                                    where schemaTableRow.TABLE_TYPE.Equals("BASE TABLE")
                                    orderby schemaTableRow.TABLE_NAME
                                    select schemaTableRow;

            foreach (SchemaDataSet.SchemaTablesRow schemaTablesRow in schemaTablesQuery)
            {
                string tableName = schemaTablesRow.TABLE_NAME;
                if (!updateParameters.TablesNotAudited.Cast<string>().Any(name => name.Equals(tableName, StringComparison.OrdinalIgnoreCase)))
                {
                    GenerateTableAudit.GenerateAuditForTable(schema, tableName, primaryKeysDataTable, sqlBuilder, updateParameters, reportProgress);
                }
            }

            reportProgress(string.Format("Generated audit for schema {0}", schema), null);
        }

    }
}