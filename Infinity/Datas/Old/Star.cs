using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace Infinity.Datas.Old
{
    /// <summary>
    /// Containts and creates a dictionary of star properties
    /// (String [Class], (String[Propertie],String[Value]))
    /// </summary>
    class Star
    {
        /// <summary>
        /// Create a dictionary of star properties
        /// (String [Class], (String[Propertie],String[Value]))
        /// </summary>
        public static Dictionary<string, Dictionary<string,string>> ComputeStarData()
        {
            //---Initialize all the datas---///

            /// <summary>
            /// All star classes can be generated in the program
            /// </summary>
            string[] Class =
            {
                "O",
                "B",
                "A",
                "F",
                "G",
                "K",
                "M"
            };

            /// <summary>
            /// Minimal temperatures for each class (Kelvin)
            /// </summary>
            int[] Temperature =
            {
                20000, //O, OLD- 30000 |  let's say the max temperature is 50000K
                10000, //B
                7500, //A
                6000, //F
                5200, //G
                3700, //K
                2400  //M
            };

            /// <summary>
            /// Surface color of each class
            /// </summary>
            string[] VegaRelativeChromacity =
            {
                "Blue", //O
                "Blue white", //B
                "White", //A
                "Yellow white", //F
                "Yellow", //G
                "Light orange", //K
                "Orange red"  //M
            };

            /// <summary>
            /// Emitted color of each class
            /// </summary>
            string[] D65Chromacity =
            {
                "Blue", //O
                "Deep blue white", //B
                "Blue white", //A
                "White", //F
                "Yellowish white", //G
                "Pale yellow orange", //K
                "Light orange red"  //M
            };

            /// <summary>
            /// Minimal solar masses for each class
            /// </summary>
            double[] SolarMass =
            {
                16, //O, max is 90
                2.1, //B
                1.4, //A
                1.04, //F
                0.8, //G
                0.45, //K
                0.08  //M
            };

            /// <summary>
            /// Minimal solar radius for each class
            /// </summary>
            double[] SolarRadius =
            {
                6.6, //O max is 10
                1.8, //B
                1.4, //A
                1.15, //F
                0.96, //G
                0.7, //K
                0.26 //M
            };

            /// <summary>
            /// Minimal luminosity for each classes
            /// </summary>
            double[] Bolometric =
            {
                30000, //O, max is 1000000
                25, //B
                5, //A
                1.5, //F
                0.6, //G
                0.08, //K
                0.0055  //M
            };

            /// <summary>
            /// Fraction of each class of star in all of them (%)
            /// </summary>
            double[] Fraction =
            {
                0.12, //O (true is 0.00003, but 0.12 gonna be use for this)
                0.13, //B
                0.6, //A
                3, //F
                7.6, //G
                12.1, //K
                76.45  //M
            };


            //---Compile everything into dictionaries---//

            Dictionary<string, Dictionary<string, string>> StarProperties =
                new Dictionary<string, Dictionary<string, string>>();

            //Creates dictionary
            for (int i = 0; i < Class.Length; i++)
            {
                //Creates Properties
                Dictionary<string, string> Properties =
                    new Dictionary<string, string>();

                Properties.Add("TEMPERATURE", Convert.ToString(Temperature[i]));
                Properties.Add("SURFACECOLOR", VegaRelativeChromacity[i]);
                Properties.Add("EMITTEDCOLOR", D65Chromacity[i]);
                Properties.Add("SOLARMASS", Convert.ToString(SolarMass[i]));
                Properties.Add("SOLARRADIUS", Convert.ToString(SolarRadius[i]));
                Properties.Add("LUMINOSITY", Convert.ToString(Bolometric[i]));
                Properties.Add("RARITY", Convert.ToString(Fraction[i]));

                //Linking the class with its properties
                StarProperties.Add(Class[i], Properties);
            }
            return StarProperties;
        }       
    }  
}
