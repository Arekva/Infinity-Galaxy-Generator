using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Infinity.Datas;
using System.Reflection;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;
using Infinity.Datas.Body;
using ConfigNodeParser;

namespace Infinity.Generators
{
    class Galaxy
    {
        /// <summary>
        /// Generates a planet
        /// </summary>
        public static void Planet(int starCount, int planetID, string gameDataPath, Random random)
        {
            Dictionary<string, Dictionary<string, string>> configFile = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> processorSettings = new Dictionary<string, string>();
            Dictionary<string, Dictionary<string, string>> pqsMods = new Dictionary<string, Dictionary<string, string>>();

            //====PQS things===========================//
            double mainColorR = random.NextDouble();
            double mainColorG = random.NextDouble();
            double mainColorB = random.NextDouble();

            double secondaryColorR = random.NextDouble();
            double secondaryColorG = random.NextDouble();
            double secondaryColorB = random.NextDouble();
            //=========================================//

            //====Keys and values for the body node====//
            Body body = new Body();

            Dictionary<string, string> bodyNode = new Dictionary<string, string>();
            body.Name = "Star " + starCount + " Planet N." + planetID;
            body.CacheFile = @"Infinity/StarSystems/Cache/" + "Star " + starCount + " Planet N." + planetID + ".bin";

            bodyNode.Add("name", body.Name);
            bodyNode.Add("cacheFile", body.CacheFile);

            configFile.Add("Body", bodyNode);
            //=========================================//

            //====Keys and values for properties node==//
            Dictionary<string, string> propertiesNode = new Dictionary<string, string>();

            double minRadius = 25000;
            double maxRadius = 1500000;
            double radius = random.NextDouble() * (minRadius - maxRadius) + maxRadius;
            bool isHomeWorld = false;

            propertiesNode.Add("radius", radius.ToString());
            propertiesNode.Add("density", 5.515e+3.ToString()); //Yes, fact is the real name of this is volumic mass and not density.. ikr | I take Earth's one for testing issues.
            propertiesNode.Add("isHomeWorld", isHomeWorld.ToString());

            configFile.Add("Properties", propertiesNode);
            //=========================================//

            //==Keys and values for the template node==//
            Template template = new Template();

            Dictionary<string, string> templateNode = new Dictionary<string, string>();
            template.Name = Datas.Enums.Template.Tylo;
            template.removeAllPQSMods = true;
            template.removeOcean = true;

            templateNode.Add("name", template.Name.ToString());
            templateNode.Add("removeAllPQSMods", template.removeAllPQSMods.ToString());
            templateNode.Add("removeOcean", template.removeOcean.ToString());

            configFile.Add("Template", templateNode);
            //=========================================//

            //====Keys and values for the orbit node===//
            Dictionary<string, double> orbitD = Orbit.Planet(random);

            string referenceBody = "Star " + starCount.ToString();
            string orbitColor = mainColorR + ", " + mainColorG + ", " + mainColorB + ", 1";

            Dictionary<string, string> orbitNode = new Dictionary<string, string>();
            foreach (KeyValuePair<string, double> param in orbitD)
            {
                orbitNode.Add(param.Key, param.Value.ToString());
            }

            orbitNode.Add("referenceBody", referenceBody);
            orbitNode.Add("color", orbitColor);

            configFile.Add("Orbit", orbitNode);
            //=========================================//

            //==Keys and value for ScaledVersion nodes=//
            ScaledVersion scaledVersion = new ScaledVersion();

            Dictionary<string, string> scaledVersionNode = new Dictionary<string, string>();
            Dictionary<string, string> scaledVersionMaterialNode = new Dictionary<string, string>();

            scaledVersion.Type = Datas.Enums.Body.ScaledVersionTypes.Vacuum;
            scaledVersion.Texture = @"Infinity\StarSystems\Planets\Star " + starCount + " Planet N." + planetID + "_Color.png";
            scaledVersion.Normals = @"Infinity\StarSystems\Planets\Star " + starCount + " Planet N." + planetID + "_Normal.png";

            scaledVersionNode.Add("type", scaledVersion.Type.ToString());
            scaledVersionMaterialNode.Add("texture", scaledVersion.Texture.ToString());
            scaledVersionMaterialNode.Add("normals", scaledVersion.Normals.ToString());

            configFile.Add("ScaledVersion", scaledVersionNode);
            configFile.Add("SVMaterial", scaledVersionMaterialNode);
            //=========================================//

            //====PQS Nodes keys and values============//
            string modName = "";

            Dictionary<string, string> VertexSimplexHeightAbsolute = new Dictionary<string, string>();

            modName = "VertexSimplexHeightAbsolute";

            VertexSimplexHeightAbsolute.Add("deformity", "8000");
            VertexSimplexHeightAbsolute.Add("frequency", "10");
            VertexSimplexHeightAbsolute.Add("octaves", "3");
            VertexSimplexHeightAbsolute.Add("persistence", "0.5");
            VertexSimplexHeightAbsolute.Add("seed", "645546");
            VertexSimplexHeightAbsolute.Add("order", "10");
            VertexSimplexHeightAbsolute.Add("enabled", "True");

            pqsMods.Add(modName, VertexSimplexHeightAbsolute);

            Dictionary<string, string> VertexSimplexNoiseColor = new Dictionary<string, string>();

            modName = "VertexSimplexNoiseColor";

            string colorStart = orbitColor;
            string colorEnd = secondaryColorR + ", " + secondaryColorG + ", " + secondaryColorB + ", 1";
            VertexSimplexNoiseColor.Add("blend", "1");
            VertexSimplexNoiseColor.Add("colorStart", colorStart);
            VertexSimplexNoiseColor.Add("colorEnd", colorEnd);
            VertexSimplexNoiseColor.Add("frequency", "1");
            VertexSimplexNoiseColor.Add("octaves", "8");
            VertexSimplexNoiseColor.Add("persistence", "0.5");
            VertexSimplexNoiseColor.Add("seed", random.Next(int.MinValue, int.MaxValue).ToString());
            VertexSimplexNoiseColor.Add("order", "100");
            VertexSimplexNoiseColor.Add("enabled", "True");

            pqsMods.Add(modName, VertexSimplexNoiseColor);
            //=========================================//

            //====Processor settings===================//
            double res = 1024;

            double lowRes = 512;
            double lowResRad = 135000;

            double midRes = 1024;
            double midResRad = 300000;

            double highRes = 2048;
            double highResRad = 750000;

            double ultraRes = 4096;
            double ultraResRad = maxRadius;

            if (radius <= lowResRad) res = lowRes;
            if (radius > lowResRad && radius <= midResRad) res = midRes;
            if (radius > midResRad && radius <= highResRad) res = highRes;
            if (radius > highResRad && radius <= ultraRes) res = ultraRes;

            processorSettings.Add("__resolution", res.ToString());
            processorSettings.Add("__radius", radius.ToString());
            processorSettings.Add("__hasOcean", "false");
            processorSettings.Add("__oceanHeight", "0");
            processorSettings.Add("__oceanColor", "0,0,0,0");
            processorSettings.Add("__normalStrength", "7");
            processorSettings.Add("mapMaxHeight", "9000");
            //=========================================//

            //Makes and saves config file
            ConfigNode conf = PlanetConfig(configFile, pqsMods);
            conf.Save(gameDataPath + @"StarSystems/Planets/Star " + starCount + " Planet N." + planetID + ".cfg");

            //Makes and saves maps
            string[] args =  { "" };
            PlanetMaps.Save(args, gameDataPath, body.Name, processorSettings, pqsMods);
        }

