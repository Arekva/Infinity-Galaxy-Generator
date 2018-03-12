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
        public static double Double()
        {
            Thread.Sleep(3);
            Random random = new Random();
            return random.NextDouble();
        }

        /// <summary>
        /// Generates a random Int32 between a minimal and maximal value
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <returns></returns>
        public static int Int32(int minValue, int maxValue)
        {
            Thread.Sleep(3);
            Random random = new Random();
            return random.Next(minValue, maxValue + 1);
        }
    }
}
