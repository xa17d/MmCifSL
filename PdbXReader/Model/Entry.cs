using PdbXReader.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdbXReader.Model
{
    public class Entry : IModelId
    {
        public Entry()
        {
            this.Entities = new ModelCollection<Entity>();
            this.AsymetricUnits = new ModelCollection<AsymmetricUnit>();
            this.ChemicalComponents = new ModelCollection<ChemicalComponent>();
            this.AtomSites = new ModelCollection<AtomSite>();
        }

        public string Id { get; set; }

        public ModelCollection<Entity> Entities { get; private set; }
        public ModelCollection<AsymmetricUnit> AsymetricUnits { get; private set; }
        public ModelCollection<ChemicalComponent> ChemicalComponents { get; private set; }
        public ModelCollection<AtomSite> AtomSites { get; private set; }

        public override string ToString() { return ReflectionToString.ToString(this); }
    }
}
