using System;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using CodeCharm.Entity.AuditFramework;

namespace CodeCharm.Model.AuditFramework
{
    internal static class SchemaHelper
    {
        private static readonly StringCollection c_fixedSizeDataTypes = new StringCollection
                                                                    {
                                                                        "int",
                                                                        "bit",
                                                                        "date",
                                                                        "datetime",
                                                                        "bigint",
                                                                        "smallint",
                                                                        "tinyint",
                                                                        "money",
                                                                        "smallmoney",
                                                                        "smalldatetime",
                                                                        "text",
                                                                        "image",
                                                                        "ntext",
                                                                        "rowversion",
                                                                        "timestamp",
                                                                        "uniqueidentifier"
                                                                    };
        internal static string DataTypeDetail(SchemaDataSet.ColumnsRow columnsRow)
        {
            StringBuilder dataTypeStringBuilder = new StringBuilder();
            if (!c_fixedSizeDataTypes.Contains(columnsRow.DATA_TYPE.ToLowerInvariant()))
            {
                if (!columnsRow.IsCHARACTER_MAXIMUM_LENGTHNull())
                {
                    dataTypeStringBuilder.AppendFormat("({0})", columnsRow.CHARACTER_MAXIMUM_LENGTH == -1 ? "MAX" : columnsRow.CHARACTER_MAXIMUM_LENGTH.ToString());
                }
                else
                {
                    if (!columnsRow.IsNUMERIC_PRECISIONNull())
                    {
                        if (!columnsRow.IsNUMERIC_SCALENull())
                        {
                            dataTypeStringBuilder.AppendFormat("({0}, {1})", columnsRow.NUMERIC_PRECISION, columnsRow.NUMERIC_SCALE);
                        }
                        else
                        {
                            dataTypeStringBuilder.AppendFormat("({0})", columnsRow.NUMERIC_PRECISION);
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("Don't know how to create the detail for this data type");
                    }
                }
            }
            return dataTypeStringBuilder.ToString();
        }

        internal static string DataTypeWithDetail(SchemaDataSet.SchemaPrimaryKeysRow primaryKeysRow, SchemaDataSet.ColumnsDataTable columnsDataTable)
        {
            var enumerableColumnsDataTable = columnsDataTable.AsEnumerable();
            var columnsTableQuery = from columnsRow in enumerableColumnsDataTable
                                    where columnsRow.TABLE_SCHEMA.Equals(primaryKeysRow.TABLE_SCHEMA, StringComparison.OrdinalIgnoreCase)
                                    where columnsRow.TABLE_NAME.Equals(primaryKeysRow.TABLE_NAME, StringComparison.OrdinalIgnoreCase)
                                    where columnsRow.COLUMN_NAME.Equals(primaryKeysRow.COLUMN_NAME, StringComparison.OrdinalIgnoreCase)
                                    select columnsRow;
            var primaryKeyColumnRow = columnsTableQuery.FirstOrDefault();
            if (null == primaryKeyColumnRow)
            {
                throw new InvalidOperationException("Unexpected: Did not find column from primary key table");
            }
            return string.Format("[{0}]{1}", primaryKeyColumnRow.DATA_TYPE.ToUpperInvariant(), DataTypeDetail(primaryKeyColumnRow));
        }

        private static readonly StringDictionary c_dataTypesToVariableTypes = new StringDictionary
                                                                                  {
                                                                                      {"text", "[varchar](max)"},
                                                                                      {"image", "[varbinary](max)"},
                                                                                      {"ntext", "[nvarchar](max)"},
                                                                                  };

        internal static string DataTypeAsVariable(SchemaDataSet.ColumnsRow columnsRow)
        {
            StringBuilder dataTypeStringBuilder = new StringBuilder();
            if (!c_dataTypesToVariableTypes.ContainsKey(columnsRow.DATA_TYPE.ToLowerInvariant()))
            {
                dataTypeStringBuilder.Append("[" + columnsRow.DATA_TYPE.ToLowerInvariant() + "]");
                dataTypeStringBuilder.Append(DataTypeDetail(columnsRow));
            }
            else
            {
                dataTypeStringBuilder.Append(c_dataTypesToVariableTypes[columnsRow.DATA_TYPE.ToLowerInvariant()]);
            }
            return dataTypeStringBuilder.ToString();
        }

        private static readonly StringCollection c_stringDataTypes = new StringCollection
                                                                    {
                                                                        "char",
                                                                        "nchar",
                                                                        "varchar",
                                                                        "nvarchar",
                                                                        "text",
                                                                        "ntext",
                                                                    };
        internal static bool IsStringDataType(SchemaDataSet.ColumnsRow columnsRow)
        {
            return c_stringDataTypes.Contains(columnsRow.DATA_TYPE.ToLowerInvariant());
        }
    }
}
