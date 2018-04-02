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

namespace Infinity.Generators
{
    class Galaxy
    {
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
            }
            //Generates new Sun position
            File.WriteAllText(gameDataPath + @"StarSystems\Stars\Sun.cfg", NewKerbolPosition(gameDataPath, galaxySettings, random, templateFiles));

            stopwatch.Stop();
            Console.WriteLine("\nDone! Time elapsed: {0:hh\\:mm\\:ss}", stopwatch.Elapsed);
        }
    }
}