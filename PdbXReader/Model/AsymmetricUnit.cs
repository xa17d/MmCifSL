using PdbXReader.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PdbXReader.Model
{
    /// <summary>
    /// Asymmetric Unit
    /// http://mmcif.wwpdb.org/dictionaries/mmcif_pdbx_v40.dic/Categories/struct_asym.html
    /// </summary>
    public class AsymmetricUnit : IModelId
    {
        public AsymmetricUnit(string id, Entity entity, string details)
        {
            this.Id = id;
            this.Entity = entity;
            this.Details = details;
        }

        public string Id { get; set; }
        public Entity Entity { get; set; }
        public string Details { get; set; }

        public override string ToString() { return ReflectionToString.ToString(this); }
    }
}
