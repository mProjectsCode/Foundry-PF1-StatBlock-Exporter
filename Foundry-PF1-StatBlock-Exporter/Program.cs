using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace Foundry_PF1_StatBlock_Exporter
{
    internal class Program
    {
        public static string RunLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string TemplateLocation = PathHelper.Combine(RunLocation, "template.txt");
        public static string InputFolder = PathHelper.Combine(RunLocation, "input");
        public static string OutputFolder = PathHelper.Combine(RunLocation, "output");
        
        static void Main(string[] args)
        {
            Convert();
        }

        public static void Convert()
        {
            string template = File.ReadAllText(TemplateLocation);
            
            string[] inputFiles = Directory.GetFiles(InputFolder);

            foreach (string inputFile in inputFiles)
            {
                Console.WriteLine($"InputFile: {inputFile}");
                string content = File.ReadAllText(inputFile);

                Converter converter = new Converter(content, template, "");
                File.WriteAllText(Path.Combine(OutputFolder, "out.md"), converter.Convert());
            }
        }
    }
}
