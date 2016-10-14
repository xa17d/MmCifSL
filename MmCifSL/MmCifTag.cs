using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MmCifSL
{
    class MmCifTag
    {
        public MmCifTag(string tag)
        {
            var parts = tag.Split(new char[] { '.' }, 2, StringSplitOptions.None);
            Category = parts[0];
            Attribute = parts[1];
        }

        public string Category { get; private set; }
        public string Attribute { get; private set; }
    }
}
