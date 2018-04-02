using System;
using System.Collections.Generic;

namespace Infinity.Generators
{
    class Orbit
    {
        /// <summary>
        /// Generate an orbit made for a star
        /// </summary>
        public static Dictionary<string, double> Star(
            Dictionary<string, double> galaxySettings, Random random)
        {
            //====List of the orbital elements====//
            string[] elements =
            {
                "inclination",
                "eccentricity",
                "semiMajorAxis",
                "meanAnomalyAtEpoch",
                "longitudeOfAscendingNode",
                "epoch"
            };

            double[] elementsValue =
            {
                0,
                0,
                0,
                0,
                0,
                0
            };

            double maxInclination = 0;
            double maxEccentricity = 0.1;
            //=====================================//

            //====Generation of all elements====//
            //Inclination
            //The inclination depends of the galaxy type (0 is Eliptical, 1 is spiral)
            if (galaxySettings["galaxySize"] == 1) maxInclination = 10;
            else maxInclination = 90;
            elementsValue[0] = Inclination(0, maxInclination, random);

            //Eccentricity
            elementsValue[1] = Eccentricity(0, maxEccentricity, random);

            //Semi Major Axis
            elementsValue[2] = SemiMajorAxis(0.001, galaxySettings["galaxySize"], random);

            //Mean Anomaly at Epoch
            elementsValue[3] = MeanAnomalyAtEpoch(0, Math.PI * 2, random);

            //Longitude of Ascending Node
            elementsValue[4] = LongitudeOfAscendingNode(0, Math.PI * 2, random);

            //Epoch
            elementsValue[5] = Epoch(0, 365.242198781, random);

            //Gives certain inclination to the galaxy
            elementsValue[0] = GalaxyAlgorithm(galaxySettings, random, elementsValue[2], elementsValue[0], maxInclination, elementsValue[4]);
            //==================================//

            //Elements in dictionnary
            Dictionary<string, double> orbit = new Dictionary<string, double>();

            for (int i = 0; i < 6; i++) //For all the orbital elements
                orbit.Add(elements[i], elementsValue[i]);

            return orbit;
        }

        /// <summary>
        /// Adds a inclination for the galaxy and creates a "shape"
        /// </summary>
        public static double GalaxyAlgorithm(
            Dictionary<string, double> galaxySettings, Random r,
            double sma, double inc, double maxInc, double lan)
        {
            double result = 0;

            if (galaxySettings["galaxyType"] == 1) //If it is a spiral galaxy
            {
                lan += 3;

                double incMultiplier = sma / galaxySettings["galaxySize"];

                double incInversionMult = 1 - incMultiplier;

                double incMultiplierFinal = incInversionMult * 6;

                inc = Math.Sqrt(maxInc * (1 - (sma / galaxySettings["galaxySize"])));

                result = inc = (r.NextDouble() * 6) + 10;
            }

            return result;
        }

        /// <summary>
        /// Generates a random Inclination
        /// </summary>
        public static double Inclination(double min, double max, Random r)
        {
            double v;
            return v = r.NextDouble() * (min - max) + max;
        }

        /// <summary>
        /// Generates a random Eccentrity
        /// </summary>
        public static double Eccentricity(double min, double max, Random r)
        {
            double v;
            return v = r.NextDouble() * (min - max) + max;
        }

        /// <summary>
        /// Generates a random Semi Major Axis
        /// </summary>
        public static double SemiMajorAxis(double min, double max, Random r)
        {
            double v;
            return v = r.NextDouble() * (min - max) + max;
        }

        /// <summary>
        /// Generates a random Mean Anomaly at Epoch
        /// </summary>
        public static double MeanAnomalyAtEpoch(double min, double max, Random r)
        {
            double v;
            return v = r.NextDouble() * (min - max) + max;
        }

        /// <summary>
        /// Generates a random Longitude of Ascending Node
        /// </summary>
        public static double LongitudeOfAscendingNode(double min, double max, Random r)
        {
            double v;
            return v = r.NextDouble() * (min - max) + max;
        }

        /// <summary>
        /// Generates a random Epoch
        /// </summary>
        public static double Epoch(double min, double max, Random r)
        {
            double v;
            return v = r.NextDouble() * (min - max) + max;
        }
    }
}
