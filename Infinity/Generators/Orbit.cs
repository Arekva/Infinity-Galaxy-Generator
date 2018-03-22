using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinity.Generators
{
    class Orbit
    {
        public static Dictionary<string, double> RandomOrbit(Dictionary<string, double> doubleDataDic, Random random)
        {
            //-----------Orbital Elements generated-----
            double inclination = 0;
            double maxInclination = 0;

                //Max Inclination depends of the galaxy type, so:
                if (doubleDataDic["galaxySize"] == 1)
                   maxInclination = 15;
                else
                    maxInclination = 90;

            double eccentricity = 0;
                double maxEccentricity = 0.1;

            double semiMajorAxis = 0;

            double meanAnomalyAtEpoch = 0;

            double longitudeOfAscendingNode = 0;

            double epoch = 0;
            //------------------------------------------

            //----Generating elements----
            eccentricity = random.NextDouble() * maxEccentricity;
            semiMajorAxis = random.NextDouble() * doubleDataDic["galaxySize"];

            if (doubleDataDic["galaxySize"] == 1) 
                inclination = (random.NextDouble() * maxInclination ) / semiMajorAxis * 0.7; //The "sMA * x" is to have a bumble at the center / flat on the borders of the spiral galaxy; lower is the value, flater will be the galaxy
            else
                inclination = (random.NextDouble() * maxInclination);
            
            meanAnomalyAtEpoch = random.NextDouble() * doubleDataDic["seed"] * 10;
            longitudeOfAscendingNode = random.NextDouble() * doubleDataDic["seed"] * 100;
            epoch = random.NextDouble() * doubleDataDic["seed"] * 1000;

            semiMajorAxis *= 9.461e+15; //Conversion in meters; 9,461e+15 = 1 Ly in meter

            //----Packing up all the elements in a single dictionary----------------------
            Dictionary<string, double> orbitalElements = new Dictionary<string, double>();
            orbitalElements.Add("inclination", inclination);
            orbitalElements.Add("eccentricity", eccentricity);
            orbitalElements.Add("semiMajorAxis", semiMajorAxis);
            orbitalElements.Add("meanAnomalyAtEpoch", meanAnomalyAtEpoch);
            orbitalElements.Add("longitudeOfAscendingNode", longitudeOfAscendingNode);
            orbitalElements.Add("epoch", epoch);
            //----------------------------------------------------------------------------

            return orbitalElements;

        }
    }
}
