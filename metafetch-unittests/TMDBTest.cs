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
