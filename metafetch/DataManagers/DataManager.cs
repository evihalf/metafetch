using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using metafetch.DataAccessors;

namespace metafetch.DataManagers
{
    /// <summary>
    /// Defines an common control interface among data managers.
    /// </summary>
    public interface DataManager
    {
        void Add(MovieEntry entry);
        void Clear(MovieEntry entry);
    }
}
