using System;
using System.ComponentModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CodeCharm.Entity.AuditFramework;
using CodeCharm.Model.AuditFramework;
using CodeCharm.Windows.AuditFramework.Properties;
using CodeCharm.Utility;
using Microsoft.Win32;

namespace CodeCharm.Windows.AuditFramework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadFieldsFromSettings();
        }

        private void LoadFieldsFromSettings()
        {
            ConnectionStringTextBox.Text = Settings.Default.AuditingDatabaseConnectionString;
            AuditTableFormatTextBox.Text = Settings.Default.AuditTableFormat;
            AlwaysRecreateCheckBox.IsChecked = Settings.Default.AlwaysRecreateAuditingSupport;
            AuditingFileGroupTextBox.Text = Settings.Default.AuditingFilegroup;
            RemoveAllCheckBox.IsChecked = Settings.Default.RemoveAll;
            AutomaticallyDeployCheckBox.IsChecked = Settings.Default.AutoDeployToDatabase;
            OutputPathTextBox.Text = Path.Combine(Path.GetFullPath(Settings.Default.OutputPath), DateTime.Now.ToString("yyyy-MM-dd T HH-mm-ss") + ".sql");
            AuditingSchemaTextBox.Text = Settings.Default.AuditingSchema;
            MatchByPrimaryKeysCheckBox.IsChecked = Settings.Default.TrackByPrimaryKeys;
            SchemasToAuditTextBox.Text = Settings.Default.SchemasToAudit.SpaceDelimitedList();
            GenerateBaselineCheckBox.IsChecked = Settings.Default.GenerateBaseline;
            ExecuteGranteesTextBox.Text = Settings.Default.ExecuteGrantees.SpaceDelimitedList();
            TablesNotAuditedTextBox.Text = Settings.Default.TablesNotAudited.SpaceDelimitedList();
            ColumnsNotAuditedTextBox.Text = Settings.Default.ColumnsNotStoredInAudit.SpaceDelimitedList();
        }

        private void SaveFieldsToSettings()
        {
            Settings.Default.AlwaysRecreateAuditingSupport = AlwaysRecreateCheckBox.IsChecked ?? false;
            Settings.Default.AuditingDatabaseConnectionString = ConnectionStringTextBox.Text;
            Settings.Default.AuditingFilegroup = AuditingFileGroupTextBox.Text;
            Settings.Default.AuditingSchema = AuditingSchemaTextBox.Text;
            Settings.Default.AuditTableFormat = AuditTableFormatTextBox.Text;
            Settings.Default.AutoDeployToDatabase = AutomaticallyDeployCheckBox.IsChecked ?? false;
            Settings.Default.OutputPath = Path.GetDirectoryName(OutputPathTextBox.Text);
            Settings.Default.RemoveAll = RemoveAllCheckBox.IsChecked ?? false;
            Settings.Default.SchemasToAudit.Clear();
            Settings.Default.SchemasToAudit.AddRange(SchemasToAuditTextBox.Text.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            Settings.Default.TrackByPrimaryKeys = MatchByPrimaryKeysCheckBox.IsChecked ?? false;
            Settings.Default.GenerateBaseline = GenerateBaselineCheckBox.IsChecked ?? false;
            Settings.Default.ExecuteGrantees.Clear();
            Settings.Default.ExecuteGrantees.AddRange(ExecuteGranteesTextBox.Text.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            Settings.Default.TablesNotAudited.Clear();
            Settings.Default.TablesNotAudited.AddRange(TablesNotAuditedTextBox.Text.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            Settings.Default.ColumnsNotStoredInAudit.Clear();
            Settings.Default.ColumnsNotStoredInAudit.AddRange(ColumnsNotAuditedTextBox.Text.Trim().Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries));

            Settings.Default.Save();
        }

        private void CommandBinding_PreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Command.Equals(ApplicationCommands.Close))
            {
                e.CanExecute = true;
                return;
            }
        }

        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFieldsToSettings();

            DbConnectionStringBuilder connectionStringBuilder = new DbConnectionStringBuilder();
            connectionStringBuilder.ConnectionString = ConnectionStringTextBox.Text;
            SqlConnection sqlConnection = new SqlConnection(connectionStringBuilder.ConnectionString);

            AuditUpdateParameters parameters = new AuditUpdateParameters
            {
                AlwaysRecreateFramework = AlwaysRecreateCheckBox.IsChecked ?? false,
                AuditingDatabaseConnection = sqlConnection,
                AuditingFileGroup = AuditingFileGroupTextBox.Text,
                AuditingSchema = AuditingSchemaTextBox.Text,
                AuditTableFormat = AuditTableFormatTextBox.Text,
                AutoDeployToDatabase = AutomaticallyDeployCheckBox.IsChecked ?? false,
                DatabaseName = sqlConnection.Database,
                OutputPath = new DirectoryInfo(OutputPathTextBox.Text),
                RemoveAll = RemoveAllCheckBox.IsChecked ?? false,
                TrackByPrimaryKeys = MatchByPrimaryKeysCheckBox.IsChecked ?? false,
                GenerateBaseline = GenerateBaselineCheckBox.IsChecked ?? false
            };
            parameters.ColumnsNotAudited.AddRange(new[] { "", "" });
            parameters.ExecuteGrantees.AddRange(ExecuteGranteesTextBox.Text.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            parameters.SchemasToAudit.AddRange(SchemasToAuditTextBox.Text.Trim().Split('\n'));
            parameters.TablesNotAudited.AddRange(TablesNotAuditedTextBox.Text.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            parameters.ColumnsNotAudited.AddRange(ColumnsNotAuditedTextBox.Text.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

            Generator generator = new Generator();
            string sql = generator.GenerateAuditingFrameworkUpdateSql(parameters);

            SqlWindow sqlWindow = new SqlWindow();
            sqlWindow.SqlText = sql;
            sqlWindow.ShowDialog();
        }

        private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            Validate();
            e.CanExecute = IsValid;
        }

        private bool IsValid
        {
            get;
            set;
        }

        private void Validate()
        {
            IsValid = false;
            try
            {
                DbConnectionStringBuilder connectionStringBuilder = new DbConnectionStringBuilder();
                connectionStringBuilder.ConnectionString = ConnectionStringTextBox.Text;

                bool valid;
                valid = !string.IsNullOrWhiteSpace(AuditTableFormatTextBox.Text);
                AuditTableFormatTextBox.Background = valid ? null : Brushes.LightSalmon;
                if (!valid)
                    return;

                IsValid = true;
            }
            catch (Exception ex)
            {
                IsValid = false;
            }
        }

        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.InitialDirectory = Path.GetFullPath(OutputPathTextBox.Text);
            saveDialog.FileName = Path.GetFileName(OutputPathTextBox.Text);
            saveDialog.CheckPathExists = true;
            saveDialog.DefaultExt = "sql";
            saveDialog.Filter = "SQL Script (.sql)|*.sql|All files (*.*)|*.*";
            saveDialog.FileOk += SaveDialog_FileOk;
            saveDialog.ShowDialog(this);
        }

        private void SaveDialog_FileOk(object sender, CancelEventArgs e)
        {
            SaveFileDialog saveDialog = (SaveFileDialog)sender;
            OutputPathTextBox.Text = saveDialog.FileName;
        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
