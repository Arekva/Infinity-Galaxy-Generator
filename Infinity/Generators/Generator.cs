using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Infinity.Datas;
using System.Reflection;

namespace Infinity.Generators
{
    class Generator
    {
        /// <summary>
        /// Generates a procedural star
        /// </summary>
        public static Dictionary<string, Dictionary<string, string>> Star(
            Dictionary<string, Dictionary<string, string>> starDatabase, Dictionary<string, double> galaxySettings)//galaxySetings is used for the orbit generator
        {
            //Generates the orbit
            Dictionary<string, double> orbit = new Dictionary<string, double>();
            orbit = Orbit.RandomOrbit(galaxySettings);

            //Converts with dots instead of commas + in string
            Dictionary<string, string> orbitProperties = new Dictionary<string, string>();
            orbitProperties.Add("Inclination", Convert.ToString(orbit["inclination"]).Replace(",","."));
            orbitProperties.Add("Eccentricity", Convert.ToString(orbit["eccentricity"]).Replace(",", "."));
            orbitProperties.Add("Semi Major Axis", Convert.ToString(orbit["semiMajorAxis"]).Replace(",", "."));
            orbitProperties.Add("Mean Anomaly At Epoch", Convert.ToString(orbit["meanAnomalyAtEpoch"]).Replace(",", "."));
            orbitProperties.Add("Longitude Of Ascending Node", Convert.ToString(orbit["longitudeOfAscendingNode"]).Replace(",", "."));
            orbitProperties.Add("Epoch", Convert.ToString(orbit["epoch"]).Replace(",", "."));

            //Generates star class
            Dictionary<string, string> globalPropertiesComma = StarGenerator.Generate(starDatabase);

            Dictionary<string, string> globalProperties = new Dictionary<string, string>();
            globalProperties.Add("Star Class", globalPropertiesComma["Star Class"].Replace(",", "."));
            globalProperties.Add("Temperature", globalPropertiesComma["Temperature"].Replace(",", "."));
            globalProperties.Add("Radius", globalPropertiesComma["Radius"].Replace(",", "."));
            globalProperties.Add("Luminosity", globalPropertiesComma["Luminosity"].Replace(",", "."));
            globalProperties.Add("Mass", globalPropertiesComma["Mass"].Replace(",", "."));
            globalProperties.Add("Color Red", globalPropertiesComma["Color Red"].Replace(",", "."));
            globalProperties.Add("Color Green", globalPropertiesComma["Color Green"].Replace(",", "."));
            globalProperties.Add("Color Blue", globalPropertiesComma["Color Blue"].Replace(",", "."));

            //Makes teh finol packoge
            Dictionary<string, Dictionary<string, string>> properties = new Dictionary<string, Dictionary<string, string>>();
            properties.Add("Orbital Properties", orbitProperties);
            properties.Add("Global Properties", globalProperties);

            return properties;
        }

        /// <summary>
        /// Get the template file then save it with star's generated values
        /// </summary>
        public static string StarFile(Dictionary<string, Dictionary<string, string>> starProperties, string gameDataPath, int starCount)
        {
            string template = File.ReadAllText(gameDataPath + "\\Infinity\\Templates\\Star.cfg");

            string starFile = template
                .Replace("NEEDS[!Kopernicus]", "FOR[Infinity]")
                .Replace("#VAR-ID", Convert.ToString(starCount+1))                               //ID
                .Replace("#VAR-STARCLASS", Datas.Query.Star.Specific(starProperties, "Global Properties", "Star Class"))
                .Replace("#VAR-INC", Datas.Query.Star.Specific(starProperties, "Orbital Properties", "Inclination"))           //Inclination
                .Replace("#VAR-ECC", Datas.Query.Star.Specific(starProperties, "Orbital Properties", "Eccentricity"))            //Eccentricity
                .Replace("#VAR-SMA", Datas.Query.Star.Specific(starProperties, "Orbital Properties", "Semi Major Axis"))           //SemiMajorAxis
                .Replace("#VAR-MAE", Datas.Query.Star.Specific(starProperties, "Orbital Properties", "Mean Anomaly At Epoch"))      //MeanAnomalyAtEpoch
                .Replace("#VAR-LAN", Datas.Query.Star.Specific(starProperties, "Orbital Properties", "Longitude Of Ascending Node"))//LAN
                .Replace("#VAR-EPO", Datas.Query.Star.Specific(starProperties, "Orbital Properties", "Epoch"))                   //Epoch
                .Replace("#VAR-COLOR-RED", Datas.Query.Star.Specific(starProperties, "Global Properties", "Color Red"))                                     //color
                .Replace("#VAR-COLOR-GREEN", Datas.Query.Star.Specific(starProperties, "Global Properties", "Color Green"))
                .Replace("#VAR-COLOR-BLUE", Datas.Query.Star.Specific(starProperties, "Global Properties", "Color Blue"))
                .Replace("#VAR-RADIUS", Datas.Query.Star.Specific(starProperties, "Global Properties", "Radius"))
                .Replace("#VAR-MASS", Datas.Query.Star.Specific(starProperties, "Global Properties", "Mass"))
                .Replace("#VAR-LUMINOSITY", Datas.Query.Star.Specific(starProperties, "Global Properties", "Luminosity"));

            return starFile;
            //File.WriteAllText(stringDataDic["gameDataPath"] + "\\Infinity\\Stars\\Star " + Convert.ToString(starCount) + ".cfg", templateFile);
        }

