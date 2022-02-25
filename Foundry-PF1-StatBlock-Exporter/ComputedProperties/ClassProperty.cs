using System.Collections.Generic;

namespace Foundry_PF1_StatBlock_Exporter.ComputedProperties
{
    public class ClassProperty : ComputedProperty
    {
        public override string GetProperty()
        {
            return Input.Traverse("rollData.classes.$0.name")?.ToString();
        }

        public override List<ComputedProperty> GetDependencies()
        {
            return new List<ComputedProperty>();
        }
    }
}