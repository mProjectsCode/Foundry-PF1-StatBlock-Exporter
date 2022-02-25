using System;
using System.IO;
using System.Reflection;

namespace Foundry_PF1_StatBlock_Exporter
{
    internal class Program
    {
        public static string RunLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string TemplateNpcLocation = PathHelper.Combine(RunLocation, "templateNpc.txt");
        public static string TemplateCharacterLocation = PathHelper.Combine(RunLocation, "templateCharacter.txt");
        public static string InputFolder = PathHelper.Combine(RunLocation, "input");
        public static string OutputFolder = PathHelper.Combine(RunLocation, "output");

        private static void Main(string[] args)
        {
            Convert();
        }

        public static void Convert()
        {
            if (!Directory.Exists(InputFolder))
            {
                Directory.CreateDirectory(InputFolder);
            }

            if (!Directory.Exists(OutputFolder))
            {
                Directory.CreateDirectory(OutputFolder);
            }

            string templateNpc = File.ReadAllText(TemplateNpcLocation);
            string templateCharacter = File.ReadAllText(TemplateCharacterLocation);

            string[] inputFiles = Directory.GetFiles(InputFolder);

            foreach (string inputFile in inputFiles)
            {
                Console.WriteLine($"InputFile: {inputFile}");
                string content = File.ReadAllText(inputFile);

                Converter converter = new Converter(content, templateNpc, templateCharacter, "");
                File.WriteAllText(Path.Combine(OutputFolder, Path.GetFileNameWithoutExtension(inputFile) + ".md"), converter.Convert());
            }
        }
    }
}