        /// <summary>
        /// Generates the planet config
        /// </summary>
        public static ConfigNode PlanetConfig(Dictionary<string, Dictionary<string, string>> configFile, Dictionary<string, Dictionary<string, string>> pqsMods)
        {
            ConfigNode MMNode = new ConfigNode("@Kopernicus:FOR[INFINITY]");
            ConfigNode BodyNode = new ConfigNode("Body");
            ConfigNode standardNode = new ConfigNode();
            ConfigNode standardSubNode = new ConfigNode();
            ConfigNode scaledVersionNode = new ConfigNode();
            ConfigNode PQSModNode = new ConfigNode();

            ConfigNode wrapper = new ConfigNode();

            wrapper.AddConfigNode(MMNode);

            foreach(KeyValuePair<string, Dictionary<string, string>> category in configFile)
            {
                if (category.Key == "Body")
                {
                    foreach (KeyValuePair<string, string> value in category.Value)
                    {
                        BodyNode.AddValue(value.Key, value.Value);
                    }

                    MMNode.AddConfigNode(BodyNode);
                }

                else
                {
                    
                    if (category.Key != "SVMaterial")
                    {
                        if (category.Key == "ScaledVersion")
                        {
                            scaledVersionNode = new ConfigNode(category.Key);

                            foreach (KeyValuePair<string, string> value in category.Value)
                            {
                                scaledVersionNode.AddValue(value.Key, value.Value);
                            }

                            BodyNode.AddConfigNode(scaledVersionNode);
                        }
                        else
                        {
                            standardNode = new ConfigNode(category.Key);

                            foreach (KeyValuePair<string, string> value in category.Value)
                            {
                                standardNode.AddValue(value.Key, value.Value);
                            }

                            BodyNode.AddConfigNode(standardNode);
                        }
                    }
                    
                    else
                    {
                        standardSubNode = new ConfigNode("Material");

                        foreach (KeyValuePair<string, string> value in category.Value)
                        {
                            standardSubNode.AddValue(value.Key, value.Value);
                        }

                        scaledVersionNode.AddConfigNode(standardSubNode);
                    }
                }
            }

            PQSModNode = new ConfigNode("Mods");

            foreach (KeyValuePair<string, Dictionary<string, string>> mod in pqsMods)
            {
                standardNode = new ConfigNode(mod.Key);
                foreach(KeyValuePair<string, string> parameter in mod.Value)
                {
                    standardNode.AddValue(parameter.Key, parameter.Value);
                }

                PQSModNode.AddConfigNode(standardNode);
            }

            standardNode = new ConfigNode("PQS");
            standardNode.AddConfigNode(PQSModNode);
            BodyNode.AddConfigNode(standardNode);

            return wrapper;
        }

