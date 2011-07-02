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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace metafetch.DataAccessors
{
    public sealed class Constants
    {
        public const string METADATA_PATH = ".metafetch";
        public const string METADATA_FILE = "metadata.xml";
    }

    public struct MovieSearchResult
    {
        public string ID;
        public string Title;
        public DateTime? Released;
    }

    public enum MovieImageType
    {
        Poster,
        Backdrop
    }

    public struct MovieImage
    {
        public MovieImageType type;
        public string path;
        public uint width;
        public uint height;
    }

    /// <summary>
    /// Encapsulates metadata of a movie.
    /// </summary>
    public class Movie
    {
        public Movie()
        {
            ID = "";
            Name = "";
            Certification = "";
            Overview = "";
            Genre = "";
            Runtime = 0;
            Released = new DateTime();
            Images = new List<MovieImage>();
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string Certification { get; set; }
        public string Overview { get; set; }
        public string Genre { get; set; }
        public int Runtime { get; set; }
        public DateTime? Released { get; set; }
        public List<MovieImage> Images { get; set; }

    }

    /// <summary>
    /// Encapsulates information about a movie entry in a library.
    /// </summary>
    public class MovieEntry
    {
        public Movie movie;
        public string movieTag;
        public string moviePath;
        public string libraryPath;
        public string manualID;
        public Exception loadException;

        public MovieEntry()
        {
            movie = null;
            movieTag = null;
            moviePath = null;
            libraryPath = null;
            manualID = null;
            loadException = null;
        }
    }

    /// <summary>
    /// Collection of movie entries, with an index on the movie path.
    /// </summary>
    public class MovieLibrary : KeyedCollection<string, MovieEntry>
    {
        public MovieLibrary() : base()
        {
        }

        protected override string GetKeyForItem(MovieEntry item)
        {
            return item.moviePath;
        }
    }
}
