using PdbXReader.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdbXReader.Model
{
    /// <summary>
    /// http://mmcif.wwpdb.org/dictionaries/mmcif_pdbx_v40.dic/Categories/pdbx_struct_assembly.html
    /// </summary>
    public class StructuralAssembly
    {
        // TODO

        public override string ToString() { return ReflectionToString.ToString(this); }
    }
}
