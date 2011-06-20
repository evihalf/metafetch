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
using System.IO;
using System.Xml.Linq;
using metafetch.DataAccessors;

namespace metafetch.DataManagers
{
    public delegate void MovieLoadEventHandler(MovieEntry entry, Exception loadException);

    /// <summary>
    /// Stores a list of movies and controls metadata attached to each movie.
    /// </summary>
    public class LibraryManager
    {
        public event MovieLoadEventHandler MovieLoaded;

        // List of library directories that contain movies.
        private List<string> m_libraryPaths;

        // List of movie entries.
        private MovieLibrary m_movies;

        private MetadataAccessor m_accessor;

        private List<DataManager> m_dataManagers;

        public LibraryManager(MetadataAccessor accessor, IEnumerable<DataManager> dataManagers)
        {
            m_libraryPaths = new List<string>();
            m_movies = new MovieLibrary();
            m_accessor = accessor;
            m_dataManagers = new List<DataManager>(dataManagers);
        }

        /// <summary>
        /// Load list of movies and metadata from library.
        /// </summary>
        public void LoadLibraryMovies(string libraryPath)
        {
            // Ensure this library was not already added.
            if (m_libraryPaths.Contains(libraryPath))
                return;

            // Add library directory to the list of libraries.
            m_libraryPaths.Add(libraryPath);

            // Get library subdirectories.
            AppLog.Instance.Log(AppLog.Severity.Debug, "Looking into library directory '" + libraryPath + "' for movies.");

            string[] movieDirs;
            try
            {
               movieDirs = Directory.GetDirectories(libraryPath);
            }
            catch (IOException exc)
            {
                AppLog.Instance.Log(AppLog.Severity.Error, "Couldn't access library directory '" + libraryPath + "': " + exc.Message);

                // Re-throw exception to caller.
                throw exc;
            }

            // Load movies from library path.
            foreach (string movieDir in movieDirs)
            {
                MovieEntry entry = new MovieEntry();
                entry.movieTag = movieDir.Substring(movieDir.LastIndexOf('\\') + 1);
                entry.moviePath = movieDir;
                entry.libraryPath = libraryPath;
                entry.movie = null;

                // Make sure there is no duplicate entry in the list.
                if (m_movies.Contains(entry.moviePath))
                    continue;

                Exception loadException = null;
                try
                {
                    // Load the movie metadata (if any).
                    entry.movie = LoadMovieMetadata(movieDir);

                    if (entry.movie == null)
                        AppLog.Instance.Log(AppLog.Severity.Debug, "\tNo metadata found for movie '" + entry.movieTag + "'.");
                    else
                        AppLog.Instance.Log(AppLog.Severity.Debug, "\tLoaded metadata for movie '" + entry.movieTag + "'.");
                }
                catch (Exception exc)
                {
                    AppLog.Instance.Log(AppLog.Severity.Error, "\tFailed to load metadata for movie '" + entry.movieTag + "'. Reason: " + exc.Message);
                    loadException = exc;
                }

                // Add the movie to the list.
                m_movies.Add(entry);

                // Notify event subscribers.
                MovieLoaded(entry, loadException);
            }
        
        }

        /// <summary>
        /// Unload all movies from a specified library.
        /// </summary>
        /// <param name="libraryPath">Library path to unload movies from</param>
        public void UnloadLibraryMovies(string libraryPath)
        {
            // Make sure the library was loaded.
            if (!m_libraryPaths.Contains(libraryPath))
                return;

            LinkedList<string> toRemove = new LinkedList<string>();
            foreach (MovieEntry entry in m_movies)
            {
                if (entry.libraryPath == libraryPath)
                    toRemove.AddLast(entry.moviePath);
            }

            foreach (string key in toRemove)
                m_movies.Remove(key);

            m_libraryPaths.Remove(libraryPath);
        }

        /// <summary>
        /// Clears the library of all movies.
        /// </summary>
        public void ClearLibraryMovies()
        {
            m_movies.Clear();
            m_libraryPaths.Clear();
        }

        public IEnumerable<MovieEntry> GetMovies()
        {
            return m_movies;
        }

        public MetadataAccessor GetMetadataAccessor()
        {
            return m_accessor;
        }

        /// <summary>
        /// Fetches metadata for a given movie. This method is
        /// thread-safe. Throws an exception on failure.
        /// </summary>
        /// <param name="entry">Entry to fetch.</param>
        public void FetchMovieMetadata(MovieEntry entry)
        {
            if (!m_movies.Contains(entry))
                throw new Exception("Movie entry does not exist in library.");

            // Clear previous metadata if it exists.
            FlushMovieMetadata(entry);

            AppLog.Instance.Log(AppLog.Severity.Debug, "Fetching metadata for '" + entry.moviePath + "'.");

            // Create metadata directory for movie if it doesn't exist.
            CreateMetadataDir(entry.moviePath);

            // If the ID was not provided, search for movie and pick first result.
            string movieID;
            if (!string.IsNullOrEmpty(entry.manualID))
            {
                movieID = entry.manualID;
            }
            else
            {
                IEnumerable<MovieSearchResult> results = m_accessor.SearchMovies(entry.movieTag, null);
                if (results.Count() < 1)
                    throw new NoResultsFoundException("No results found for specified movie.");

                MovieSearchResult result = results.First();
                movieID = result.ID;
            }

            // Fetch the movie.
            Movie movie = m_accessor.FetchMovie(movieID, entry.moviePath + "\\" + Constants.METADATA_PATH);

            lock (entry)
            {
                entry.movie = movie;

                // Write the metadata to disk.
                WriteMovieMetadata(entry.moviePath, entry.movie);

                // Notify data managers of the change.
                foreach (DataManager manager in m_dataManagers)
                    manager.Add(entry);
            }
            
        }

