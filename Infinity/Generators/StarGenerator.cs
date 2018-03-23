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
     * ---Habitable Zone----------
     * 
     * -Perfect habitable zone
     * 
     * d = L^(1/2)
     * 
     * -Min and max distance for the habitable zone
     * 
     * dMin = 0.9x(L^(1/2))
     * dMax = 1.35x(L^(1/2))
     * 
     * d = distance of the perfect habitable zone (AU)
     * L = Luminosity of the star divided by Sun's one
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

        public static Dictionary<string, string> Generate(Dictionary<string, Dictionary<string, string>> starDatas, string gameDataPath)
        {
            //-- Star Properties --//
            //double habitableZoneMin;    //AU
            //double habitableZoneBest;   //AU
            //double habitableZoneMax;    //AU
            //---------------------//

            //Creates the dictionary of star's properties
            Dictionary<string, string> properties = new Dictionary<string, string>();

            //Generates the star class
            double frequency = RandomN.Double() * 100;

            double cumuledFrequencies = 0;

            //Star Properties
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

            string starLumClass = "";

            //Checks for each type of star

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

            temperature = RandomN.Int32(minTemperature, maxTemperature);
            radius = RandomN.Double() * (maxRadius - minRadius) + minRadius;
            luminosity = starLumCalcutation(radius, temperature); //Approximate one, lol.
            mass = (RandomN.Double() * (maxMass - minMass) + minMass)/radius; // the "divided by radius" is just a thing to (i hope) make the star more realistic..

            //Gets the luminosity class in their type
            //%centage of the temperature in the range
            double percentageTemp = ((double)temperature - (double)minTemperature)/ (double)(maxTemperature - (double)minTemperature)*10;
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



            double[] rgb = KToRGB(temperature);

            double red = rgb[0];
            if (double.IsNaN(red))
            {
                red = 255;
            }
            double green = rgb[1];
            if(double.IsNaN(green))
            {
                green = 255;
            }
            double blue = rgb[2];
            if (double.IsNaN(blue))
            {
                blue = 255;
            }

            //TEMP------
            /*string tempCoronaPathLumClass = "";
            if ((starLumClass.Equals("9")) || (starLumClass.Equals("8")) || (starLumClass.Equals("7")))
                tempCoronaPathLumClass = "9";

            if ((starLumClass.Equals("6")) || (starLumClass.Equals("5")) || (starLumClass.Equals("4")) || (starLumClass.Equals("3")))
                tempCoronaPathLumClass = "5";

            if ((starLumClass.Equals("2")) || (starLumClass.Equals("1")) || (starLumClass.Equals("0")))
                tempCoronaPathLumClass = "9";*/
            //----------

            properties.Add("Star Class", Convert.ToString(allClasses[classID]));
            properties.Add("Temperature", Convert.ToString(temperature));
            properties.Add("Radius", Convert.ToString(radius));
            properties.Add("Luminosity", Convert.ToString(luminosity));
            properties.Add("Mass", Convert.ToString(mass));
            properties.Add("Color Red", Convert.ToString(red));
            properties.Add("Color Blue", Convert.ToString(blue));
            properties.Add("Color Green", Convert.ToString(green));
            properties.Add("Star Luminosity Class", starLumClass);
            properties.Add("Corona Path", @"Infinity\Templates\Coronas\" + allClasses[classID] + starLumClass + " V.png");

            return properties;
        }
        private static double starLumCalcutation(double radius, double temperature)
        {
            //not working one L = 4*π*R²*σ*T⁴
            //shoud work = L = R²*T⁴

            double luminosity = Math.Sqrt(radius) * Math.Pow(temperature / 5778, 4);
            return luminosity;
        }
        
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

            rgb = new double[]
            {
                Red,
                Green,
                Blue
            };

            return rgb;
        }

        //private static double tempInTempRange(int te)
    }
}
