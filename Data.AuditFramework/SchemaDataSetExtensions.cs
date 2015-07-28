using System;
using System.Data;
using System.Linq;
using CodeCharm.Entity.AuditFramework;

namespace CodeCharm.Data.AuditFramework
{
    public static class SchemaDataSetExtensions
    {
        public static bool TableExists(this SchemaDataSet.SchemaTablesDataTable schemaTablesDataTable, string schema, string tableName)
        {
            return schemaTablesDataTable.Any(row => row.TABLE_SCHEMA.Equals(schema, StringComparison.OrdinalIgnoreCase)
                && row.TABLE_NAME.Equals(tableName, StringComparison.OrdinalIgnoreCase)
                && row.TABLE_TYPE.Equals("BASE TABLE", StringComparison.Ordinal));
        }

        

        public static bool ProcedureExists(this SchemaDataSet.ProceduresDataTable proceduresDataTable, string schema, string procedureName)
        {
            return proceduresDataTable.Any(row => row.ROUTINE_SCHEMA.Equals(schema, StringComparison.OrdinalIgnoreCase)
                && row.ROUTINE_NAME.Equals(procedureName, StringComparison.OrdinalIgnoreCase)
                );
        }

        public static void FillPrimaryKeysBySchema(this SchemaDataSet.SchemaPrimaryKeysDataTable primaryKeysDataTable, string schema, IDbConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            const string SELECT_PRIMARY_KEYS = @"
SELECT        cons.TABLE_SCHEMA
            , cons.TABLE_NAME
            , cols.COLUMN_NAME
            , 0 AS IsMatch
    FROM            INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS cons 
    LEFT OUTER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS cols 
                ON  cons.CONSTRAINT_NAME = cols.CONSTRAINT_NAME
    WHERE   (cons.CONSTRAINT_TYPE = 'PRIMARY KEY') 
        AND (cons.TABLE_SCHEMA = @tableSchema)
    ORDER BY  cons.TABLE_SCHEMA
            , cons.TABLE_NAME
            , cols.COLUMN_NAME";

            primaryKeysDataTable.Clear();
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = SELECT_PRIMARY_KEYS;

                IDbDataParameter parameter = command.CreateParameter();
                command.Parameters.Add(parameter);

                parameter.DbType = DbType.String;
                parameter.Direction = ParameterDirection.Input;
                parameter.ParameterName = "@tableSchema";
                parameter.Value = schema;

                ConnectionState state = connection.State;
                bool startedClosed = state == ConnectionState.Closed;
                try
                {
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        primaryKeysDataTable.BeginLoadData();
                        primaryKeysDataTable.Load(reader);
                        primaryKeysDataTable.EndLoadData();
                    }
                }
                finally
                {
                    if (startedClosed)
                    {
                        if (connection.State
                            != ConnectionState.Closed)
                        {
                            connection.Close();
                        }
                    }
                }
            }
        }
    }
}
