using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Foundry_PF1_StatBlock_Exporter.ComputedProperties;
using Newtonsoft.Json.Linq;

namespace Foundry_PF1_StatBlock_Exporter
{
    public class Converter
    {
        private static readonly Regex tagRegex = new Regex("{{ .*? }}");

        private readonly JObject input;
        private readonly string template;
        private string inputString;
        private readonly PropertyStore propertyStore;

        private List<string> replacements = new List<string>();
        private string title;

        public Converter(string inputString, string templateNpc, string templateCharacter, string title)
        {
            this.inputString = inputString;
            this.title = title;

            input = JObject.Parse(inputString);

            if (input?["data"]?["type"]?.Value<string>() == "npc")
            {
                template = templateNpc;
            }

            if (input?["data"]?["type"]?.Value<string>() == "character")
            {
                template = templateCharacter;
            }

            propertyStore = new PropertyStore(input);
        }

        public string Convert()
        {
            if (template == null)
            {
                return "";
            }

            MatchEvaluator matchEvaluator = ReplaceTag;

            return tagRegex.Replace(template, matchEvaluator);
        }

        private string ReplaceTag(Match match)
        {
            string tag = match.Value;
            string tagContent = tag[3..];
            tagContent = tagContent[..^3];
            Console.WriteLine($"tag content : {tagContent}");

            if (tagContent.StartsWith("@"))
            {
                JToken i = input.Traverse(tagContent[1..]) ?? "NULL";

                Console.WriteLine($"value: {i}");
                return i.ToString();
            }

            if (tagContent.StartsWith("#"))
            {
                Console.WriteLine($"value: {propertyStore.Get(tagContent[1..])}");
                return propertyStore.Get(tagContent[1..]);
            }

            return "NULL";
        }
    }
}