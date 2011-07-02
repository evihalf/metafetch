using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace metafetch
{
    public class NoResultsFoundException : Exception
    {
        public NoResultsFoundException(string message) : base(message)
        {
        }
    }

}