        /// <summary>
        ///Generates a random star
        /// </summary>
        public static Dictionary<string, Dictionary<string, string>> Star(
            Dictionary<string, Dictionary<string, string>> starDatabase, Dictionary<string, double> galaxySettings,
            string gameDataPath, Random random)//galaxySetings is used for the orbit generator
        {
            //Star's dictionary
            Dictionary<string, Dictionary<string, string>> Star = new Dictionary<string, Dictionary<string, string>>();

            //Generates the orbit
            Dictionary<string, double> orbitD = Orbit.Star(galaxySettings, random);
            Dictionary<string, string> orbitS = new Dictionary<string, string>();
            foreach (KeyValuePair<string, double> key in orbitD) orbitS.Add(key.Key, Convert.ToString(key.Value));

            //Generates the body
            Dictionary<string, string> body = StarGenerator.Generate(starDatabase, gameDataPath, random);

            //Generates light curves
            double lum = 0;
            Double.TryParse(body["LUMINOSITY"], out lum);
            double rad = 0;
            Double.TryParse(body["RADIUS"], out rad);

            Dictionary<string, double> lightcurvesD = StarLightCurves(lum, rad);
            Dictionary<string, string> lightCurvesS = new Dictionary<string, string>();
            foreach (KeyValuePair<string, double> key in lightcurvesD) lightCurvesS.Add(key.Key, Convert.ToString(key.Value));

            Star.Add("Orbit", orbitS);
            Star.Add("Body", body);
            Star.Add("LightCurves", lightCurvesS);

            return Star;
        }

