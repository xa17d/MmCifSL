using PdbXReader.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PdbXReader.Model
{
    public class EntryDetails
    {
        public override string ToString() { return ReflectionToString.ToString(this); }
    }
}
