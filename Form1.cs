using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DummyFoldersSync
{
    public partial class Form1 : Form
    {
        private CancellationTokenSource _cancellationTokenSource;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        
        private void BtnBrowseSource_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    ((TextBox)Controls["txtSource"]).Text = folderDialog.SelectedPath;
                }
            }
        }

        private void BtnBrowseDestination_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    ((TextBox)Controls["txtDestination"]).Text = folderDialog.SelectedPath;
                }
            }
        }

        private async void BtnStart_Click(object sender, EventArgs e)
        {
            string sourceFolder = ((TextBox)Controls["txtSource"]).Text;
            string destFolder = ((TextBox)Controls["txtDestination"]).Text;

            if (string.IsNullOrWhiteSpace(sourceFolder) || string.IsNullOrWhiteSpace(destFolder))
            {
                MessageBox.Show("Please select both source and destination folders.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Directory.Exists(sourceFolder))
            {
                MessageBox.Show("Source folder does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(destFolder))
            {
                var result = MessageBox.Show("Destination folder does not exist. Create it?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        Directory.CreateDirectory(destFolder);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to create destination folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            Button btnStart = (Button)Controls["btnStart"] ?? throw new InvalidOperationException("Start button not found.");
            ProgressBar progressBar = (ProgressBar)Controls["progressBar"] ?? throw new InvalidOperationException("Progress bar not found.");
            ListView logView = (ListView)Controls["logView"] ?? throw new InvalidOperationException("Log view not found.");

            btnStart.Enabled = false;
            btnStart.Text = "Syncing...";
            logView.Items.Clear();

            try
            {
                _cancellationTokenSource = new CancellationTokenSource();
                
                // Count total files to process for progress reporting
                int totalFiles = Directory.GetFiles(sourceFolder, "*.*", SearchOption.AllDirectories).Length;
                progressBar.Maximum = totalFiles;
                progressBar.Value = 0;

                // Start the synchronization
                await Task.Run(() => SyncFolders(sourceFolder, destFolder, logView, progressBar, _cancellationTokenSource.Token));
                
                MessageBox.Show("Synchronization completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Synchronization was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during synchronization: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnStart.Text = "Start Sync";
                btnStart.Enabled = true;
                _cancellationTokenSource?.Dispose();
            }
        }

        private void SyncFolders(string sourceFolder, string destFolder, ListView logView, ProgressBar progressBar, CancellationToken cancellationToken)
        {
            int processedFiles = 0;

            // Get all source directories including the root
            var sourceDirs = Directory.GetDirectories(sourceFolder, "*", SearchOption.AllDirectories)
                .Concat(new[] { sourceFolder });

            // Create directory structure in destination
            foreach (string sourceDir in sourceDirs)
            {
                cancellationToken.ThrowIfCancellationRequested();

                string destDir = sourceDir.Replace(sourceFolder, destFolder);
                if (!Directory.Exists(destDir))
                {
                    Directory.CreateDirectory(destDir);
                    LogOperation("Created Dir", destDir, logView);
                }
            }

            // Copy files that don't exist in destination
            foreach (string sourceDir in sourceDirs)
            {
                cancellationToken.ThrowIfCancellationRequested();

                string destDir = sourceDir.Replace(sourceFolder, destFolder);
                string[] sourceFiles = Directory.GetFiles(sourceDir);

                foreach (string sourceFile in sourceFiles)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    
                    string destFile = Path.Combine(destDir, Path.GetFileName(sourceFile));
                    
                    if (!File.Exists(destFile))
                    {
                        File.Copy(sourceFile, destFile);
                        LogOperation("Copied", destFile, logView);
                    }
                    else
                    {
                        LogOperation("Skipped", destFile, logView);
                    }
                    
                    // Update progress
                    processedFiles++;
                    UpdateProgress(progressBar, processedFiles);
                }
            }
        }

        private void LogOperation(string operation, string path, ListView logView)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => LogOperation(operation, path, logView)));
                return;
            }

            ListViewItem item = new ListViewItem(new string[] { operation, path });
            logView.Items.Add(item);
            logView.Items[logView.Items.Count - 1].EnsureVisible();
        }

        private void UpdateProgress(ProgressBar progressBar, int value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateProgress(progressBar, value)));
                return;
            }

            if (value <= progressBar.Maximum)
                progressBar.Value = value;
        }
    }
}