        /// <summary>
        /// Generates light curves for star
        /// </summary>
        public static Dictionary<string, double> StarLightCurves(double lum, double rad)
        {
            if (lum < 1) lum = 1;
            Dictionary<string, double> keys = new Dictionary<string, double>();

            //brightnessCurve aka sunflare size curve
            //====Variables====//
            double[] BCVars = { 0.5, 0.9, 10 };
            string[] BCNames = { "KEYBC1X", "KEYBC2X", "KEYBC3X" };
            double kRad;
            //=================//

            //====Calculation of the curve====//
            kRad = rad * 6.957e+8; //Rad in meters
            kRad /= 3; //KSP Sized
            kRad /= 261600000; //Obtains percentage of kerbol's radius

            //Calculates and adds to the dictionary
            for (int i = 0; i < BCVars.Length; i++)
            {
                BCVars[i] /= kRad;
                keys.Add(BCNames[i], BCVars[i]);
            }
            //================================//

            //IntensityCurve aka the intensity of the light in normal space
            //====Variables====//
            double[] ICVars = { 1.35E+10, 1E+11, 2.82E+11 };
            string[] ICNames = { "KEYIC1X", "KEYIC2X", "KEYIC3X" };
            double icLum;
            double tempPower = 40;
            //=================//

            //====Calculation of the curve====//
            icLum = lum * tempPower;

            //To replace with a thing with absolute value, and in term to totally delete this shit
            if (rad > 1)
                icLum *= rad;
            else
                icLum /= rad;

            //Calculates and adds to the dictionary
            for (int i = 0; i < ICVars.Length; i++)
            {
                ICVars[i] /= icLum;
                keys.Add(ICNames[i], ICVars[i]);
            }
            //================================//

            //ScaledIntensityCurve aka intensity of the light in scaled space (1/6000th of the normal one)
            // /!\ ALGO HAS TO BE CHANGED (SET SCVARS TO INITIAL DECLARATION VALUE OF ICVARS/6000)
            //====Variables====//
            double[] SCVars = { 2E+07, 1E+9, 2.82E+9 };
            string[] SCNames = { "KEYSC1X", "KEYSC2X", "KEYSC3X" };
            //=================//

            //====Calculation of the curve====//
            //Calculates and adds to the dictionary
            for (int i = 0; i < ICVars.Length; i++)
            {
                SCVars[i] *= lum;
                keys.Add(SCNames[i], SCVars[i]);
            }
            //================================//

            //IVAIntensityCurve aka the intensity of the light in IVA view 
            //====Variables====//
            double[] IVAVars = { 1.35E+10, 1E+11, 2.82E+11 };
            string[] IVANames = { "KEYIVA1X", "KEYIVA2X", "KEYIVA3X" };
            //=================//

            //====Calculation of the curve====//
            //Calculates and adds to the dictionary
            for (int i = 0; i < IVAVars.Length; i++)
            {
                IVAVars[i] *= lum;
                keys.Add(IVANames[i], IVAVars[i]);
            }
            //================================//

            //Calculates the power of the luminosity
            //====Variables====//
            double[] POWVars = { 0.5, 0.9, 10 };
            string[] POWNames = { "KEYPOW0Y", "KEYPOW1Y", "KEYPOW2Y" };
            //=================//
            //====Calculation of the curve====//
            //Calculates and adds to the dictionary
            for (int i = 0; i < IVAVars.Length; i++)
            {
                POWVars[i] *= lum;
                keys.Add(POWNames[i], POWVars[i]);
            }
            //================================//

            return keys;
        }

        /// <summary>
        /// Generates the file of the star
        /// </summary>
        public static string StarFile(
            string gameDataPath, int starCount,
            Dictionary<string, double> galaxySettings, Dictionary<string, string> templateFiles,
            Dictionary<string, Dictionary<string, string>> starDatabase, Random random, out Dictionary<string, Dictionary<string, string>> starRaw)
        {
            string file = templateFiles["Star"];

            starRaw = Star(starDatabase, galaxySettings, gameDataPath, random);

            //====Replacement of all the variables====//
            //With variables
            file = file.Replace("NEEDS[!Kopernicus]", "FOR[INFINITY]")
                .Replace("#VAR-ID", Convert.ToString(starCount + 1));

            //With the Star datas
            foreach (KeyValuePair<string, Dictionary<string, string>> node1 in starRaw)
            {
                foreach (KeyValuePair<string, string> node2 in node1.Value)
                {
                    file = file.Replace("#VAR-" + node2.Key, node2.Value);
                }
            }
            //========================================//
            return file;
        }

