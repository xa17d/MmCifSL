using PdbXReader.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdbXReader.Model
{
    /// <summary>
    /// Molecular Entity
    /// http://mmcif.wwpdb.org/dictionaries/mmcif_pdbx_v40.dic/Categories/entity.html
    /// </summary>
    public class Entity : IModelId
    {
        public Entity(string id, EntityType type, string description)
        {
            this.Id = id;
            this.Type = type;
            this.Description = description;
        }

        public string Id { get; private set; }
        public EntityType Type { get; private set; }
        public string Description { get; private set; }

        public override string ToString() { return ReflectionToString.ToString(this); }
    }
}
