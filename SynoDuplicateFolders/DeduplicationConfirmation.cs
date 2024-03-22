using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using SynoDuplicateFolders.Data.Core;
using System.Linq;

namespace SynoDuplicateFolders
{
    public partial class DeduplicationConfirmation : Form
    {
        private List<DirectoryInfo> folders;
        private Deduplication dedupJob = new Deduplication();
        public DeduplicationConfirmation(List<DirectoryInfo> folders)
        {
            InitializeComponent();
            this.folders = folders;
            progressBar1.Minimum = 0;
        }

        public void WriteLine(string message)
        {
            listBox1.Items.Add(message);
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            btnNo.Enabled = false;
            btnYes.Enabled = false;

            dedupJob.AcceptDeletes();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Hide();
        }
        private void DedupJob_OnDeduplicationConfirmation(object sender, DeduplicationConfirmationEventArgs e)
        {
            var message = new List<string>(e.Message);
            var question = message.Last();
            message.Remove(question);

            progressBar1.Visible = false;
            btnNo.Visible = true;
            btnYes.Visible = true;
            listBox1.Items.AddRange(message.ToArray());
            lblProgress.Text = question.Replace("(yes/no)?", string.Empty);
            listBox1.SelectedItem = null;
        }


        private void DedupJob_OnDeduplicationInformationUpdate(object sender, DeduplicationInformationEventArgs e)
        {
            listBox1.Items.Add(e.Message);
            Application.DoEvents();
        }

        private void DedupJob_OnDeduplicationRequestStatusUpdate(object sender, DeduplicationRequestStatusEventArgs e)
        {
            if (e.ProgressMaximum.HasValue) progressBar1.Maximum = e.ProgressMaximum.Value;
            progressBar1.Value = e.ProgressValue;
            if (progressBar1.Maximum == progressBar1.Value)
            {
                progressBar1.Maximum = int.MaxValue;
                progressBar1.Value = int.MaxValue - 1;
            }
    
            System.Diagnostics.Debug.WriteLine($"ProgressBar {progressBar1.Value}/{progressBar1.Maximum}");
            if (e.StatusMessage is null) { } else
            lblProgress.Text = e.StatusMessage;
            Application.DoEvents();

        }

        private void DeduplicationConfirmation_Shown(object sender, EventArgs e)
        {
            dedupJob.OnDeduplicationRequestStatusUpdate += DedupJob_OnDeduplicationRequestStatusUpdate;
            dedupJob.OnDeduplicationInformationUpdate += DedupJob_OnDeduplicationInformationUpdate;
            dedupJob.OnDeduplicationConfirmation += DedupJob_OnDeduplicationConfirmation;
            dedupJob.DeduplicateFiles(folders);
        }
    }
}
