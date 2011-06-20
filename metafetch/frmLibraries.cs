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
