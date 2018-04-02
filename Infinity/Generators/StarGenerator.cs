using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infinity.Datas.Query;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Infinity.Generators
{
    /* [IMPORTANT DATAS]
     * 
     * ---Luminosity of the star---
     * 
     * - Complex one
     * L = 4*π*R²*σ*T⁴
     * 
     * L = Luminosity of the star divided by Sun's one
     * T = Temperature of the star divided by Sun's one (Kelvin)
     * R = Radius of the star divided by Sun's one (meter)
     * σ = Stefan-Boltzmann constant (5.670373e-8 W/m²/K⁴) 
     * π = Pi (~3.1416)
     * 
     *- Used one
     * L = R^2 * M^4
     * 
     * L = Luminosity of the star divided by Sun's one
     * R = Radius of the star in solar radius
     * M = Mass in solar masses
     *
     * 
     
     * 
     * ---Some Datas-------------
     * 
     * --Sun--
     * radius = 695 700 km
     * mass = 1.989e+30 kg
     * volumic mass = 1 408 kg/m^3
     * 
     * --Minimum and maximum values for a star--
     * 
     * -Mass
     * mMin = 0.07 solar masses
     * mMax = 150 solar masses
     * 
     * -Size
     * rMin = 0.01 solar radius
     * rMax = 1000 solar radius
     */

    /// <summary>
    /// Main star generator
    /// </summary>
    class StarGenerator
    {
        //===Sun properties===

        private readonly double solRadius = 695700; //km
        private readonly double solMass = 1.989e+30; //kg
        private readonly double solVolumicMass = 1408; //kg/m^3

        //====================

        /// <summary>
        /// Generates a star
        /// </summary>
        /// <param name="starDatas"></param>
        /// <param name="gameDataPath"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Generate(Dictionary<string, Dictionary<string, string>> starDatas, string gameDataPath, Random random)
        {       
            //Generates the star class
            double frequency = random.NextDouble() * 100;

            double cumuledFrequencies = 0;

            //Star Properties Variables
            int classID; //<---- where m is 0, k is 1 etc etc

            int minTemperature = 0;
            int maxTemperature = 0;
            int temperature = 0;

            double minRadius = 0;
            double maxRadius = 0;
            double radius = 0; //in solar radius when initia.

            double maxMass = 0;
            double minMass = 0;
            double mass = 0; //in solar mass when initia.
            double luminosity = 0;
            //---------------

            string[] allClasses =
            {
                "M",
                "K",
                "G",
                "F",
                "A",
                "B",
                "O"
            };

            string starLumClass = "Error";

            //Checks frequency for each type of star in the database
            for (classID = 0; classID < starDatas.Count; classID++)
            {
                string starFreqS = Star.Specific(starDatas, allClasses[classID], "Rarity");
                Double.TryParse(starFreqS, out double starFreq);

                //Gets the surface temperature
                cumuledFrequencies += starFreq;
                if (frequency < cumuledFrequencies)
                {
                    //Gets the minimal temperature possible
                    string minTemperatureS = Star.Specific(starDatas, allClasses[classID], "Temperature");
                    Int32.TryParse(minTemperatureS, out minTemperature);

                    //Gets the minimal radius possible
                    string minRadiusS = Star.Specific(starDatas, allClasses[classID], "Solar radius");
                    Double.TryParse(minRadiusS, out minRadius);

                    //Gets the maximal properties
                    if (classID == starDatas.Count - 1) //If it is a O class
                    {
                        maxTemperature = 50000;
                        maxRadius = 10;
                        maxMass = 90;
                    }
                    else
                    {
                        string maxTemperatureS = Star.Specific(starDatas, allClasses[classID + 1], "Temperature");
                        Int32.TryParse(maxTemperatureS, out maxTemperature);

                        string maxRadiusS = Star.Specific(starDatas, allClasses[classID + 1], "Solar radius");
                        Double.TryParse(maxRadiusS, out maxRadius);

                        string maxMassS = Star.Specific(starDatas, allClasses[classID + 1], "Solar mass");
                        Double.TryParse(maxMassS, out maxMass);
                    }

                    break;
                }
            }

            temperature = random.Next(minTemperature, maxTemperature + 1);
            radius = random.NextDouble() * (maxRadius - minRadius) + minRadius;
            luminosity = starLumCalcutation(radius, temperature); //Approximate one, lol.
            mass = (random.NextDouble() * (maxMass - minMass) + minMass)/radius; // the "divided by radius" is just a thing to (i hope) make the star more realistic..

            //Calculates and loads the luminosity class
            starLumClass = starLumClassCalc(temperature, minTemperature, maxTemperature);

            //Calculates and loads the color
            double[] rgb = KToRGB(temperature);

            //Calculates the habitable zone
            habitableZoneCalc(luminosity, out double minHabZone, out double bestHabZone, out double maxHabZone);

            //Adding all the properties in a dictionnary
            Dictionary<string, string> properties = new Dictionary<string, string>();

            properties.Add("Star Class", Convert.ToString(allClasses[classID]));
            properties.Add("Temperature", Convert.ToString(temperature));
            properties.Add("Radius", Convert.ToString(radius));
            properties.Add("Luminosity", Convert.ToString(luminosity));
            properties.Add("Mass", Convert.ToString(mass));
            properties.Add("Color Red", Convert.ToString(rgb[0]));
            properties.Add("Color Green", Convert.ToString(rgb[1]));
            properties.Add("Color Blue", Convert.ToString(rgb[2]));
            properties.Add("Star Luminosity Class", starLumClass);
            properties.Add("Corona Path", @"Infinity\Templates\Coronas\V\" + allClasses[classID] + @"\" + starLumClass + ".png");
            properties.Add("Habitable Zone Min", Convert.ToString(minHabZone));
            properties.Add("Habitable Zone Best", Convert.ToString(bestHabZone));
            properties.Add("Habitable Zone Max", Convert.ToString(maxHabZone));

            return properties;
        }

        /// <summary>
        /// Calculates the habitable zone in AU
        /// </summary>
        /// <param name="luminosity"></param>
        /// <param name="min"></param>
        /// <param name="best"></param>
        /// <param name="max"></param>
        private static void habitableZoneCalc(double luminosity, out double min, out double best, out double max)
        {
            min = 0.9 * (Math.Pow(luminosity, 0.5));
            best = Math.Pow(luminosity, 0.5);
            max = 1.35 * (Math.Pow(luminosity, 0.5));
        }

        /// <summary>
        /// Calculates star's luminosity class (0, 1, 2, etc)
        /// </summary>
        /// <param name="temperature"></param>
        /// <param name="minTemperature"></param>
        /// <param name="maxTemperature"></param>
        /// <returns></returns>
        private static string starLumClassCalc(double temperature, double minTemperature, double maxTemperature)
        {
            //Gets the luminosity class in their type
            //%centage of the temperature in the range
            string starLumClass = "Error";
            double percentageTemp = ((double)temperature - (double)minTemperature) / (double)(maxTemperature - (double)minTemperature) * 10;
            percentageTemp = Math.Round(percentageTemp);

            if (percentageTemp == 0)
                starLumClass = "9";

            if (percentageTemp == 1)
                starLumClass = "8";

            if (percentageTemp == 2)
                starLumClass = "7";

            if (percentageTemp == 3)
                starLumClass = "6";

            if (percentageTemp == 4)
                starLumClass = "5";

            if (percentageTemp == 5)
                starLumClass = "4";

            if (percentageTemp == 6)
                starLumClass = "3";

            if (percentageTemp == 7)
                starLumClass = "2";

            if (percentageTemp == 8)
                starLumClass = "1";

            if ((percentageTemp == 9) || (percentageTemp == 10))
                starLumClass = "0";

            return starLumClass;
        }

        /// <summary>
        /// Calculates star's luminosity
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="temperature"></param>
        /// <returns></returns>
        private static double starLumCalcutation(double radius, double temperature)
        {
            //not working one L = 4*π*R²*σ*T⁴
            //shoud work = L = R²*T⁴

            double luminosity = Math.Sqrt(radius) * Math.Pow(temperature / 5778, 4);
            return luminosity;
        }
        
        /// <summary>
        /// Calculates star's color by temperature
        /// </summary>
        /// <param name="Temperature"></param>
        /// <returns></returns>
        private static double[] KToRGB(int Temperature)
        //took and converted from http://www.tannerhelland.com/4435/convert-temperature-rgb-algorithm-code/
        {
            double Red;
            double Green;
            double Blue;

            double[] rgb;

            Temperature = Temperature / 100;

            if (Temperature <= 66)
            {
                Red = 255;
            }
            else
            {
                Red = Temperature - 60;
                Red = 329.698727466 * Math.Pow(Red, -0.1332047592);
                if (Red < 0)
                {
                    Red = 0;
                }
                if (Red > 255)
                {
                    Red = 255;
                }
            }

            if (Temperature <= 66)
            {
                Green = Temperature;
                Green = 99.4708025861 * Math.Log(Green) - 161.1195681661;
                if (Green < 0)
                {
                    Green = 0;
                }
                if (Green > 255)
                {
                    Green = 255;
                }
            }
            else
            {
                Green = Temperature - 60;
                Green = 288.1221695283 * Math.Log(Green, -0.0755148492);
                if (Green < 0)
                {
                    Green = 0;
                }
                if (Green > 255)
                {
                    Green = 255;
                }
            }

            if (Temperature >= 66)
            {
                Blue = 255;
            }
            else
            {
                if (Temperature <= 19)
                {
                    Blue = 0;
                }
                else
                {
                    Blue = Temperature - 10;
                    Blue = 138.5177312231 * Math.Log(Blue) - 305.0447927307;
                    if (Blue < 0)
                    {
                        Blue = 0;
                    }
                    if (Blue > 255)
                    {
                        Blue = 255;
                    }
                }
            }

            Red = Math.Round(Red);
            Green = Math.Round(Green);
            Blue = Math.Round(Blue);

            if (double.IsNaN(Red))
                Red = 255;

            if (double.IsNaN(Green))
                Green = 255;

            if (double.IsNaN(Blue))
                Blue = 255;

            rgb = new double[]
            {
                Red,
                Green,
                Blue
            };

            return rgb;
        }
    }
}
