using System.Collections.Generic;

namespace Foundry_PF1_StatBlock_Exporter.ComputedProperties
{
    public class TotalHdProperty : ComputedProperty
    {
        public override string GetProperty()
        {
            return Input.Traverse("rollData.attributes.hd.total")?.ToString();
        }

        public override List<ComputedProperty> GetDependencies()
        {
            return new List<ComputedProperty>();
        }
    }
}