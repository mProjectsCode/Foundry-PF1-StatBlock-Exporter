using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Foundry_PF1_StatBlock_Exporter
{
    public static class JTokenHelper
    {
        public static JToken Traverse(this JToken token, string path)
        {
            return token.Traverse(path.Split("."));
        }

        public static JToken Traverse(this JToken token, IEnumerable<string> path)
        {
            foreach (string s in path)
            {
                if (token == null)
                {
                    return null;
                }

                if (s.StartsWith("$"))
                {
                    if (int.TryParse(s[1..], out int i))
                    {
                        token = token.GetPropertyIndex(i);
                    }
                    else
                    {
                        Console.WriteLine("Invalid Path");
                    }
                }
                else
                {
                    if (int.TryParse(s, out int i))
                    {
                        token = token.GetArray(i);
                    }
                    else
                    {
                        token = token.GetProperty(s);
                    }
                }
            }

            return token;
        }

        public static JToken GetProperty(this JToken token, string key)
        {
            return token?[key];
        }

        public static JToken GetArray(this JToken token, int index)
        {
            try
            {
                return token?[index];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static JToken GetPropertyIndex(this JToken token, int index)
        {
            try
            {
                JEnumerable<JToken>? children = token?.Children();

                if (children == null)
                {
                    return null;
                }

                int i = 0;
                foreach (JToken child in children)
                {
                    if (i == index)
                    {
                        return child.First;
                    }

                    i += 1;
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}