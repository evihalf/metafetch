/*-----------------------------------------------------------------------------
  Copyright (C) 2011 Daniel Flahive. All rights reserved.

  Redistribution and use in source and binary forms, with or without
  modification, are permitted provided that the following conditions
  are met:

  1. Redistributions of source code must retain the above copyright notice,
     this list of conditions and the following disclaimer.

  2. Redistributions in binary form must reproduce the above copyright notice,
     this list of conditions and the following disclaimer in the documentation
     and/or other materials provided with the distribution.

  THIS SOFTWARE IS PROVIDED ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES,
  INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY
  AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
  IN NO EVENT SHALL THE AUTHOR OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
  INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
  (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
  LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
  ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
  (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
  THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
  ---------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using metafetch.DataAccessors;
using metafetch.DataManagers;

namespace metafetch
{
    public partial class frmManualFetch : Form
    {
        private MovieEntry m_entry;
        private MetadataAccessor m_accessor;

        public frmManualFetch(MovieEntry entry, MetadataAccessor accessor)
        {
            InitializeComponent();

            m_entry = entry;
            m_accessor = accessor;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lvResults.SelectedItems.Count > 0)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void UpdateUI()
        {
            if (txtSearch.Text.Length < 1)
                btnSearch.Enabled = false;
            else
                btnSearch.Enabled = true;

            if (lvResults.SelectedItems.Count > 0)
                btnOK.Enabled = true;
            else
                btnOK.Enabled = false;
        }

        private void LockUI()
        {
            btnSearch.Enabled = false;
            lvResults.Enabled = false;
            btnOK.Enabled = false;
        }

        private void UnlockUI()
        {
            btnSearch.Enabled = true;
            lvResults.Enabled = true;
            btnOK.Enabled = true;

            UpdateUI();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            LockUI();

            lvResults.Items.Clear();
            pbProgress.Visible = true;

            backgroundSearch.RunWorkerAsync(txtSearch.Text);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text.Length < 1)
                btnSearch.Enabled = false;
            else
                btnSearch.Enabled = true;
        }

        private void backgroundSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string search = (string)e.Argument;

                IEnumerable<MovieSearchResult> results = m_accessor.SearchMovies(search, null);
                e.Result = results;
            }
            catch (Exception exc)
            {
                AppLog.Instance.Log(AppLog.Severity.Error, "Manual fetch background search error: " + exc.ToString());
                e.Cancel = true;
            }
        }

        private void backgroundSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pbProgress.Visible = false;
            UnlockUI();

            if (e.Cancelled || e.Error != null)
            {
                MessageBox.Show("An error occurred while searching. Please check your internet connection."
                    + " More information may be available in the log file.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                IEnumerable<MovieSearchResult> results = (IEnumerable<MovieSearchResult>)e.Result;

                foreach (MovieSearchResult result in results)
                {
                    ListViewItem item = new ListViewItem();
                    item.Tag = result.ID;
                    item.Text = result.Title;
                    item.SubItems.Add((result.Released != null) ? result.Released.Value.Year.ToString() : "");
                    lvResults.Items.Add(item);
                }
            }
        }

        private void lvResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvResults.SelectedItems.Count > 0)
                btnOK.Enabled = true;
            else
                btnOK.Enabled = false;
        }

        public string SelectedID
        {
            get { return (string)lvResults.SelectedItems[0].Tag; }
            private set { }
        }
    }
}