        /// <summary>
        /// Deletes all metadata associated with a movie.
        /// </summary>
        /// <param name="entry">Movie to delete metadata.</param>
        public void FlushMovieMetadata(MovieEntry entry)
        {
            AppLog.Instance.Log(AppLog.Severity.Debug, "Flushing metadata for movie '" + entry.moviePath + "'.");
            string metadataPath = entry.moviePath + "\\" + Constants.METADATA_PATH;
            string metadataFile = metadataPath + "\\" + Constants.METADATA_FILE;

            Exception deleteException = null;
            try
            {
                // Tell data managers to clear metadata.
                foreach (DataManager manager in m_dataManagers)
                    manager.Clear(entry);

                if (Directory.Exists(metadataPath))
                {
                    if (File.Exists(metadataFile))
                        File.Delete(metadataFile);

                    // Delete associated images.
                    if (entry.movie != null)
                    {
                        foreach (MovieImage mi in entry.movie.Images)
                        {
                            if (File.Exists(mi.path))
                                File.Delete(mi.path);
                        }
                    }

                    // Try to delete directory.
                    Directory.Delete(metadataPath);
                }
                
            }
            catch (Exception exc)
            {
                deleteException = exc;
            }

            // Clear movie metadata.
            entry.movie = null;
            
            // Re-throw delete exception if one occurred.
            if (deleteException != null)
                throw deleteException;
        }

        /// <summary>
        /// Load metadata from the given movie path into a movie object.
        /// An exception is thrown if loading metadata failed.
        /// </summary>
        /// <param name="moviePath">Directory containing movie.</param>
        /// <returns>Movie object containing metadata, or null if no metadata exists.</returns>
        private Movie LoadMovieMetadata(string moviePath)
        {
            string metadataPath = moviePath + "\\" + Constants.METADATA_PATH;
            string metadataFile = metadataPath + "\\" + Constants.METADATA_FILE;

            if (File.Exists(metadataFile))
            {
                // Load the movie metadata from file and parse it, returning
                // the resulting movie object filled with metadata.
                XDocument metadata = XDocument.Load(metadataFile);

                return ParseMetadata(metadata);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Write metadata for movie to file.
        /// An exception is thrown if writing failed.
        /// </summary>
        /// <param name="moviePath">Directory containing movie to write metadata for.</param>
        /// <param name="movie">Movie object containing metadata to write.</param>
        private void WriteMovieMetadata(string moviePath, Movie movie)
        {
            string metadataPath = moviePath + "\\" + Constants.METADATA_PATH;
            string metadataFile = metadataPath + "\\" + Constants.METADATA_FILE;

            CreateMetadataDir(moviePath);

            XDocument metadata = ConstructMetadata(movie);
            metadata.Save(metadataFile);
        }

        private void CreateMetadataDir(string moviePath)
        {
            string metadataPath = moviePath + "\\" + Constants.METADATA_PATH;
            
            // Create the directory first if it doesn't exist.
            if (!Directory.Exists(metadataPath))
            {
                DirectoryInfo di = Directory.CreateDirectory(metadataPath);

                // Set directory as hidden.
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }
        }


        public IEnumerable<string> GetLibraryPaths()
        {
            return m_libraryPaths;
        }

        /// <summary>
        /// Parse associated movie metadata into a movie
        /// object. Throws an exception if there was a
        /// problem parsing the metadata.
        /// </summary>
        /// <param name="xdoc">Xml document containing metadata.</param>
        /// <returns>Movie object with parsed metadata.</returns>
        public static Movie ParseMetadata(XDocument xdoc)
        {
            Movie m = new Movie();

            try
            {
                m.ID = xdoc.Root.Element("id").Value;
                m.Name = xdoc.Root.Element("name").Value;
                m.Certification = xdoc.Root.Element("certification").Value;
                m.Overview = xdoc.Root.Element("overview").Value;
                m.Released = DateTime.Parse(xdoc.Root.Element("released").Value);
                m.Images = (from image in xdoc.Root.Element("images").Descendants()
                            select new MovieImage
                            {
                                type = (MovieImageType)Enum.Parse(typeof(MovieImageType), image.Attribute("type").Value, true),
                                path = image.Attribute("path").Value,
                                width = uint.Parse(image.Attribute("width").Value),
                                height = uint.Parse(image.Attribute("height").Value)
                            }).ToList();
            }
            catch (NullReferenceException)
            {
                // Re-throw a slightly more informative error message.
                throw new NullReferenceException("Required metadata element not found.");
            }

            return m;
        }

        /// <summary>
        /// Construct an XML document specifying metadata from a given
        /// movie object.
        /// </summary>
        /// <param name="m">Movie object containing metadata.</param>
        /// <returns>XML Document containing movie metadata.</returns>
        public static XDocument ConstructMetadata(Movie m)
        {
            // Construct the image elements first.
            XElement images = new XElement("images");
            foreach (MovieImage mi in m.Images)
            {
                XElement image = new XElement("image",
                    new XAttribute("type", mi.type.ToString()),
                    new XAttribute("path", mi.path),
                    new XAttribute("width", mi.width.ToString()),
                    new XAttribute("height", mi.height.ToString()));
                images.Add(image);
            }

            // Construct the xml document.
            XDocument xdoc = new XDocument(
                new XElement("movie",
                    new XElement("id", m.ID),
                    new XElement("name", m.Name),
                    new XElement("certification", m.Certification),
                    new XElement("overview", m.Overview),
                    new XElement("released", m.Released.ToString()),
                    images));

            return xdoc;
        }

    }
}
