using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace metafetch.DataAccessors
{
    public interface MetadataAccessor
    {
        /// <summary>
        /// Search for a movie by title and year from the data source.
        /// This method should be thread-safe.
        /// </summary>
        /// <param name="title">Title to search for.</param>
        /// <param name="year">(Optional) Year to search for.</param>
        /// <returns>A list containing search results.</returns>
        IEnumerable<MovieSearchResult> SearchMovies(string title, Nullable<int> year);

        /// <summary>
        /// Fetch full movie metadata from the data source.
        /// This method should be thread-safe.
        /// </summary>
        /// <param name="id">ID of movie to fetch metadata for.</param>
        /// <param name="metadataPath">Location to store downloaded media.</param>
        /// <returns>A movie object containing the movie's metadata.</returns>
        Movie FetchMovie(string id, string metadataPath);

        /// <summary>
        /// Attempts to connect to the data source to see if a connection can
        /// be established. If the method returns without throwing an exception,
        /// the connection is suitable for use. If an exception is thrown, it is
        /// likely that any other calls will fail as well, which indicates either
        /// a host or client communication problem.
        /// </summary>
        void ConnectionTest();
    }
}
