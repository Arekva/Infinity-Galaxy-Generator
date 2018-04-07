// Script for planet texture exporting
//
// Using PlanetaryProcessor, made by Thomas P.; big thanks to him
// for having made this tool for Infinity.
//
// Also, this means that PlanetaryProcessor.dll is under his licensing,
// see his GitHub for more details.

using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ConfigNodeParser;
using PlanetaryProcessor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Infinity.Generators
{
    class PlanetMaps
    {
        public static void Save(string[] args, string gameDataPath, string planetName, Dictionary<string, string> settings, Dictionary<string, Dictionary<string, string>> pqsMods)
        {
            gameDataPath = gameDataPath.Replace(@"\GameData\Infinity", "");
            Run(args, gameDataPath, planetName, settings, pqsMods).Wait();
        }
        public static async Task Run(string[] args, string gameDataPath, string planetName, Dictionary<string, string> settings, Dictionary<string, Dictionary<string, string>> pqsMods)
        {
            

            NodeTree config = new NodeTree();

            foreach (KeyValuePair<string, string> setting in settings)
            {
                config.SetValue(setting.Key, setting.Value);
            }

            NodeTree mods = config.AddNode("Mods");

            foreach (KeyValuePair<string, Dictionary<string, string>> mod in pqsMods)
            {
                NodeTree modNode = mods.AddNode(mod.Key);
                
                foreach (KeyValuePair<string, string> parameter in mod.Value)
                {
                    modNode.SetValue(parameter.Key, parameter.Value);
                }
            }

            Console.WriteLine("\nGenerating maps.. This action can take a while, especially with high resolution\n");

            using (Processor processor = await Processor.Create(gameDataPath))
            {
                
                Console.WriteLine("\nExporting maps..");

                Processor.EncodedTextureData data = await processor.GenerateMapsEncoded(config);

                await SaveStream(gameDataPath + @"GameData\Infinity\StarSystems\Planets\" + planetName + "_Color.png", data.Color);
                await SaveStream(gameDataPath + @"GameData\Infinity\StarSystems\Planets\" + planetName + "_Height.png", data.Height);
                await SaveStream(gameDataPath + @"GameData\Infinity\StarSystems\Planets\" + planetName + "_Normal.png", data.Normal);
                Console.Clear();
            }
        }

        public static async Task SaveStream(String file, Stream stream)
        {
            using (stream)
            {
                using (FileStream fs = File.OpenWrite(file))
                {
                    await stream.CopyToAsync(fs);
                }
            }
        }
    }
}
