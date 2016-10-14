using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MmCifSL
{
    public class MmCifParseException : Exception
    {
        public MmCifParseException(string message) : base(message)
        {
        }
    }
}
