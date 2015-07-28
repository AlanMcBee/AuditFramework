using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CodeCharm.Entity.AuditFramework;
using CodeCharm.Model.AuditFramework.T4Templates;
using Microsoft.SqlServer.Management.Smo;

namespace CodeCharm.Model.AuditFramework
{
    internal static class GenerateDatabaseFiles
    {
        internal static void GenerateDatabaseSetup(StringBuilder sqlBuilder, AuditUpdateParameters updateParameters, Action<string, float?> reportProgress)
        {
            Server server = new Server(updateParameters.AuditingDatabaseConnection.DataSource);
            Database database = server.Databases[updateParameters.AuditingDatabaseConnection.Database];
            if (!database.FileGroups.Contains(updateParameters.AuditingFileGroup))
            {
                DataFile dataFile = database.FileGroups[0].Files[0];
                string fileName = dataFile.FileName;
                string path = Path.GetDirectoryName(fileName);
                if (path == null)
                {
                    throw new InvalidOperationException("Can't determine path of database file");
                }
                updateParameters.SqlFilePath = new DirectoryInfo(path);
                SetupDatabase setupDatabaseTemplate = new SetupDatabase(updateParameters);
                string text = setupDatabaseTemplate.TransformText();
                sqlBuilder.AppendLine(text);
            }
            reportProgress("Generated database setup", null);
        }
    }
}
