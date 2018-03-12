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

        public static string StarFile(Dictionary<string, Dictionary<string, string>> starProperties, string gameDataPath, int starCount)
        {
            string template = File.ReadAllText(gameDataPath + "\\Infinity\\Templates\\Star.cfg");

            string starFile = template
                .Replace("NEEDS[!Kopernicus]", "FOR[Infinity]")
                .Replace("#VAR-ID", Convert.ToString(starCount))                               //ID
                .Replace("#VAR-STARCLASS", Datas.Query.Star.Specific(starProperties, "Global Properties", "Star class"))
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

        public static void Galaxy(
            string gameDataPath, Dictionary<string, double> doubleDataDic, Dictionary<string, Dictionary<string, string>> starDatabase)
        {
            
            string starFolder = gameDataPath + "\\Infinity\\Stars";

            //Generates stars

            for (int i = 0; i < doubleDataDic["starNumber"]; i++)
            {
                Dictionary<string, Dictionary<string, string>> Star = Generator.Star(starDatabase, doubleDataDic);

                File.WriteAllText(gameDataPath + "\\Infinity\\Stars\\Star " + Convert.ToString(i++) + ".cfg", Generator.StarFile(Star, gameDataPath, i));
            }

            /*Old things
            //-----------Orbital Elements-!--------------
            double inclination = 0;

            double eccentricity = 0;

            double semiMajorAxis = 0;

            double meanAnomalyAtEpoch = 0;

            double longitudeOfAscendingNode = 0;

            double epoch = 0;

            //All in a dictionary
            Dictionary<string, double> orbitalElements = new Dictionary<string, double>();
            orbitalElements.Add("inclination", inclination);
            orbitalElements.Add("eccentricity", eccentricity);
            orbitalElements.Add("semiMajorAxis", semiMajorAxis);
            orbitalElements.Add("meanAnomalyAtEpoch", meanAnomalyAtEpoch);
            orbitalElements.Add("longitudeOfAscendingNode", longitudeOfAscendingNode);
            orbitalElements.Add("epoch", epoch);
            //------------------------------------------

            //Other things----
            string templateFilePath = stringDataDic["gameDataPath"];

            string luminosityClass = "";
            string orbitColor = "";

            int starCount = 0;
            //----------------

            Console.WriteLine("\nGeneration of the config files..");

            for (int i = 0; i < starConfigsDics.Length; i++)
            {
                Console.WriteLine("\nPass {0}/{1}..", i+1, starConfigsDics.Length);

                foreach (KeyValuePair<string, string> pair in starConfigsDics[i])
                {
                    templateFilePath = stringDataDic["gameDataPath"];

                    luminosityClass = "";
                    orbitColor = "";

                    if (pair.Key == "spectralclass")
                    {
                        if (pair.Value == "M") // Generates M Class stars
                        {
                            Console.WriteLine("Generating M Stars..");

                            foreach (var pair2 in starNumberForEachClass) // Searchs for M class number to generate
                            {
                                if (pair2.Key == "M")
                                {
                                    foreach (KeyValuePair<string, string> pair3 in starConfigsDics[i])
                                    {
                                        if (pair3.Key == "templatepath") //template path
                                        {
                                            templateFilePath = templateFilePath + pair3.Value;
                                        }
                                    }
                                    for (int j = 0; j < pair2.Value; j++)
                                    {
                                        starCount++;

                                        //Orbit Generation
                                        orbitalElements = Orbit.RandomOrbit(doubleDataDic);

                                        //Converts orbital elements commas to points
                                        Dictionary<string, string> orbitalElementsString = new Dictionary<string, string>();

                                        orbitalElementsString.Add("inclination", Convert.ToString(orbitalElements["inclination"]).Replace(",", "."));
                                        orbitalElementsString.Add("eccentricity", Convert.ToString(orbitalElements["eccentricity"]).Replace(",", "."));
                                        orbitalElementsString.Add("semiMajorAxis", Convert.ToString(orbitalElements["semiMajorAxis"]).Replace(",", "."));
                                        orbitalElementsString.Add("meanAnomalyAtEpoch", Convert.ToString(orbitalElements["meanAnomalyAtEpoch"]).Replace(",", "."));
                                        orbitalElementsString.Add("longitudeOfAscendingNode", Convert.ToString(orbitalElements["longitudeOfAscendingNode"]).Replace(",", "."));
                                        orbitalElementsString.Add("epoch", Convert.ToString(orbitalElements["epoch"]).Replace(",", "."));

                                        //-----Get other star settings------------


                                        foreach (KeyValuePair<string, string> pair3 in starConfigsDics[i])
                                        {
                                            if (pair3.Key == "luminosityclass") //lum class
                                            {
                                                luminosityClass = pair3.Value;
                                            }
                                            if (pair3.Key == "orbitcolor") //orbit color
                                            {
                                                orbitColor = pair3.Value;
                                            }
                                        }

                                        //----------------------------------------

                                        //-----Now replace file variables and save-----
                                        string templateFile = File.ReadAllText(templateFilePath);

                                        templateFile = templateFile
                                            .Replace("NEEDS[!Kopernicus]", "FOR[Infinity]")
                                            .Replace("#VAR-ID", Convert.ToString(starCount))                               //ID
                                            .Replace("#VAR-STARCLASS", pair2.Key)                                  //Star Class
                                            .Replace("#VAR-LUMCLASS", luminosityClass)                             //Lumiosity Class
                                            .Replace("#VAR-INC", orbitalElementsString["inclination"])             //Inclination
                                            .Replace("#VAR-ECC", orbitalElementsString["eccentricity"])            //Eccentricity
                                            .Replace("#VAR-SMA", orbitalElementsString["semiMajorAxis"])           //SemiMajorAxis
                                            .Replace("#VAR-MAE", orbitalElementsString["meanAnomalyAtEpoch"])      //MeanAnomalyAtEpoch
                                            .Replace("#VAR-LAN", orbitalElementsString["longitudeOfAscendingNode"])//LAN
                                            .Replace("#VAR-EPO", orbitalElementsString["epoch"])                   //Epoch
                                            .Replace("#VAR-COL", orbitColor);                                      //color

                                        File.WriteAllText(stringDataDic["gameDataPath"] + "\\Infinity\\Stars\\Star " + Convert.ToString(starCount) + ".cfg", templateFile);
                                    }
                                }
                            }
                        }

                        if (pair.Value == "G") // Generates G Class stars
                        {
                            Console.WriteLine("Generating G Stars..");

                            foreach (var pair2 in starNumberForEachClass) // Searchs for G class number to generate
                            {
                                if (pair2.Key == "G")
                                {
                                    foreach (KeyValuePair<string, string> pair3 in starConfigsDics[i])
                                    {
                                        if (pair3.Key == "templatepath") //template path
                                        {
                                            templateFilePath = templateFilePath + pair3.Value;
                                        }
                                    }
                                    for (int j = 0; j < pair2.Value; j++)
                                    {
                                        starCount++;

                                        //Orbit Generation
                                        orbitalElements = Orbit.RandomOrbit(doubleDataDic);

                                        //Converts orbital elements commas to points
                                        Dictionary<string, string> orbitalElementsString = new Dictionary<string, string>();

                                        orbitalElementsString.Add("inclination", Convert.ToString(orbitalElements["inclination"]).Replace(",", "."));
                                        orbitalElementsString.Add("eccentricity", Convert.ToString(orbitalElements["eccentricity"]).Replace(",", "."));
                                        orbitalElementsString.Add("semiMajorAxis", Convert.ToString(orbitalElements["semiMajorAxis"]).Replace(",", "."));
                                        orbitalElementsString.Add("meanAnomalyAtEpoch", Convert.ToString(orbitalElements["meanAnomalyAtEpoch"]).Replace(",", "."));
                                        orbitalElementsString.Add("longitudeOfAscendingNode", Convert.ToString(orbitalElements["longitudeOfAscendingNode"]).Replace(",", "."));
                                        orbitalElementsString.Add("epoch", Convert.ToString(orbitalElements["epoch"]).Replace(",", "."));

                                        //-----Get other star settings------------


                                        foreach (KeyValuePair<string, string> pair3 in starConfigsDics[i])
                                        {
                                            if (pair3.Key == "luminosityclass") //lum class
                                            {
                                                luminosityClass = pair3.Value;
                                            }
                                            if (pair3.Key == "orbitcolor") //orbit color
                                            {
                                                orbitColor = pair3.Value;
                                            }
                                        }

                                        //----------------------------------------

                                        //-----Now replace file variables and save-----
                                        string templateFile = File.ReadAllText(templateFilePath);

                                        templateFile = templateFile
                                            .Replace("NEEDS[!Kopernicus]", "FOR[Infinity]")
                                            .Replace("#VAR-ID", Convert.ToString(starCount))                               //ID
                                            .Replace("#VAR-STARCLASS", pair2.Key)                                  //Star Class
                                            .Replace("#VAR-LUMCLASS", luminosityClass)                             //Lumiosity Class
                                            .Replace("#VAR-INC", orbitalElementsString["inclination"])             //Inclination
                                            .Replace("#VAR-ECC", orbitalElementsString["eccentricity"])            //Eccentricity
                                            .Replace("#VAR-SMA", orbitalElementsString["semiMajorAxis"])           //SemiMajorAxis
                                            .Replace("#VAR-MAE", orbitalElementsString["meanAnomalyAtEpoch"])      //MeanAnomalyAtEpoch
                                            .Replace("#VAR-LAN", orbitalElementsString["longitudeOfAscendingNode"])//LAN
                                            .Replace("#VAR-EPO", orbitalElementsString["epoch"])                   //Epoch
                                            .Replace("#VAR-COL", orbitColor);                                      //color

                                        File.WriteAllText(stringDataDic["gameDataPath"] + "\\Infinity\\Stars\\Star " + Convert.ToString(starCount++) + ".cfg", templateFile);
                                    }
                                }
                            }
                        }

                        if (pair.Value == "O") // Generates O Class stars
                        {
                            Console.WriteLine("Generating O Stars..");

                            foreach (var pair2 in starNumberForEachClass) // Searchs for M class number to generate
                            {
                                if (pair2.Key == "O")
                                {
                                    foreach (KeyValuePair<string, string> pair3 in starConfigsDics[i])
                                    {
                                        if (pair3.Key == "templatepath") //template path
                                        {
                                            templateFilePath = templateFilePath + pair3.Value;
                                        }
                                    }
                                    for (int j = 0; j < pair2.Value; j++)
                                    {
                                        starCount++;
                                        //Orbit Generation
                                        orbitalElements = Orbit.RandomOrbit(doubleDataDic);

                                        //Converts orbital elements commas to points
                                        Dictionary<string, string> orbitalElementsString = new Dictionary<string, string>();

                                        orbitalElementsString.Add("inclination", Convert.ToString(orbitalElements["inclination"]).Replace(",", "."));
                                        orbitalElementsString.Add("eccentricity", Convert.ToString(orbitalElements["eccentricity"]).Replace(",", "."));
                                        orbitalElementsString.Add("semiMajorAxis", Convert.ToString(orbitalElements["semiMajorAxis"]).Replace(",", "."));
                                        orbitalElementsString.Add("meanAnomalyAtEpoch", Convert.ToString(orbitalElements["meanAnomalyAtEpoch"]).Replace(",", "."));
                                        orbitalElementsString.Add("longitudeOfAscendingNode", Convert.ToString(orbitalElements["longitudeOfAscendingNode"]).Replace(",", "."));
                                        orbitalElementsString.Add("epoch", Convert.ToString(orbitalElements["epoch"]).Replace(",", "."));

                                        //-----Get other star settings------------


                                        foreach (KeyValuePair<string, string> pair3 in starConfigsDics[i])
                                        {
                                            if (pair3.Key == "luminosityclass") //lum class
                                            {
                                                luminosityClass = pair3.Value;
                                            }
                                            if (pair3.Key == "orbitcolor") //orbit color
                                            {
                                                orbitColor = pair3.Value;
                                            }
                                        }

                                        //----------------------------------------

                                        //-----Now replace file variables and save-----
                                        string templateFile = File.ReadAllText(templateFilePath);

                                        templateFile = templateFile
                                            .Replace("NEEDS[!Kopernicus]", "FOR[Infinity]")
                                            .Replace("#VAR-ID", Convert.ToString(starCount))                               //ID
                                            .Replace("#VAR-STARCLASS", pair2.Key)                                  //Star Class
                                            .Replace("#VAR-LUMCLASS", luminosityClass)                             //Lumiosity Class
                                            .Replace("#VAR-INC", orbitalElementsString["inclination"])             //Inclination
                                            .Replace("#VAR-ECC", orbitalElementsString["eccentricity"])            //Eccentricity
                                            .Replace("#VAR-SMA", orbitalElementsString["semiMajorAxis"])           //SemiMajorAxis
                                            .Replace("#VAR-MAE", orbitalElementsString["meanAnomalyAtEpoch"])      //MeanAnomalyAtEpoch
                                            .Replace("#VAR-LAN", orbitalElementsString["longitudeOfAscendingNode"])//LAN
                                            .Replace("#VAR-EPO", orbitalElementsString["epoch"])                   //Epoch
                                            .Replace("#VAR-COL", orbitColor);                                      //color

                                        File.WriteAllText(stringDataDic["gameDataPath"] + "\\Infinity\\Stars\\Star " + Convert.ToString(starCount++) + ".cfg", templateFile);
                                    }
                                }
                            }
                        }
                    }
                }
            }*/
        }
    }
}
