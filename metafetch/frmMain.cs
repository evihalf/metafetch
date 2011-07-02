using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using metafetch.DataAccessors;
using metafetch.DataAccessors.TMDB;
using metafetch.DataManagers;
using metafetch.DataManagers.WMC;

namespace metafetch
{
    public partial class frmMain : Form
    {
        private LibraryManager m_library;
        private UserSettings m_settings;

        private BackgroundFetcher m_fetcher;

        public frmMain()
        {
            InitializeComponent();

            m_settings = new UserSettings();

            // Create accessor for TheMovieDb.
            TMDBAccessor tmdbAccessor = new TMDBAccessor(ConfigurationManager.AppSettings["TMDB_API_KEY"], "en");

            // Create data manager for Windows Media Center.
            string wmcCache = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\eHome";
            WMCDataManager wmcDataManager = new WMCDataManager(wmcCache + "\\DvdInfoCache", wmcCache + "\\DvdCoverCache");

            List<DataManager> dataManagers = new List<DataManager>();
            dataManagers.Add(wmcDataManager);

            // Create the library using the specified accessor and data manager(s).
            m_library = new LibraryManager(tmdbAccessor, dataManagers);
            m_library.MovieLoaded += new DataManagers.MovieLoadEventHandler(MovieLoadEventHandler);

            // Create the background fetcher and hook into events.
            m_fetcher = new BackgroundFetcher(m_library, this, int.Parse(ConfigurationManager.AppSettings["MAX_CONCURRENT_FETCHES"]));
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Load movies from library paths.
            ReloadMovies(m_settings.LibraryPaths.OfType<string>());
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save library paths.
            StringCollection paths = new StringCollection();
            paths.AddRange(m_library.GetLibraryPaths().ToArray<string>());
            m_settings.LibraryPaths = paths;

            m_settings.Save();
        }

