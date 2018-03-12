using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infinity.Datas.Querry;

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

        public static Dictionary<string, string> Generate(Dictionary<string, Dictionary<string, string>> starDatas)
        {
            //-- Star Properties
            //double radius;  //km
            //double mass;    //kg
            //double luminosity;  //Solar luminosity

            //double habitableZoneMin;    //AU
            //double habitableZoneBest;   //AU
            //double habitableZoneMax;    //AU
            //-------------------//

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

            double luminosity = 0; //in solar luminosity when initia.
            double mass = 0; //in solar mass when initia.
            
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

            //Checks for each type of star
            
            for(classID = 0; classID < starDatas.Count; classID++)
            {
                Star.Specific(starDatas, allClasses[classID], "Rarity", out string starFreqS);
                Double.TryParse(starFreqS, out double starFreq);

                //Gets the surface temperature
                cumuledFrequencies += starFreq;
                if (frequency < cumuledFrequencies)
                {
                    //Gets the minimal temperature possible
                    Star.Specific(starDatas, allClasses[classID], "Temperature", out string minTemperatureS);
                        Int32.TryParse(minTemperatureS, out minTemperature);

                    //Gets the minimal radius possible
                    Star.Specific(starDatas, allClasses[classID], "Solar radius", out string minRadiusS);
                        Double.TryParse(minRadiusS, out minRadius);


                    //Gets the maximal properties
                    if (classID == starDatas.Count - 1) //If it is a O class
                    {
                        maxTemperature = 50000;
                        maxRadius = 10;
                    }
                    else
                    {
                        Star.Specific(starDatas, allClasses[classID + 1], "Temperature", out string maxTemperatureS);
                        Int32.TryParse(maxTemperatureS, out maxTemperature);

                        Star.Specific(starDatas, allClasses[classID + 1], "Solar radius", out string maxRadiusS);
                        Double.TryParse(maxRadiusS, out maxRadius);
                    }

                    break;
                } 
            }    

            temperature = RandomN.Int32(minTemperature, maxTemperature);
            radius = RandomN.Double() * (maxRadius - minRadius) + minRadius;

            //L = 4*π*R²*σ*T⁴
            luminosity = 4*Math.PI*Math.Sqrt(radius)* 5.670373e-8 * Math.Pow(temperature/5778, 4);

            Console.WriteLine("Étoile de classe {0}:\n" +
                "Température minimale = {1}, Température maximale = {2}\nTempérature de l'étoile = {3}\n" +
                "Rayon minimal = {4}, Rayon maximal = {5}\nRayon de l'étoile = {6}\n" +
                "Luminositée = {5}",
                allClasses[classID], minTemperature, maxTemperature, temperature, minRadius, maxRadius, radius, luminosity);




            return properties;
        }
    }
}
