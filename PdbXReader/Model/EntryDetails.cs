using PdbXReader.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdbXReader.Model
{
    public class EntryDetails
    {
        public override string ToString() { return ReflectionToString.ToString(this); }
    }
}
