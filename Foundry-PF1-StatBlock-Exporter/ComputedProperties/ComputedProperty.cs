using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Foundry_PF1_StatBlock_Exporter.ComputedProperties
{
    public abstract class ComputedProperty
    {
        public JToken Input { get; set; }

        public PropertyStore PropertyStore { get; set; }

        public int DependencyDepth { get; set; } = -1;

        public virtual string GetProperty()
        {
            return "NULL";
        }

        public virtual List<ComputedProperty> GetDependencies()
        {
            return new List<ComputedProperty>();
        }
    }
}