        /// <summary>
        /// Creates a Wormhole
        /// </summary> 
        public static string Wormhole(
            int starCount,
            Dictionary<string, double> galaxySettings, Dictionary<string,Dictionary<string, string>> star,
            Dictionary<string, string> templateFiles, bool Up)
        {
            //====Variables====//
            string file;
            string partner;
            string partnerb;
            string radius = "0";
            starCount++;
            //=================//
            file = templateFiles["Wormhole"];

            //Searches for the radius
            foreach (KeyValuePair<string, Dictionary<string, string>> node1 in star)
            {
                foreach (KeyValuePair<string, string> node2 in node1.Value)
                {
                    if (node2.Key == "RADIUS") radius = node2.Value;
                }
            }

            if (Up)//If it is the up Wormhole
            {
                partner = "Star " + Convert.ToString(starCount+1);

                if (starCount != galaxySettings["starNumber"]) //If it is not the last star generated
                {
                    file = file
                       .Replace("NEEDS[!Kopernicus]", "FOR[Infinity]")
                       .Replace("#VAR-PARTNER", partner)
                       .Replace("#VAR-ID", Convert.ToString(starCount))
                       .Replace("#VAR-RADIUS", radius)
                       .Replace("#VAR-MEANANOMALYATEPOCH", Convert.ToString(Math.PI))
                       .Replace("#VAR-WAY", "Up")
                       .Replace("#VAR-WAZ", "Down")
                       .Replace("#VAR-PARBNER", Convert.ToString(starCount));
                }
            }

            else //if it is a down one
            {
                
                    string parbner = "Star " + (starCount);
                    if (starCount == 1) { partner = "Sun"; parbner = "Star 1"; } //If first star generated
                    else partner = "Star " + (starCount-1);


                    file = file
                        .Replace("NEEDS[!Kopernicus]", "FOR[Infinity]")
                        .Replace("#VAR-PARTNER", partner)
                        .Replace("#VAR-ID", Convert.ToString(starCount))
                        .Replace("#VAR-RADIUS", radius)
                        .Replace("#VAR-MEANANOMALYATEPOCH", "0")
                        .Replace("#VAR-WAY", "Down")
                        .Replace("#VAR-WAZ", "Up")
                        .Replace("#VAR-PARBNER", parbner);
                
            }

            return file;
        }

        /// <summary>
        /// New position for Kerbol in the galaxy
        /// </summary>
        public static string NewKerbolPosition(
            string gameDataPath, Dictionary<string, double> galaxySettings, Random random, Dictionary<string, string> templateFiles)
        {
            //Generates the orbit
            Dictionary<string, double> orbitD = Orbit.Star(galaxySettings, random);
            Dictionary<string, string> orbitS = new Dictionary<string, string>();
            foreach (KeyValuePair<string, double> key in orbitD) orbitS.Add(key.Key, Convert.ToString(key.Value));

            string file = templateFiles["BaseSystemOrbit"];
            file = file.Replace("NEEDS[!Kopernicus]", "FOR[INFINITY]");
            foreach (KeyValuePair<string, string> value in orbitS)
            {
                file = file.Replace("#VAR-" + value.Key, value.Value);
            }

            return file;
        }

        /// <summary>
        /// Generates a galaxy
        /// </summary>
        public static void Generate(
            string gameDataPath, Dictionary<string, double> galaxySettings, Dictionary<string, Dictionary<string, string>> starDatabase,
            Random random, Dictionary<string, string> templateFiles)
        {
            string starFolder = gameDataPath + @"StarSystems\Stars";

            //Generates stars
            Stopwatch stopwatch = new Stopwatch();
            Console.Clear();
            Console.WriteLine("Generating Galaxy... Star     /{0}", galaxySettings["starNumber"]);
            stopwatch.Start();
            for (int i = 0; i < galaxySettings["starNumber"]; i++)
            {
                Console.SetCursorPosition(26, 0);
                Console.Write(i + 1);

                //====Generates a star file====//
                Dictionary<string, Dictionary<string, string>> starRaw;
                File.WriteAllText(gameDataPath + @"StarSystems\Stars\Star " + Convert.ToString(i + 1) + ".cfg", StarFile(gameDataPath, i, galaxySettings, templateFiles, starDatabase, random, out starRaw));

                //====Generates wormholes files====//
                //Down one
                File.WriteAllText(gameDataPath + @"StarSystems\Wormholes\Wormhole Down to Star " + Convert.ToString(i) + ".cfg", Wormhole(i, galaxySettings, starRaw, templateFiles, false));
                //And up..
                File.WriteAllText(gameDataPath + @"StarSystems\Wormholes\Wormhole Up to Star " + Convert.ToString(i+1) + ".cfg", Wormhole(i, galaxySettings, starRaw, templateFiles, true));

                //Creates planet
                int planetNumber = random.Next(1, 5);

                for(int j = 0; j < planetNumber; j++)
                {
                    Planet(i + 1, j + 1, gameDataPath, random);
                }
            }
            //Generates new Sun position
            File.WriteAllText(gameDataPath + @"StarSystems\Stars\Sun.cfg", NewKerbolPosition(gameDataPath, galaxySettings, random, templateFiles));

            stopwatch.Stop();
            Console.WriteLine("\nDone! Time elapsed: {0:hh\\:mm\\:ss}", stopwatch.Elapsed);
        }
    }
}