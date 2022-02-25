using System.Collections.Generic;

namespace Foundry_PF1_StatBlock_Exporter.ComputedProperties
{
    public class HdSizeProperty : ComputedProperty
    {
        public override string GetProperty()
        {
            return Input.Traverse("rollData.classes.$0.hd")?.ToString();
        }

        public override List<ComputedProperty> GetDependencies()
        {
            return new List<ComputedProperty>();
        }
    }
}