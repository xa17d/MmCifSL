using PdbXReader.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PdbXReader.Model
{
    /// <summary>
    /// http://mmcif.wwpdb.org/dictionaries/mmcif_pdbx_v40.dic/Categories/pdbx_struct_oper_list.html
    /// </summary>
    public class StructuralOperation
    {
        // TODO

        public override string ToString() { return ReflectionToString.ToString(this); }
    }
}
