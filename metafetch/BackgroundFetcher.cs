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
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using metafetch.DataManagers;
using metafetch.DataAccessors;

namespace metafetch
{
    class BackgroundFetcher
    {
        public delegate void Completed(object sender, RunWorkerCompletedEventArgs e);
        public delegate void ProgressUpdate(object sender, ProgressChangedEventArgs e);

        private LibraryManager m_library;
        private frmMain m_caller;
        private volatile bool m_cancelRequested;
        private int m_concurrentFetches;

        public BackgroundFetcher(LibraryManager library, frmMain caller, int concurrentFetches = 5)
        {
            m_library = library;
            m_caller = caller;
            m_concurrentFetches = concurrentFetches;
        }

        public void FetchAsync(IEnumerable<MovieEntry> entries)
        {
            // Start the fetch operation on a new thread.
            m_cancelRequested = false;

            Thread fetchThread = new Thread(new ParameterizedThreadStart(DoFetch));
            fetchThread.Start(entries);
        }

        public void CancelAsync()
        {
            // Signal to the fetcher thread that a cancellation request
            // was made.
            m_cancelRequested = true;
        }

        private void DoFetch(object parameter)
        {
            IEnumerable<MovieEntry> entries = (IEnumerable<MovieEntry>)parameter;
            List<MovieEntry> processedEntries = new List<MovieEntry>();

            FetchWorker[] workers = new FetchWorker[m_concurrentFetches];
            WaitHandle[] doneWorkers = new WaitHandle[workers.Length];

            for (int i = 0; i < workers.Length; i++)
            {
                workers[i] = new FetchWorker(m_library, processedEntries);
                doneWorkers[i] = workers[i].DoneSignal;
            }

            foreach (MovieEntry entry in entries)
            {
                // Wait for a thread to become available.
                int index = WaitHandle.WaitAny(doneWorkers);

                // Notify caller of progress.
                int percentage = (int)(((double)processedEntries.Count() / (double)entries.Count()) * 100.0);
                ProgressChangedEventArgs progressArgs = new ProgressChangedEventArgs(percentage, null);
                m_caller.Invoke(new ProgressUpdate(m_caller.backgroundFetcher_ProgressChanged), new object[] { this, progressArgs });

                if (m_cancelRequested)
                {
                    break;
                }

                // Start a new fetch operation.
                workers[index].FetchAsync(entry);
            }

            // Wait for workers to finish. At a fixed interval, poll to see if any more
            // workers have finished their tasks so that progress may be updated.
            int previousProgress = -1;
            while (true)
            {
                if (WaitHandle.WaitAll(doneWorkers, 500))
                {
                    break;
                }
                else
                {
                    // Notify caller of progress if changed.
                    int percentage = (int)(((double)processedEntries.Count() / (double)entries.Count()) * 100.0);

                    if (percentage != previousProgress)
                    {
                        ProgressChangedEventArgs progressArgs = new ProgressChangedEventArgs(percentage, null);
                        m_caller.Invoke(new ProgressUpdate(m_caller.backgroundFetcher_ProgressChanged), new object[] { this, progressArgs });
                    }

                    previousProgress = percentage;
                }
            }

            // Notify caller of completion. Don't set 'cancelled' even if the process was cancelled, because this
            // will cause the results to be inaccessible. Even when cancelled there still might be partial results.
            RunWorkerCompletedEventArgs completedArgs = new RunWorkerCompletedEventArgs(processedEntries, null, false);
            m_caller.Invoke(new Completed(m_caller.backgroundFetcher_RunWorkerCompleted), new object[] { this, completedArgs });
        }

        private class FetchWorker
        {
            private LibraryManager m_library;
            private ManualResetEvent m_doneEvent;
            private ICollection<MovieEntry> m_processedEntries;

            public FetchWorker(LibraryManager library, ICollection<MovieEntry> processedEntries)
            {
                m_library = library;
                m_doneEvent = new ManualResetEvent(true);
                m_processedEntries = processedEntries;
            }

            public void FetchAsync(MovieEntry entry)
            {
                m_doneEvent.Reset();

                Thread workerThread = new Thread(new ParameterizedThreadStart(DoFetch));
                workerThread.Start(entry);
            }

            public void DoFetch(object parameter)
            {
                MovieEntry entry = (MovieEntry)parameter;

                try
                {
                    m_library.FetchMovieMetadata(entry);
                }
                catch (Exception exc)
                {
                    // Store exception in entry so that the caller task can
                    // process the error and display it to the user.
                    AppLog.Instance.Log(AppLog.Severity.Error, "Error fetching item '" + entry.moviePath + "': " + exc.ToString());
                    entry.loadException = exc;
                }
                finally
                {
                    lock (m_processedEntries)
                    {
                        m_processedEntries.Add(entry);
                    }

                    m_doneEvent.Set();
                }
            }

            public ManualResetEvent DoneSignal
            {
                get { return m_doneEvent; }
                private set { }
            }
        }
    }
}
