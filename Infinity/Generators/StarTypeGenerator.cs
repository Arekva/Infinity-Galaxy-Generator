using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using System.Threading.Tasks;

namespace Infinity.Generators
{
    class StarTypeGenerator
    {
        //Loads all the configs
        public static Dictionary<string, string>[] StarsSettingsLoader(string gameDataPath)
        {
            gameDataPath += "\\Infinity\\Settings\\";

            //Loads M class settings
            Dictionary<string, string> MClassSettings = File.ReadAllLines(gameDataPath + "M Class Settings.txt")
                .Select(l => l.Split(new[] { '=' }))
                .ToDictionary(s => s[0].Trim(), s => s[1].Trim());

            //Load G class settings
            Dictionary<string, string> GClassSettings = File.ReadAllLines(gameDataPath + "G Class Settings.txt")
                .Select(l => l.Split(new[] { '=' }))
                .ToDictionary(s => s[0].Trim(), s => s[1].Trim());

            //Loads O class settings
            Dictionary<string, string> OClassSettings = File.ReadAllLines(gameDataPath + "O Class Settings.txt")
                .Select(l => l.Split(new[] { '=' }))
                .ToDictionary(s => s[0].Trim(), s => s[1].Trim());

            //Packing up all the dictionaries in one array, simple to send after that, yk, like IRL packages: 1 is simpler to send and store than 3 (*i think?*)
            Dictionary<string, string>[] starSettingsDics =
            {
                MClassSettings,     //0
                OClassSettings,     //1
                GClassSettings      //2
            };

            return starSettingsDics; //Hop ! Sent by the post.
        }

        public static Dictionary<string, int> GenerateNumberEachStarClass(
            Dictionary<string, string>[] starConfigsDics, int starNumber)
        {
            double mFreq = 0;
            double gFreq = 0;
            double oFreq = 0;

            int mTotal = 0;
            int gTotal = 0;
            int oTotal = 0;

            //Takes frequency data in the star settings
            Console.WriteLine("\nRecovering frequency datas..");
            for(int i = 0; i < starConfigsDics.Length; i++)
            {
                //M Class
                if ((starConfigsDics[i]["spectralclass"]) == "M")
                    foreach (var pair in starConfigsDics[i])
                        if(pair.Key == "frequency")
                            Double.TryParse(pair.Value, out mFreq);

                //G Class
                if ((starConfigsDics[i]["spectralclass"]) == "G")
                    foreach (var pair in starConfigsDics[i])
                        if (pair.Key == "frequency")
                            Double.TryParse(pair.Value, out gFreq);
                //O Class
                if ((starConfigsDics[i]["spectralclass"]) == "O")
                    foreach (var pair in starConfigsDics[i])
                        if (pair.Key == "frequency")
                            Double.TryParse(pair.Value, out oFreq);

            }
            //--

            //Calculate nber of star per type
            Console.WriteLine("\nCalculating the number of star by classes..");
            for (int i = 0; i < starNumber; i++)
            {
                //Generates a number used to know what star type will be generated   
                Random random = new Random();
                double freq = random.NextDouble() * 100;

                Thread.Sleep(10);

                if(freq <= mFreq) //Generating M Class
                    mTotal++;

                else if((freq > mFreq) && (freq <= mFreq + gFreq)) //Generating G class
                    gTotal++;

                else if(freq > (mFreq + gFreq)) //Generating O class
                    oTotal++;
            }

            //Packaging of that
            Dictionary<string, int> freqForEachStar = new Dictionary<string, int>();
            Console.WriteLine(Environment.NewLine);
            freqForEachStar.Add("M", mTotal);
            freqForEachStar.Add("G", gTotal);
            freqForEachStar.Add("O", oTotal);

            return freqForEachStar;
        }
    }
}
