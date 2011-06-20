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

namespace metafetch
{
    public partial class frmLibraries : Form
    {
        public frmLibraries()
        {
            InitializeComponent();
        }

        public void SetLibraryPaths(IEnumerable<string> libraryPaths)
        {
            foreach (string path in libraryPaths)
                lstLibraries.Items.Add(path);
        }

        public IEnumerable<string> GetLibraryPaths()
        {
            return lstLibraries.Items.OfType<string>();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void lstLibraries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstLibraries.SelectedIndex >= 0)
                btnRemove.Enabled = true;
            else
                btnRemove.Enabled = false;
        }

        private void btnAddLibrary_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Select library path";

            while (true)
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // Make sure the path isn't already in the list.
                    if (lstLibraries.Items.Contains(dialog.SelectedPath))
                    {
                        MessageBox.Show("Selected library has already been added. Please choose a different library.",
                            "Duplicate Library", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        continue;
                    }
                    else
                    {
                        lstLibraries.Items.Add(dialog.SelectedPath);
                    }
                }

                break;
            }
            
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lstLibraries.SelectedIndex >= 0)
            {
                lstLibraries.Items.RemoveAt(lstLibraries.SelectedIndex);
            }
        }
    }
}
