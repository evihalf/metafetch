using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using metafetch.DataAccessors;
using metafetch.DataAccessors.TMDB;

namespace metafetch_unittests
{
    [TestClass]
    public class TMDBTest
    {
        private TMDBAccessor m_accessor;

        [TestInitialize]
        public void Initialize()
        {
            // Initialize data accessor with API key and default language code.
            m_accessor = new TMDBAccessor("1832136f1eaafcbfa0dec5c053322997", "en");
        }

        [TestMethod]
        public void TestConnection()
        {
            // No exceptions are thrown if the test was successful.
            m_accessor.ConnectionTest();
        }

        [TestMethod]
        public void TestSearch()
        {
            // Run a dummy search that should return at least one result, without
            // specifying a year.
            IEnumerable<MovieSearchResult> results;
            results = m_accessor.SearchMovies("transformers", null);

            Assert.IsTrue(results.Count() > 0);

            // Run another dummy search, specifying a year.
            results = m_accessor.SearchMovies("transformers", 2007);

            Assert.IsTrue(results.Count() > 0);
        }
    }
}