        /// <summary>
        /// Creates a new orbit for Kerbol
        /// </summary>
        public static string NewKerbolPosition(string gameDataPath, Dictionary<string, double> galaxySettings)
        {
            string template = File.ReadAllText(gameDataPath + "\\Infinity\\Templates\\BaseSystemOrbit.cfg");

            //Generates the orbit
            Dictionary<string, double> orbit = new Dictionary<string, double>();

            orbit = Orbit.RandomOrbit(galaxySettings);

            //Converts with dots instead of commas + in string
            Dictionary<string, string> orbitProperties = new Dictionary<string, string>();
            orbitProperties.Add("Inclination", Convert.ToString(orbit["inclination"]).Replace(",", "."));
            orbitProperties.Add("Eccentricity", Convert.ToString(orbit["eccentricity"]).Replace(",", "."));
            orbitProperties.Add("Semi Major Axis", Convert.ToString(orbit["semiMajorAxis"]).Replace(",", "."));
            orbitProperties.Add("Mean Anomaly At Epoch", Convert.ToString(orbit["meanAnomalyAtEpoch"]).Replace(",", "."));
            orbitProperties.Add("Longitude Of Ascending Node", Convert.ToString(orbit["longitudeOfAscendingNode"]).Replace(",", "."));
            orbitProperties.Add("Epoch", Convert.ToString(orbit["epoch"]).Replace(",", "."));

            string starFile = template
                .Replace("NEEDS[!Kopernicus]", "FOR[Infinity]")
                .Replace("#VAR-INC", orbitProperties["Inclination"])
                .Replace("#VAR-ECC", orbitProperties["Eccentricity"])
                .Replace("#VAR-SMA", orbitProperties["Semi Major Axis"]) 
                .Replace("#VAR-MAE", orbitProperties["Mean Anomaly At Epoch"])
                .Replace("#VAR-LAN", orbitProperties["Longitude Of Ascending Node"])
                .Replace("#VAR-EPO", orbitProperties["Epoch"]);

            return starFile;
        }
        /// <summary>
        /// Creates the galaxy with generated stars, planet, and other celestial bodies
        /// </summary>
        public static void Galaxy(
            string gameDataPath, Dictionary<string, double> doubleDataDic, Dictionary<string, Dictionary<string, string>> starDatabase)
        {
            
            string starFolder = gameDataPath + "\\Infinity\\Stars";

            //Generates stars

            for (int i = 0; i < doubleDataDic["starNumber"]; i++)
            {
                Dictionary<string, Dictionary<string, string>> Star = Generator.Star(starDatabase, doubleDataDic);

                File.WriteAllText(gameDataPath + "\\Infinity\\Stars\\Star " + Convert.ToString(i+1) + ".cfg", Generator.StarFile(Star, gameDataPath, i));
            }

            //Generates new Sun position

            File.WriteAllText(gameDataPath + "\\Infinity\\Stars\\Sun.cfg", NewKerbolPosition(gameDataPath, doubleDataDic));
        }
    }
}
