using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Foundry_PF1_StatBlock_Exporter.ComputedProperties
{
    public class PropertyStore
    {
        public static readonly Dictionary<string, ComputedProperty> ComputedProperties = new Dictionary<string, ComputedProperty>
        {
            {"totalHd", new TotalHdProperty()},
            {"health", new HealthProperty()},
            {"class", new ClassProperty()},
            {"hdSize", new HdSizeProperty()},
            {"conHpBonus", new ConHpBonusProperty()},
        };

        private readonly Dictionary<string, string> _properies = new Dictionary<string, string>();

        public PropertyStore(JToken input)
        {
            foreach (ComputedProperty computedProperty in ComputedProperties.Values)
            {
                computedProperty.Input = input;
                computedProperty.PropertyStore = this;
            }

            CalculateDependencyDepthForTree();

            Dictionary<string, ComputedProperty> orderedComputedProperties = ComputedProperties.OrderBy(x => x.Value.DependencyDepth).ToDictionary(x => x.Key, x => x.Value);

            foreach ((string key, ComputedProperty value) in orderedComputedProperties)
            {
                _properies.Add(key, value.GetProperty());
            }
        }

        public string Get(string property)
        {
            return _properies.ContainsKey(property) ? _properies[property] : "NULL";
        }

        public void CalculateDependencyDepthForTree()
        {
            foreach (ComputedProperty computedProperty in ComputedProperties.Values)
            {
                computedProperty.DependencyDepth = -1;
            }

            foreach (ComputedProperty computedProperty in ComputedProperties.Values)
            {
                CalculateDependencyDepth(computedProperty, 0);
            }

            ComputedProperty p = ComputedProperties.Values.First();
            foreach (ComputedProperty computedProperty in ComputedProperties.Values)
            {
                if (computedProperty.DependencyDepth > p.DependencyDepth)
                {
                    p = computedProperty;
                }
            }

            UpdateDependencyDepth(p, p.DependencyDepth);

            foreach (ComputedProperty computedProperty in ComputedProperties.Values)
            {
                Console.WriteLine($"Name: {computedProperty.GetType().Name}, depth: {computedProperty.DependencyDepth}");
            }
        }

        private int CalculateDependencyDepth(ComputedProperty computedProperty, int iterations)
        {
            if (iterations > ComputedProperties.Count)
            {
                throw new Exception("loop detected");
            }

            if (computedProperty.DependencyDepth == -1)
            {
                int depth = 0;

                foreach (ComputedProperty dependency in computedProperty.GetDependencies())
                {
                    iterations += 1;

                    depth = Math.Max(depth, CalculateDependencyDepth(dependency, iterations));
                }

                computedProperty.DependencyDepth = depth + 1;
            }

            return computedProperty.DependencyDepth;
        }

        private void UpdateDependencyDepth(ComputedProperty computedProperty, int depth)
        {
            computedProperty.DependencyDepth = depth;

            foreach (ComputedProperty dependency in computedProperty.GetDependencies())
            {
                UpdateDependencyDepth(dependency, depth - 1);
            }
        }
    }
}