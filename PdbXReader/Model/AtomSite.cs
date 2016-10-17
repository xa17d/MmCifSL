using PdbXReader.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PdbXReader.Model
{
    public class AtomSite : IModelId
    {
        public AtomSite(string id, string symbol, string atomId, ChemicalComponent component, AsymmetricUnit asymmetricUnit, Entity entity, string sequenceId, float x, float y, float z)
        {
            this.Id = id;
            this.Symbol = symbol;
            this.AtomId = atomId;
            this.Component = component;
            this.AsymmetricUnit = asymmetricUnit;
            this.Entity = entity;
            this.SequenceId = sequenceId;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public string Id { get; private set; }
        public string Symbol { get; private set; }
        public string AtomId { get; private set; }
        public ChemicalComponent Component { get; private set; }
        public AsymmetricUnit AsymmetricUnit { get; private set; }
        public Entity Entity { get; private set; }
        public string SequenceId { get; private set; }

        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        public override string ToString() { return ReflectionToString.ToString(this); }
    }
}
