using System.Collections.Generic;

namespace Foundry_PF1_StatBlock_Exporter.ComputedProperties
{
    public class HealthProperty : ComputedProperty
    {
        public override string GetProperty()
        {
            return $"{Input.Traverse("rollData.attributes.hp.max")} ({PropertyStore.Get("totalHd")}d{PropertyStore.Get("hdSize")}+{PropertyStore.Get("conHpBonus")})";
        }

        public override List<ComputedProperty> GetDependencies()
        {
            return new List<ComputedProperty>
            {
                PropertyStore.ComputedProperties["totalHd"],
                PropertyStore.ComputedProperties["hdSize"],
            };
        }
    }
}