using System.IO.Packaging;

namespace DummyFoldersSync
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Form1";
        }

        #endregion

        private void InitializeCustomComponents()
        {
            // Form settings
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            this.Text = "Folder Synchronizer";
            this.Width = 600;
            this.Height = 500;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Source folder controls
            Label lblSource = new Label
            {
                Text = "Source Folder:",
                Location = new Point(20, 20),
                AutoSize = true
            };
            this.Controls.Add(lblSource);

            TextBox txtSource = new TextBox
            {
                Name = "txtSource",
                Location = new Point(20, 45),
                Width = 450
            };
            this.Controls.Add(txtSource);

            Button btnBrowseSource = new Button
            {
                Text = "Browse...",
                Location = new Point(480, 44),
                Width = 80
            };
            btnBrowseSource.Click += BtnBrowseSource_Click;
            this.Controls.Add(btnBrowseSource);

            // Destination folder controls
            Label lblDestination = new Label
            {
                Text = "Destination Folder:",
                Location = new Point(20, 80),
                AutoSize = true
            };
            this.Controls.Add(lblDestination);

            TextBox txtDestination = new TextBox
            {
                Name = "txtDestination",
                Location = new Point(20, 105),
                Width = 450
            };
            this.Controls.Add(txtDestination);

            Button btnBrowseDestination = new Button
            {
                Text = "Browse...",
                Location = new Point(480, 104),
                Width = 80
            };
            btnBrowseDestination.Click += BtnBrowseDestination_Click;
            this.Controls.Add(btnBrowseDestination);

            // Sync button
            Button btnStart = new Button
            {
                Name = "btnStart",
                Text = "Start Sync",
                Location = new Point(20, 145),
                Width = 100,
                Height = 30
            };
            btnStart.Click += BtnStart_Click;
            this.Controls.Add(btnStart);

            // Progress bar
            ProgressBar progressBar = new ProgressBar
            {
                Name = "progressBar",
                Location = new Point(130, 145),
                Width = 430,
                Height = 30
            };
            this.Controls.Add(progressBar);

            // Log area
            ListView logView = new ListView
            {
                Name = "logView",
                Location = new Point(20, 185),
                Width = 540,
                Height = 250,
                View = View.Details,
                FullRowSelect = true
            };
            logView.Columns.Add("Operation", 100);
            logView.Columns.Add("Path", 440);
            this.Controls.Add(logView);
        }

    }
}
