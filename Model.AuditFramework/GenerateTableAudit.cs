using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using CodeCharm.Entity.AuditFramework;

namespace CodeCharm.Model.AuditFramework
{
    internal static class GenerateTableAudit
    {
        internal static TrackingTable GenerateAuditForTable(string schema, string tableName, SchemaDataSet.SchemaPrimaryKeysDataTable primaryKeysDataTable, StringBuilder sqlBuilder, AuditUpdateParameters updateParameters, Action<string, float?> reportProgress)
        {
            TrackingTable trackingTable;
            reportProgress(String.Format("Generating audit for table {0}", tableName), null);

            List<SchemaDataSet.ColumnsRow> targetTableColumns = GetAuditedColumnsForTable(schema, tableName, updateParameters);
            bool createNewTableVersion = NewVersionOfTrackingTableNeeded(schema, tableName, targetTableColumns, updateParameters);
            if (createNewTableVersion)
            {
                trackingTable = GenerateTableTracking.GenerateNewTrackingTable(schema, tableName, targetTableColumns, updateParameters, sqlBuilder, reportProgress);
            }
            else
            {
                trackingTable = GenerateTableTracking.GetCurrentTrackingTable(schema, tableName, updateParameters);
            }
            GenerateTableTracking.GenerateAuditSupportForTable(schema, tableName, trackingTable, sqlBuilder, targetTableColumns, updateParameters, reportProgress);
            GenerateTableTracking.GenerateInstantSupportForTable(schema, tableName, sqlBuilder, updateParameters);
            GenerateDelta.CreateDeltaProcedures(schema, tableName, trackingTable, targetTableColumns, sqlBuilder, updateParameters, reportProgress);
            reportProgress(String.Format("Generated audit for table {0}", tableName), null);
            return trackingTable;
        }

        private static List<SchemaDataSet.ColumnsRow> GetAuditedColumnsForTable(string schema, string tableName, AuditUpdateParameters updateParameters)
        {
            StringCollection columnsNotAudited = updateParameters.ColumnsNotAudited;
            IEnumerable<SchemaDataSet.ColumnsRow> columnsRows = GetColumnsForTable(schema, tableName, updateParameters);
            IEnumerable<SchemaDataSet.ColumnsRow> query = from columnRow in columnsRows
                                                          where !(columnsNotAudited.Cast<string>().Any(columnName => columnName.Equals(columnRow.COLUMN_NAME, StringComparison.OrdinalIgnoreCase)))
                                                          select columnRow;
            return query.ToList();
        }

        internal static IEnumerable<SchemaDataSet.ColumnsRow> GetColumnsForTable(string schema, string tableName, AuditUpdateParameters updateParameters)
        {
            SchemaDataSet schemaDataSet = updateParameters.SchemaDataSet;
            SchemaDataSet.ColumnsDataTable columnsDataTable = schemaDataSet.Columns;
            IEnumerable<SchemaDataSet.ColumnsRow> columnsRows = columnsDataTable.Rows.Cast<SchemaDataSet.ColumnsRow>();
            IEnumerable<SchemaDataSet.ColumnsRow> query = from columnRow in columnsRows
                                                          where columnRow.TABLE_SCHEMA.Equals(schema, StringComparison.OrdinalIgnoreCase)
                                                                && columnRow.TABLE_NAME.Equals(tableName, StringComparison.OrdinalIgnoreCase)
                                                          orderby columnRow.ORDINAL_POSITION
                                                          select columnRow;
            return query;
        }

        /// <summary>
        /// Determines whether a new tracking table should be built and catalogued
        /// </summary>
        /// <param name="schema">The name of the audited schema</param>
        /// <param name="tableName">The name of the audited table to check</param>
        /// <param name="targetTableColumns">A list of the columns in the audited table</param>
        /// <param name="updateParameters">Generator parameters</param>
        /// <returns>True of the audited table has different columns (name or type) than the tracking table</returns>
        /// <remarks>
        /// This routine only checks the column names and data types for a match. A change to indexes, relationships, and keys will not affect
        /// the comparison.
        /// </remarks>
        private static bool NewVersionOfTrackingTableNeeded(string schema, string tableName, ICollection<SchemaDataSet.ColumnsRow> targetTableColumns, AuditUpdateParameters updateParameters)
        {
            // check whether we even have a tracking table
            TrackingTable trackingTable = GenerateTableTracking.GetCurrentTrackingTable(schema, tableName, updateParameters);
            if (null == trackingTable)
            {
                return true;
            }

            // get the columns from the tracking table
            List<SchemaDataSet.ColumnsRow> trackingTableColumns = GetTrackingColumnsForTable(trackingTable, updateParameters);

            // check whether the number of columns match
            if (targetTableColumns.Count != trackingTableColumns.Count)
            {
                return true;
            }

            // compare each column
            return AllColumnsMatch(targetTableColumns, trackingTableColumns);
        }

