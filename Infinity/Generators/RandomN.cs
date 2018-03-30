using System;
using System.Threading;
namespace Infinity.Generators
{
    /// <summary>
    /// Generates a random value of the type you want
    /// </summary>
    class RandomN
    {
        /// <summary>
        /// Generates a random double between 0 and 1
        /// </summary>
        /// <returns></returns>
        public static double Double(Random random)
        {
            //Thread.Sleep(3);
            return random.NextDouble();
        }

        /// <summary>
        /// Generates a random Int32 between a minimal and maximal value
        /// </summary>
        /// <returns></returns>
        public static int Int32(Random random, int minValue, int maxValue)
        {
            //Thread.Sleep(3);
            return random.Next(minValue, maxValue + 1);
        }
    }
}
