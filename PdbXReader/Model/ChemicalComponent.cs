using PdbXReader.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PdbXReader.Model
{
    /// <summary>
    /// Chemical Component
    /// http://mmcif.wwpdb.org/dictionaries/mmcif_pdbx_v40.dic/Categories/chem_comp.html
    /// </summary>
    public class ChemicalComponent : IModelId
    {
        public ChemicalComponent(string id, string type, string name, string formula)
        {
            this.Id = id;
            this.Type = type;
            this.Name = name;
            this.Formula = formula;
        }

        public string Id { get; private set; }
        public string Type { get; private set; }
        public string Name { get; private set; }
        public string Formula { get; private set; }

        public override string ToString() { return ReflectionToString.ToString(this); }
    }
}
