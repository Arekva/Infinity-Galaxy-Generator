using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Infinity.Datas;
using System.Reflection;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Diagnostics;

namespace Infinity.Generators
{
    class NewGenerator
    {
        //Generates a random star
        public static void StarDictionary(Dictionary<string, Dictionary<string, string>> starDatabase, Dictionary<string, double> galaxySettings, string gameDataPath, Random random)//galaxySetings is used for the orbit generator
        {
            //Generates the orbit
            Dictionary<string, double> orbitDouble = Orbit.Star(galaxySettings, random);

            Dictionary<string, string> orbitString = new Dictionary<string, string>();
        }
    }
}