        private static List<SchemaDataSet.ColumnsRow> GetTrackingColumnsForTable(TrackingTable trackingTable, AuditUpdateParameters updateParameters)
        {
            IEnumerable<SchemaDataSet.ColumnsRow> columnsRows = GetColumnsForTable(updateParameters.AuditingSchema, trackingTable.ToString(), updateParameters);
            IEnumerable<SchemaDataSet.ColumnsRow> query = from columnRow in columnsRows
                                                          where !columnRow.COLUMN_NAME.Equals("TrackºId", StringComparison.Ordinal)
                                                          select columnRow;
            return query.ToList();
        }

        private static bool AllColumnsMatch(IEnumerable<SchemaDataSet.ColumnsRow> targetTableColumns, IEnumerable<SchemaDataSet.ColumnsRow> trackingTableColumns)
        {
            Dictionary<string, SchemaDataSet.ColumnsRow> targetColumns = targetTableColumns.ToDictionary(row => row.COLUMN_NAME.ToLowerInvariant());
            Dictionary<string, SchemaDataSet.ColumnsRow> trackingColumns = trackingTableColumns.ToDictionary(row => row.COLUMN_NAME.ToLowerInvariant());
            int targetCount = targetColumns.Count;
            for (int i = 0; i < targetCount; i++)
            {
                SchemaDataSet.ColumnsRow targetColumn = targetColumns.ElementAt(0).Value;
                string targetKey = targetColumn.COLUMN_NAME.ToLowerInvariant();
                targetColumns.Remove(targetKey);

                SchemaDataSet.ColumnsRow trackingColumn = trackingColumns[targetKey];
                if (null == trackingColumn)
                {
                    return true;
                }
                string trackingKey = trackingColumn.COLUMN_NAME.ToLowerInvariant();
                if (!ColumnsMatch(targetColumn, trackingColumn))
                {
                    return true;
                }
                trackingColumns.Remove(trackingKey);
            }
            if (targetColumns.Count > 0 || trackingColumns.Count > 0)
            {
                return true;
            }
            return false;
        }

        private static bool ColumnsMatch(SchemaDataSet.ColumnsRow columnRow1, SchemaDataSet.ColumnsRow columnRow2)
        {
            if (columnRow1.IsDATA_TYPENull() != columnRow2.IsDATA_TYPENull()
                || (!columnRow1.IsDATA_TYPENull() && !columnRow2.IsDATA_TYPENull()
                    && !columnRow1.DATA_TYPE.Equals(columnRow2.DATA_TYPE, StringComparison.OrdinalIgnoreCase))
                )
            {
                return false;
            }
            if (columnRow1.IsCHARACTER_MAXIMUM_LENGTHNull() != columnRow2.IsCHARACTER_MAXIMUM_LENGTHNull()
                || (!columnRow1.IsCHARACTER_MAXIMUM_LENGTHNull() && !columnRow2.IsCHARACTER_MAXIMUM_LENGTHNull()
                    && columnRow1.CHARACTER_MAXIMUM_LENGTH != columnRow2.CHARACTER_MAXIMUM_LENGTH)
                )
            {
                return false;
            }
            if (columnRow1.IsNUMERIC_PRECISIONNull() != columnRow2.IsNUMERIC_PRECISIONNull()
                || (!columnRow1.IsNUMERIC_PRECISIONNull() && !columnRow2.IsNUMERIC_PRECISIONNull()
                    && columnRow1.NUMERIC_PRECISION != columnRow2.NUMERIC_PRECISION)
                )
            {
                return false;
            }
            if (columnRow1.IsNUMERIC_PRECISION_RADIXNull() != columnRow2.IsNUMERIC_PRECISION_RADIXNull()
                || (!columnRow1.IsNUMERIC_PRECISION_RADIXNull() && !columnRow2.IsNUMERIC_PRECISION_RADIXNull()
                    && columnRow1.NUMERIC_PRECISION_RADIX != columnRow2.NUMERIC_PRECISION_RADIX)
                )
            {
                return false;
            }
            if (columnRow1.IsNUMERIC_SCALENull() != columnRow2.IsNUMERIC_SCALENull()
                || (!columnRow1.IsNUMERIC_SCALENull() && !columnRow2.IsNUMERIC_SCALENull()
                    && columnRow1.NUMERIC_SCALE != columnRow2.NUMERIC_SCALE)
                )
            {
                return false;
            }
            if (columnRow1.IsDATETIME_PRECISIONNull() != columnRow2.IsDATETIME_PRECISIONNull()
                || (!columnRow1.IsDATETIME_PRECISIONNull() && !columnRow2.IsDATETIME_PRECISIONNull()
                    && columnRow1.DATETIME_PRECISION != columnRow2.DATETIME_PRECISION)
                )
            {
                return false;
            }
            if (columnRow1.IsCHARACTER_OCTET_LENGTHNull() != columnRow2.IsCHARACTER_OCTET_LENGTHNull()
                || (!columnRow1.IsCHARACTER_OCTET_LENGTHNull() && !columnRow2.IsCHARACTER_OCTET_LENGTHNull()
                    && columnRow1.CHARACTER_OCTET_LENGTH != columnRow2.CHARACTER_OCTET_LENGTH)
                )
            {
                return false;
            }
            return true;
        }
    }
}
