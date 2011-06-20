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