        private void librariesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLibraries dialog = new frmLibraries();
            dialog.SetLibraryPaths(m_library.GetLibraryPaths());

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Reload movies from updated library paths.
                ReloadMovies(dialog.GetLibraryPaths());
            }
        }

        public void MovieLoadEventHandler(MovieEntry entry, Exception loadException)
        {
            // Add the item to the correct list (depending on whether or
            // not it already has metadata available).
            if (entry.movie != null)
            {
                AddMetadataWith(entry);
            }
            else
            {
                if (loadException != null)
                    AddMetadataWithout(entry, "Error loading metadata for movie from file.");
                else
                    AddMetadataWithout(entry);
            }
        }

        private void ReloadMovies(IEnumerable<string> libraryPaths)
        {
            lblStatus.Text = "Loading movies from libraries...";
            m_library.ClearLibraryMovies();

            lvMetadataWithout.Items.Clear();
            lvMetadataWith.Items.Clear();

            // TODO: Multithread this so the UI doesn't hang when loading.
            foreach (string path in libraryPaths)
            {
                m_library.LoadLibraryMovies(path);
            }

            lblStatus.Text = "";
            UpdateUI();
        }

        private void UpdateUI()
        {
            // Enable/Disable fetch button depending on how many items
            // are selected.
            if (chkAutoFetch.Checked == true)
            {
                btnFetchSelected.Enabled = false;
            }
            else
            {
                if (lvMetadataWithout.CheckedItems.Count > 0)
                    btnFetchSelected.Enabled = true;
                else
                    btnFetchSelected.Enabled = false;
            }

            // Enable/disable flush button.
            if (lvMetadataWith.CheckedItems.Count > 0)
                btnFlushSelected.Enabled = true;
            else
                btnFlushSelected.Enabled = false;
        }

        private void lvMetadataWithout_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UpdateUI();
        }

        private void chkAutoFetch_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void lvMetadataWith_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UpdateUI();
        }

        private void btnFetchSelected_Click(object sender, EventArgs e)
        {
            if (lvMetadataWithout.CheckedItems.Count > 0)
            {
                // Fetch data for selected items.
                List<MovieEntry> entries = new List<MovieEntry>(lvMetadataWithout.Items.Count);
                foreach (ListViewItem item in lvMetadataWithout.CheckedItems)
                {
                    MovieEntry entry = (MovieEntry)item.Tag;
                    entries.Add(entry);
                }

                FetchMetadata(entries);
            }
        }

        private void btnFlushSelected_Click(object sender, EventArgs e)
        {
            // Flush metadata of all selected items.
            bool ignore = false;

            foreach (ListViewItem item in lvMetadataWith.CheckedItems)
            {
                MovieEntry entry = (MovieEntry)item.Tag;

                // Remove from the 'with metadata' list and add to
                // 'without metadata' list.
                lvMetadataWith.Items.Remove(item);
                AddMetadataWithout(entry);

                // Flush movie metadata.
                while (true) // Allow retrying if selected by user.
                {
                    try
                    {
                        m_library.FlushMovieMetadata(entry);
                    }
                    catch (Exception exc)
                    {
                        AppLog.Instance.Log(AppLog.Severity.Error, "Error flushing metadata: " + exc.ToString());

                        if (!ignore)
                        {
                            DialogResult option = MessageBox.Show("An error occurred while flushing the metadata of a selected movie." +
                                " Make sure you have access to the movie library. More information may be available in the log file.", "Error",
                                MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);

                            switch (option)
                            {
                                case DialogResult.Abort:
                                    return;
                                case DialogResult.Retry:
                                    continue;
                                case DialogResult.Ignore:
                                    ignore = true;
                                    break;
                            }
                        }

                    }

                    break;
                }

            }
        }

        private void AddMetadataWithout(MovieEntry entry, string errorMessage = null)
        {
            // Add item to 'without' list.
            ListViewItem item = new ListViewItem();
            item.Tag = entry;
            item.Text = "";

            if (errorMessage != null)
            {
                item.ImageIndex = 0;
                item.ToolTipText = errorMessage;
            }
            
            item.SubItems.Add(entry.movieTag);
            item.SubItems.Add(entry.moviePath);
            lvMetadataWithout.Items.Add(item);
        }

        private void AddMetadataWith(MovieEntry entry)
        {
            // Add item to 'with' list.
            ListViewItem item = new ListViewItem();
            item.Tag = entry;
            item.Text = "";
            item.SubItems.Add(entry.movie.Name);
            item.SubItems.Add((entry.movie.Released != null) ? entry.movie.Released.Value.Year.ToString() : "");
            lvMetadataWith.Items.Add(item);
        }

        private void MetadataError(ListViewItem item, string errorMessage = null)
        {
            if (errorMessage != null)
            {
                item.ImageIndex = 0;
                item.ToolTipText = errorMessage;
            }
            else
            {
                item.ImageIndex = -1;
                item.ToolTipText = null;
            }
        }

        private void LockUI()
        {
            // Lock the user interface components when a task is being executed.
            lvMetadataWithout.Enabled = false;
            lvMetadataWith.Enabled = false;
            btnFetchSelected.Enabled = false;
            btnFlushSelected.Enabled = false;
            
            chkAutoFetch.Enabled = false;
            librariesToolStripMenuItem.Enabled = false;
        }

        private void UnlockUI()
        {
            // Unlock UI components after a task is finished.
            lvMetadataWithout.Enabled = true;
            lvMetadataWith.Enabled = true;
            btnFetchSelected.Enabled = true;
            btnFlushSelected.Enabled = true;

            chkAutoFetch.Enabled = true;
            librariesToolStripMenuItem.Enabled = true;

            UpdateUI();
        }

        private void FetchMetadata(IEnumerable<MovieEntry> entries)
        {
            if (entries.Count() < 1)
                return;

            // Prepare UI for fetching operation.
            LockUI();

            lblStatus.Text = "Fetching metadata...";
            pbProgress.Value = 0;
            pbProgress.Style = ProgressBarStyle.Blocks;
            pbProgress.Visible = true;
            btnCancel.Enabled = true;
            btnCancel.Visible = true;

            // Start background fetch of metadata.
            m_fetcher.FetchAsync(entries);
        }

        public void backgroundFetcher_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pbProgress.Visible = false;
            lblStatus.Text = "";
            btnCancel.Visible = false;

            if (e.Error != null)
            {
                AppLog.Instance.Log(AppLog.Severity.Error, "Error in background fetcher: " + e.Error.ToString());
                MessageBox.Show("An error occurred while fetching metadata. More information may be available in the log file.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            // Update processed items in the listviews.
            IEnumerable<MovieEntry> processed = (IEnumerable<MovieEntry>)e.Result;
            List<ListViewItem> removables = new List<ListViewItem>();
            List<ListViewItem> removablesWith = new List<ListViewItem>();

            foreach (MovieEntry entry in processed)
            {
                // Find the corresponding listview item.
                ListViewItem item = null;
                bool fromWithoutList = false;

                foreach (ListViewItem lvi in lvMetadataWithout.Items)
                {
                    if (((MovieEntry)lvi.Tag) == entry)
                    {
                        item = lvi;
                        fromWithoutList = true;
                        break;
                    }
                }

                if (item == null)
                {
                    foreach (ListViewItem lvi in lvMetadataWith.Items)
                    {
                        if (((MovieEntry)lvi.Tag) == entry)
                        {
                            item = lvi;
                            fromWithoutList = false;
                            break;
                        }
                    }
                }

                if (item != null)
                {
                    if (entry.movie == null)
                    {
                        // Failed to fetch metadata.
                        if (entry.loadException is NoResultsFoundException)
                            MetadataError(item, entry.loadException.Message);
                        else
                            MetadataError(item, "Error downloading movie metadata.");
                    }
                    else
                    {
                        if (fromWithoutList)
                        {
                            // Remove it from the without list and add to
                            // with list.
                            removables.Add(item);
                            AddMetadataWith(entry);
                        }
                        else
                        {
                            // Add new item and remove old item from 'with' list.
                            removablesWith.Add(item);
                            AddMetadataWith(entry);
                        }
                    }
                }
            }

            foreach (ListViewItem item in removables)
                lvMetadataWithout.Items.Remove(item);

            foreach (ListViewItem item in removablesWith)
                lvMetadataWith.Items.Remove(item);

            UnlockUI();

            // Update selected 'with' item so that the picture updates.
            lvMetadataWith_SelectedIndexChanged(this, EventArgs.Empty);
        }

        

        public void backgroundFetcher_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Update progress bar.
            pbProgress.Value = e.ProgressPercentage;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Cancel the current background process.
            m_fetcher.CancelAsync();

            lblStatus.Text = "Finishing last item(s)...";
            pbProgress.Style = ProgressBarStyle.Marquee;
            btnCancel.Enabled = false;
        }

        private void FetchMetadataManual(MovieEntry entry)
        {
            // Show the manual metadata fetch form.
            frmManualFetch dialog = new frmManualFetch(entry, m_library.GetMetadataAccessor());
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Fetch the selected item by ID. Use the same fetching
                // infrastructure as with the automated fetch.
                List<MovieEntry> entries = new List<MovieEntry>();

                entry.manualID = dialog.SelectedID;
                entries.Add(entry);

                FetchMetadata(entries);
            }
        }

        private void lvMetadataWithout_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Select/unselect all.
            if (e.Column == 0)
            {
                if (lvMetadataWithout.CheckedItems.Count == lvMetadataWithout.Items.Count)
                {
                    // Uncheck all.
                    foreach (ListViewItem item in lvMetadataWithout.Items)
                        item.Checked = false;
                }
                else
                {
                    // Check all.
                    foreach (ListViewItem item in lvMetadataWithout.Items)
                        item.Checked = true;
                }
            }
        }

        private void lvMetadataWith_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Select/unselect all.
            if (e.Column == 0)
            {
                if (lvMetadataWith.CheckedItems.Count == lvMetadataWith.Items.Count)
                {
                    // Uncheck all.
                    foreach (ListViewItem item in lvMetadataWith.Items)
                        item.Checked = false;
                }
                else
                {
                    // Check all.
                    foreach (ListViewItem item in lvMetadataWith.Items)
                        item.Checked = true;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lvMetadataWith_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvMetadataWith.SelectedItems.Count > 0)
            {
                MovieEntry entry = (MovieEntry)lvMetadataWith.SelectedItems[0].Tag;

                // Display picture if available.
                if (entry != null && entry.movie != null)
                {
                    if (entry.movie.Images.Count > 0)
                    {
                        pbImage.ImageLocation = entry.movie.Images.First().path;
                    }
                }
            }
            else
            {
                pbImage.ImageLocation = null;
            }
        }

        private void cmsWith_Opening(object sender, CancelEventArgs e)
        {
            // Make sure an item is under the mouse when opening the
            // context menu.
            Point mousePos = lvMetadataWith.PointToClient(Control.MousePosition);
            ListViewItem item = lvMetadataWith.GetItemAt(mousePos.X, mousePos.Y);
            if (item != null)
            {
                cmsWith.Tag = (MovieEntry)item.Tag;
            }
            else
            {
                e.Cancel = true;
            }

        }

        private void cmsWithout_Opening(object sender, CancelEventArgs e)
        {
            // Make sure an item is under the mouse when opening the
            // context menu.
            Point mousePos = lvMetadataWithout.PointToClient(Control.MousePosition);
            ListViewItem item = lvMetadataWithout.GetItemAt(mousePos.X, mousePos.Y);
            if (item != null)
            {
                cmsWithout.Tag = (MovieEntry)item.Tag;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void withManualFetchMenuItem_Click(object sender, EventArgs e)
        {
            // Manually fetch the selected item.
            MovieEntry entry = (MovieEntry)cmsWith.Tag;

            if (entry != null)
                FetchMetadataManual(entry);
        }

        private void withoutManualFetchMenuItem_Click(object sender, EventArgs e)
        {
            // Manually fetch the selected item.
            MovieEntry entry = (MovieEntry)cmsWithout.Tag;

            if (entry != null)
                FetchMetadataManual(entry);
        }

        

    }
}
