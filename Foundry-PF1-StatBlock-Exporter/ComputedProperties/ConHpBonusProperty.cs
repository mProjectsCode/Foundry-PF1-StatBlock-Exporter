using System.Collections.Generic;

namespace Foundry_PF1_StatBlock_Exporter.ComputedProperties
{
    public class ConHpBonusProperty : ComputedProperty
    {
        public override string GetProperty()
        {
            return ((int?) Input.Traverse("rollData.attributes.hp.max") - (int?) Input.Traverse("rollData.attributes.vigor.max")).ToString();
        }

        public override List<ComputedProperty> GetDependencies()
        {
            return new List<ComputedProperty>();
        }
    }
}