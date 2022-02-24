using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Foundry_PF1_StatBlock_Exporter
{
    public class Converter
    {
        private string inputString;
        private string template;
        private string title;

        private dynamic input;

        private List<string> replacements = new List<string>();

        private static Regex tagRegex = new Regex("{{ .*? }}");

        public Converter(string inputString, string template, string title)
        {
            this.inputString = inputString;
            this.template = template;
            this.title = title;
            
            input = JsonConvert.DeserializeObject(inputString);
        }

        public string Convert()
        {
            MatchEvaluator matchEvaluator = new MatchEvaluator(ReplaceTag);

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
                string[] parts = tagContent[1..].Split(".");

                dynamic i = input;
                    
                foreach (string part in parts)
                {
                    i = i?[part];
                }

                if (i == null)
                {
                    i = "NULL";
                }
                    
                Console.WriteLine($"value: {i.ToString()}");
                return i;
            }

            return "NULL";
        }
    }
}