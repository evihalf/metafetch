using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Net;

namespace metafetch.DataAccessors.TMDB
{
    public class TMDBAccessor : MetadataAccessor
    {
        public const string TMDB_BASE_URL = "http://api.themoviedb.org/2.1/";
        public const string TMDB_COMMAND_URL = TMDB_BASE_URL + "{cmd}/{lang}/xml/{key}/";
        public const int FETCH_TIMEOUT = 10 * 1000; // 10 seconds.

        private string m_apiKey;
        private string m_language;

        public TMDBAccessor(string apiKey, string language)
        {
            m_apiKey = apiKey;
            m_language = language;
        }

        public IEnumerable<MovieSearchResult> SearchMovies(string title, Nullable<int> year)
        {
            // Query TMDb for movies with the given title and year.
            string fetchUrl = TMDB_COMMAND_URL;
            fetchUrl = fetchUrl.Replace("{cmd}", "Movie.search");
            fetchUrl = fetchUrl.Replace("{lang}", m_language);
            fetchUrl = fetchUrl.Replace("{key}", m_apiKey);

            if (year != null)
                fetchUrl = fetchUrl + title + "+" + year.ToString();
            else
                fetchUrl = fetchUrl + title;

            XDocument searchPage = XDocument.Load(fetchUrl);

            List<MovieSearchResult> results = new List<MovieSearchResult>();
            foreach (XElement x in searchPage.Root.Elements("movies").Descendants("movie"))
            {
                MovieSearchResult result = new MovieSearchResult();
                result.ID = x.Element("id").Value;
                result.Title = x.Element("name").Value;

                result.Released = null;
                string released = x.Element("released").Value;

                if (!string.IsNullOrEmpty(released))
                    result.Released = DateTime.Parse(released);

                results.Add(result);
            }

            return results;
        }

        public Movie FetchMovie(string id, string metadataPath)
        {
            // Query TMDB for movie info by ID.
            Movie m = new Movie();

            string fetchUrl = TMDB_COMMAND_URL;
            fetchUrl = fetchUrl.Replace("{cmd}", "Movie.getInfo");
            fetchUrl = fetchUrl.Replace("{lang}", m_language);
            fetchUrl = fetchUrl.Replace("{key}", m_apiKey);
            fetchUrl += id;

            // Fetch and parse results.
            XDocument infoPage = XDocument.Load(fetchUrl);
            XElement movieElem = infoPage.Root.Element("movies").Element("movie");
            m.ID = movieElem.Element("id").Value;
            m.Name = movieElem.Element("name").Value;
            m.Certification = movieElem.Element("certification").Value;
            m.Overview = movieElem.Element("overview").Value;
            m.Genre = movieElem.Element("categories").Descendants("category").First().Attribute("name").Value;
            m.Runtime = int.Parse(movieElem.Element("runtime").Value);
            
            m.Released = null;
            string released = movieElem.Element("released").Value;
            if (!string.IsNullOrEmpty(released))
                m.Released = DateTime.Parse(released);

            // Fetch first image specified.
            if (movieElem.Descendants("images").Count() > 0)
            {
                XElement imageElem = movieElem.Element("images").Descendants().First();

                // Download the image to the movie's metadata directory.
                string url = imageElem.Attribute("url").Value;
                string path = metadataPath + "\\" + url.Substring(url.LastIndexOf('/') + 1);

                WebClient wc = new WebClient();
                wc.DownloadFile(url, path);

                MovieImage mi = new MovieImage();
                mi.type = (MovieImageType)Enum.Parse(typeof(MovieImageType), imageElem.Attribute("type").Value, true);
                mi.width = uint.Parse(imageElem.Attribute("width").Value);
                mi.height = uint.Parse(imageElem.Attribute("height").Value);
                mi.path = path;
                m.Images.Add(mi);
            }

            return m;
        }

        public void ConnectionTest()
        {
            // Fetch the main API page to test the connection to TMDb.
            string fetchUrl = TMDB_BASE_URL;

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(fetchUrl);
            request.Timeout = FETCH_TIMEOUT;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
                throw new WebException("Expected response was '" + HttpStatusCode.OK.ToString() + "', got '" + response.StatusCode.ToString() + "'.");
        }
    }
}
