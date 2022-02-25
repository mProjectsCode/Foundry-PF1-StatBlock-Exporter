using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Foundry_PF1_StatBlock_Exporter
{
    public class Converter
    {
        private string inputString;
        private string template;
        private string title;

        private JObject input;

        private List<string> replacements = new List<string>();

        private static Regex tagRegex = new Regex("{{ .*? }}");

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
        }

        public string Convert()
        {
            if (template == null)
            {
                return "";
            }
            
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

                JToken i = input;
                    
                foreach (string part in parts)
                {
                    if (part.StartsWith("$"))
                    {
                        int a = 0;
                        if (int.TryParse(part[1..], out a))
                        {
                            try
                            {
                                JEnumerable<JToken>? children = i?.Children();
                                
                                if (children == null)
                                {
                                    break;
                                }
                                
                                int j = 0;
                                foreach (JToken child in children)
                                {
                                    if (j == a)
                                    {
                                        i = child.First;
                                        break;
                                    }

                                    j += 1;
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("error");
                                i = null;
                            }
                        }
                    }
                    else
                    {
                        int a = 0;
                        if (int.TryParse(part, out a))
                        {
                            try
                            {
                                i = i?[a];
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("error");
                                i = null;
                            }
                        }
                        else
                        {
                            try
                            {
                                i = i?[part];
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("error");
                                i = null;
                            }
                        }
                    }
                    
                }

                if (i == null)
                {
                    i = "NULL";
                }
                    
                Console.WriteLine($"value: {i.ToString()}");
                return i.ToString();
            }

            return "NULL";
        }
    }
}