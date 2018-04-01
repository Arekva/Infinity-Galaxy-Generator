using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using Infinity.Generators;
using Infinity.Datas;
using Infinity.Datas.Query;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Infinity
{
    class Program
    {
        public static void Main(string[] args)
        {
            //I still prefer comma system
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            //------Global Variables------//
            bool Dev = false;


            bool ok = false;

            int errorDisplayTime = 750;

            string gameDataPath = "Path To GameData"; //Placeholder so it doesn't throw an error
            if (File.Exists("C:\\Infinity\\Developer.INFINITY"))
            {
                Dev = true;
                gameDataPath = File.ReadAllText("C:\\Infinity\\Developer.INFINITY");
            }
            int starNumber;
            double galaxySize;
            int galaxyType;

            Random randomSeed = new Random();
            //Seed Generation--
            int seed = randomSeed.Next(int.MinValue, int.MaxValue);
            //-----------------

            double[] defaultValues =
            {
                50,     //0 - Star Number
                2,      //1 - Galaxy size
                1,      //2 - Galaxy Type (1 stands for Spiral, 2 for Elliptical
            };

            //----------------------------//

            Dictionary<string, Dictionary<string, string>> starDatas = Datas.Star.ComputeStarData();

            //User's GameData input checking
            while (true)
            {
                if (!Dev)
                {
                    Console.WriteLine("Welcome in Infinity, the procedural Galaxy generator!\n\nPlease enter here your GameData folder path:");

                    gameDataPath = Console.ReadLine();

                    if (InputCheck.GameData(gameDataPath) == true)
                        break;
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("GameData folder incorrect, retry with a correct one.");
                        Console.ResetColor();

                        Thread.Sleep(errorDisplayTime);

                        Console.Clear();
                    }
                }
                else
                {
                    Console.WriteLine("Congratulations! I have detected that you are a developer. Your GameData is located at: " + gameDataPath + ".");
                    Console.WriteLine("You have also bypassed the checks for a proper GameData. Live on the edge, but be careful.");
                    break;
                }
            }

            //User's number of star input checking
            while (true)
            {
                Console.WriteLine("\nHow many stars do you want in your galaxy?\n\n" +
                    "Recommended:\n" +
                    "-Small configs:  50 ~100\n" +
                    "-Middle configs: 100~250\n" +
                    "-High configs:   250~500");

                string input = Console.ReadLine();

                InputCheck.StarNumber(input, out ok, out starNumber);

                if (ok)
                {
                    File.WriteAllText(gameDataPath + "\\Infinity\\SharedData\\StarCount.INFINITY", Convert.ToString(starNumber));
                    break;
                }


                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Number incorrect, retry with a correct one (>0 or / and integer.)");
                    Console.ResetColor();

                    Thread.Sleep(errorDisplayTime * 2);

                    Console.Clear();
                }
            }

            //User's galaxy size input checking
            while (true)
            {
                ok = false;

                Console.WriteLine("\nWrite here the radius of your Galaxy in Light-Years\n" +
                    "(minimal value is 0.01 Ly, max is what ksp can support, this means you have to be careful with high values.");

                string input = Console.ReadLine();

                InputCheck.GalaxySize(input, out ok, out galaxySize);

                if (ok)
                    break;

                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Number incorrect, retry with a correct one (>0.01)");
                    Console.ResetColor();

                    Thread.Sleep(errorDisplayTime);

                    Console.Clear();
                }
            }
            
            //User's advanced mode inputs
            while (true)
            {
                ok = false;
                Console.WriteLine("\nDo you want to access to the advanced settings? (y/n)");

                string input = Console.ReadLine();

                InputCheck.AdvancedSettings(input, out ok, out input);

                if (ok)
                {
                    if (input.Equals("n"))
                    {
                        galaxyType = (int)defaultValues[2]; //Spiral Galaxy

                    }
                    else
                    {
                        while (true)//Galaxy type choice
                        {
                            Console.WriteLine("Choose the type of galaxy:\n\n1 - Spiral (Default)\n2 - Elliptical\n");

                            input = Console.ReadLine();

                            InputCheck.GalaxyChoice(input, out ok, out int inputInt);

                            if (ok && (inputInt == 1 || inputInt == 2))
                            {
                                galaxyType = inputInt;
                                break;
                            }

                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Number incorrect, retry with a correct one (integral number)");
                                Console.ResetColor();

                                Thread.Sleep(errorDisplayTime * 2);

                                Console.Clear();
                            }
                        }
                        while (true) //Seed choice
                        {
                            ok = false;

                            Console.WriteLine("\nEnter a custom seed (Leave empty to use random)");

                            input = Console.ReadLine();

                            if (input.Equals("")) break;
                            else seed = input.GetHashCode(); break;
                                
                        }
                    }
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Bad choice, retry with a correct one (write 'n' (no) or 'y' (yes))");
                    Console.ResetColor();

                    Thread.Sleep(errorDisplayTime * 3);

                    Console.Clear();
                }

            }

            Random random = new Random(seed);

            //User's choice on delete;
            while (true)
            {
                ok = false;
                Console.WriteLine("\nAre you sure to rebuild a whole new galaxy? The old one will be deleted and saves will be unusable (y/n)");

                string input = Console.ReadLine();

                InputCheck.LastChoice(input, out ok, out input);

                if (input.Equals("n"))
                {
                    Console.WriteLine("\nOk well bye, so.");
                    Thread.Sleep(errorDisplayTime);
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Hold on some times, the program is removing old files and creating new ones...");
                    break;
                }
            }

            //------------Dictionaries with all values, ready to be put in the generator !
            //Doubles--
            Dictionary<string, double> galaxySettings = new Dictionary<string, double>();
            galaxySettings.Add("starNumber", starNumber);
            galaxySettings.Add("galaxySize", galaxySize);
            galaxySettings.Add("galaxyType", galaxyType);

            //---------------------------------------------------------------------------

            //Saves the seed into a file
            File.WriteAllText(gameDataPath + @"\Infinity\SharedData\Seed.INFINITY", ("" + seed));


            //--Removing stars beforehand created
            Console.WriteLine("\nRemoving old star systems..");
            string[] starFileList = Directory.GetFiles(gameDataPath + @"\Infinity\StarSystems\Stars", "*.cfg");
            string[] planetFileList = Directory.GetFiles(gameDataPath + @"\Infinity\Planets", "*.cfg");
            string[] moonFileList = Directory.GetFiles(gameDataPath + @"\Infinity\Planets\Moons", "*.cfg");
            string[] planetHabZoneList = Directory.GetFiles(gameDataPath + @"\Infinity\StarSystems\Planets", "*.cfg");
            string[] wormholes = Directory.GetFiles(gameDataPath + @"\Infinity\StarSystems\Wormholes", "*.cfg");

            foreach (string file in starFileList)
            {
                File.Delete(file);
            }
            //"No touchy" -Carrot       "You sure?" -Tutur
            /*foreach (string file in planetFileList)
            {
                File.Delete(file);
            }
            foreach (string file in moonFileList)
            {
                File.Delete(file);
            }*/
            foreach (string file in planetHabZoneList)
            {
                File.Delete(file);
            }
            foreach (string file in wormholes)
            {
                File.Delete(file);
            }
            //---------------

            Console.WriteLine("\nGenerating the galaxy..");
            Generator.Galaxy(gameDataPath, galaxySettings, starDatas, random);

            Console.WriteLine("\nGalaxy generated. Have fun!");
            //Exit function
            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }
    }
}
