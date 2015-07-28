using System;
using System.Text;
using CodeCharm.Data.AuditFramework;
using CodeCharm.Entity.AuditFramework;
using CodeCharm.Model.AuditFramework.T4Templates;

namespace CodeCharm.Model.AuditFramework
{
    internal static class GenerateAuditingBase
    {
        internal static void CreateBaseAuditingTables(StringBuilder sqlBuilder, AuditUpdateParameters updateParameters, Action<string, float?> reportProgress)
        {
            GenerateCatalogTable(sqlBuilder, updateParameters);
            reportProgress("Generated Catalog table", null);
            GenerateActionCodesTable(sqlBuilder, updateParameters);
            reportProgress("Generated ActionCodes table", null);
            GenerateTransactionSequenceTable(sqlBuilder, updateParameters);
            reportProgress("Generated TransactionSequence table", null);
            GenerateSessionTransactionTable(sqlBuilder, updateParameters);
            reportProgress("Generated SessionTransaction table", null);
            GenerateAuditMasterTable(sqlBuilder, updateParameters);
            reportProgress("Generated AuditMaster table", null);
        }

        private static void GenerateCatalogTable(StringBuilder sqlBuilder, AuditUpdateParameters updateParameters)
        {
            SchemaDataSet schemaDataSet = updateParameters.SchemaDataSet;
            if (!schemaDataSet.SchemaTables.TableExists(updateParameters.AuditingSchema, "Catalog"))
            {
                CreateCatalogTable createCatalogTableTemplate = new CreateCatalogTable(updateParameters);
                string sql = createCatalogTableTemplate.TransformText();
                sqlBuilder.AppendLine(sql);
            }
        }

        private static void GenerateActionCodesTable(StringBuilder sqlBuilder, AuditUpdateParameters updateParameters)
        {
            SchemaDataSet schemaDataSet = updateParameters.SchemaDataSet;
            if (!schemaDataSet.SchemaTables.TableExists(updateParameters.AuditingSchema, "ActionCodes"))
            {
                CreateActionCodesTable createActionCodesTableTemplate = new CreateActionCodesTable(updateParameters);
                string sql = createActionCodesTableTemplate.TransformText();
                sqlBuilder.AppendLine(sql);
            }
        }

        private static void GenerateTransactionSequenceTable(StringBuilder sqlBuilder, AuditUpdateParameters updateParameters)
        {
            SchemaDataSet schemaDataSet = updateParameters.SchemaDataSet;
            if (!schemaDataSet.SchemaTables.TableExists(updateParameters.AuditingSchema, "TransactionSequence"))
            {
                CreateTransactionSequenceTable createTransactionSequenceTable = new CreateTransactionSequenceTable(updateParameters);
                string sql = createTransactionSequenceTable.TransformText();
                sqlBuilder.AppendLine(sql);
            }
        }

        private static void GenerateSessionTransactionTable(StringBuilder sqlBuilder, AuditUpdateParameters updateParameters)
        {
            SchemaDataSet schemaDataSet = updateParameters.SchemaDataSet;
            if (!schemaDataSet.SchemaTables.TableExists(updateParameters.AuditingSchema, "SessionTransaction"))
            {
                CreateSessionTransactionTable createSessionTransactionTable = new CreateSessionTransactionTable(updateParameters);
                string sql = createSessionTransactionTable.TransformText();
                sqlBuilder.AppendLine(sql);
            }
        }

        private static void GenerateAuditMasterTable(StringBuilder sqlBuilder, AuditUpdateParameters updateParameters)
        {
            SchemaDataSet schemaDataSet = updateParameters.SchemaDataSet;
            if (!schemaDataSet.SchemaTables.TableExists(updateParameters.AuditingSchema, "AuditMaster"))
            {
                CreateAuditMasterTable createAuditMasterTableTemplate = new CreateAuditMasterTable(updateParameters);
                string sql = createAuditMasterTableTemplate.TransformText();
                sqlBuilder.AppendLine(sql);
            }
        }

        internal static void CreateBaseAuditingProcedures(StringBuilder sqlBuilder, AuditUpdateParameters updateParameters, Action<string, float?> reportProgress)
        {
            GenerateRequestTransactionSequenceProcedure(sqlBuilder, updateParameters);
            GenerateCurrentTransactionSequenceProcedure(sqlBuilder, updateParameters);
            GenerateReleaseTransactionSequenceProcedure(sqlBuilder, updateParameters);
            reportProgress("Generated TransactionSequence procedures", null);
        }

        private static void GenerateRequestTransactionSequenceProcedure(StringBuilder sqlBuilder, AuditUpdateParameters updateParameters)
        {
            CreateRequestTransactionSequenceProcedure createRequestTransactionSequenceProcedure = new CreateRequestTransactionSequenceProcedure(updateParameters);
            string sql = createRequestTransactionSequenceProcedure.TransformText();
            sqlBuilder.AppendLine(sql);
        }

        private static void GenerateCurrentTransactionSequenceProcedure(StringBuilder sqlBuilder, AuditUpdateParameters updateParameters)
        {
            CreateCurrentTransactionSequenceProcedure createCurrentTransactionSequenceProcedure = new CreateCurrentTransactionSequenceProcedure(updateParameters);
            string sql = createCurrentTransactionSequenceProcedure.TransformText();
            sqlBuilder.AppendLine(sql);
        }

        private static void GenerateReleaseTransactionSequenceProcedure(StringBuilder sqlBuilder, AuditUpdateParameters updateParameters)
        {
            CreateReleaseTransactionSequenceProcedure createReleaseTransactionSequenceProcedure = new CreateReleaseTransactionSequenceProcedure(updateParameters);
            string sql = createReleaseTransactionSequenceProcedure.TransformText();
            sqlBuilder.AppendLine(sql);
        }

        internal static void CreateProcedureGrant(StringBuilder sqlBuilder, AuditUpdateParameters updateParameters, Action<string, float?> reportProgress)
        {
            GrantExecuteToTransactionSequenceProcedures grantExecuteToTransactionSequenceProcedures = new GrantExecuteToTransactionSequenceProcedures(updateParameters);
            string sql = grantExecuteToTransactionSequenceProcedures.TransformText();
            sqlBuilder.AppendLine(sql);
        }

    }
}
