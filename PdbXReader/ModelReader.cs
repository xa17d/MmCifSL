using MmCifSL;
using PdbXReader.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdbXReader
{
    public class ModelReader : IDisposable
    {
        public ModelReader()
        {

        }

        public void Dispose()
        {

        }

        public Entry ReadEntry(string cifContent)
        {
            var mmCifData = new MmCifParser().Parse(cifContent);

            Entry entry = new Entry();

            // Read Id
            entry.Id = mmCifData["_entry"][0]["id"];

            // Read Entities
            foreach (var mmCifEntity in mmCifData["_entity"])
            {
                var entity = new Entity(
                    mmCifEntity["id"],
                    ConvertToEntityType(mmCifEntity["type"]),
                    mmCifEntity["pdbx_description"]
                    );

                entry.Entities.Add(entity);
            }

            // Read Asymmetric Units
            foreach (var mmCifAsym in mmCifData["_struct_asym"])
            {
                var asymUnit = new AsymmetricUnit(
                    mmCifAsym["id"],
                    entry.Entities.GetById(mmCifAsym["entity_id"]),
                    mmCifAsym["details"]
                    );

                entry.AsymetricUnits.Add(asymUnit);
            }

            // Read Chemical Components
            foreach (var mmCifChemComp in mmCifData["_chem_comp"])
            {
                var chemComp = new ChemicalComponent(
                    mmCifChemComp["id"],
                    mmCifChemComp["type"],
                    mmCifChemComp["name"],
                    mmCifChemComp["formula"]
                    );

                entry.ChemicalComponents.Add(chemComp);
            }

            // Read Atom Sites
            foreach (var mmCifAtomSite in mmCifData["_atom_site"])
            {
                var atomSite = new AtomSite(
                    mmCifAtomSite["id"],
                    mmCifAtomSite["type_symbol"],
                    mmCifAtomSite["label_atom_id"],
                    entry.ChemicalComponents.GetById(mmCifAtomSite["label_comp_id"]),
                    entry.AsymetricUnits.GetById(mmCifAtomSite["label_asym_id"]),
                    entry.Entities.GetById(mmCifAtomSite["label_entity_id"]),
                    mmCifAtomSite["label_seq_id"],
                    ConvertToFloat(mmCifAtomSite["Cartn_x"]),
                    ConvertToFloat(mmCifAtomSite["Cartn_y"]),
                    ConvertToFloat(mmCifAtomSite["Cartn_z"])
                    );

                entry.AtomSites.Add(atomSite);
            }

            return entry;
        }

        private EntityType ConvertToEntityType(string v)
        {
            var vNorm = v.ToLower().Replace(" ", "").Replace("-", "");
            switch (vNorm)
            {
                case "polymer": return EntityType.Polymer;
                case "nonpolymer": return EntityType.NonPolymer;
                case "water": return EntityType.Water;
                default:
                    throw new ArgumentException("Invalid entity type: " + v);
            }
        }


        private float ConvertToFloat(string v)
        {
            return float.Parse(v, CultureInfo.InvariantCulture);
        }
    }
}
