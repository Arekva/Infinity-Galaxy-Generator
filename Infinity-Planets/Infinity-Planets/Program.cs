using System;
using System.IO;
using System.Collections.Generic;

namespace InfinityPlanets
{
    class Program
    {
        static void Main(string[] args)
        {
			Console.Title = "Infinity Planet Generator";
            string GDP = "PathTo GameData";
            string Name = "User";
            int NumberStar = 1;
            bool OPOS = false;
            
            bool Dev = false;
            if (File.Exists("C:\\Infinity\\Developer.INFINITY"))
            {
                Dev = true;
                GDP = File.ReadAllText("C:\\Infinity\\Developer.INFINITY");
                if (File.Exists("C:\\Infinity\\DevName.INFINITY"))
                {
                    Name = File.ReadAllText("C:\\Infinity\\DevName.INFINITY");
                }
                else
                {
                    Name = Environment.UserName;
                }
                if (File.Exists("C:\\Infinity\\Dev.INFINITY"))
                {
                    if (false)
                    {
                        //OPOS = true;
                    }
                }
            }
            else
            {
                Name = Environment.UserName;
            }
            Console.WriteLine("Hello! This is the Infinity Planet Generator, the companion to the Infinity Procedural Star Generator!");
            Console.WriteLine(" This program is Copyright (c) Mrcarrot 2018. The Procedural Star Generator is Copyright (c) Tutur 2018.");
            Console.WriteLine(" Both assemblies are All Rights Reserved.");
            if (!Dev)
                Console.WriteLine("Please tell me the filepath to your GameData, and I will make some plernerts!");
            while (true)
            {
                if (!Dev)
                    GDP = Console.ReadLine();
                else
                {

                    Console.WriteLine("Congratulations, " + Name + "! We have detected that you are a developer of the Infinity project, and you have bypassed");
                    Console.WriteLine("the checks for a proper GameData!");
                    Console.WriteLine("Proceed with caution.");


                    break;
                }
                if (File.Exists(GDP + "\\Infinity\\Templates\\GasPlanet.cfg") == true)
                    break;
                else
                {
                    if (!Dev)
                        Console.ForegroundColor = ConsoleColor.Red;
                    if (File.Exists(GDP + "\\Squad\\squadcore.ksp"))
                    {
                        Console.WriteLine("That GameData does not contain a vital file needed for the generation of your planets. Make sure you have Infinity");
                        Console.WriteLine("installed, " + Name + ".");
                    }
                    else
                    {
                        Console.WriteLine("That folder is not even a proper KSP GameData. Please try again");
                    }
                    Console.ResetColor();
                }
            }
            Logger.Setup(GDP);
            if(File.Exists(GDP + "\\Infinity\\SharedData\\StarCount.INFINITY"))
            {
                NumberStar = Convert.ToInt32(File.ReadAllText(GDP + "\\Infinity\\SharedData\\StarCount.INFINITY"));
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Star count not detected. Please ensure that you run the Star Generator first.");
                Console.ResetColor();
            }
            Console.WriteLine("Your GameData path is " + GDP);
            Console.WriteLine("Press any key to begin generation.");
            Console.ReadKey();
            string[] PlanetsList = Directory.GetFiles(GDP + "\\Infinity\\Planets");
            Console.WriteLine("Deleting old planet files.");
            Logger.Lawg(Logger.LogMessage("Deleting old planet files."));
            foreach (string file in PlanetsList)
            {
                File.Delete(file);
            }
            MakeSomePlernerts(GDP, NumberStar, OPOS);
            GasPlernerts(GDP, NumberStar);
            Console.WriteLine("Generation finished. \nLaunch KSP? y/n");
            bool KSP = false;
            bool KSPLauncher = false;
            if (Console.ReadLine() == "y")
                KSP = true;
            if(!KSP)
            {
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Use KSP Launcher? y/n");
                if (Console.ReadLine() == "y")
                    KSPLauncher = true;
                Console.WriteLine("Press any key to exit. KSP will be launched.");
                Console.ReadKey();
                LaunchKSP(Convert.ToString(Directory.GetParent(GDP)), KSPLauncher);
            }
        }
        /// <summary>
        /// Makes planets with PQS
        /// </summary>
        /// <param name="GDP"></param>
        /// <param name="numberStar"></param>
        /// <param name="OPOS"></param>
        static void MakeSomePlernerts(string GDP, int numberStar, bool OPOS)//I wanted MakeSomePlernerts!, but c#'s syntax wouldn't have liked that
        {
            //Planet variables
            string[] PlanetVars =
            {
                "#VAR-STAR",
                "#VAR-"
            };
            Random random = new Random();
            //Planet Variable definitions
            if (!OPOS)
            {
                for (int i = 1; i <= numberStar; i++)
                {
                    for(int p = 1; p <= random.Next(0, 5); p++)
                    {
                        string OrbitColor = RandomColor();
                        string PQSColor1 = "0." + random.Next(0, 256) + ",0." + random.Next(0, 256) + ",0." + random.Next(0, 256);
                        string PQSColor2 = "0." + random.Next(0, 256) + ",0." + random.Next(0, 256) + ",0." + random.Next(0, 256);
                        double PlanetRadius = random.Next(22, 95) * 100000;
                        string PQSPFile = LoadPQSPlanetPresets(GDP);
                        string GasPFile = LoadGasPlanetPresets(GDP);
                        int Seed = random.Next(1000, 90000);
                        double SMA = (random.Next(45 * p * p, 75 * p * p) * 10000000 / p);
                        SMA = Math.Abs(SMA);
                        string PQSConfig;
                        string ID = Convert.ToString(i) + "-" +  Convert.ToString(p);

                        PQSConfig = PQSPFile
                            .Replace("NEEDS[!Kopernicus]", "FOR[Infinity]")
                            .Replace(PlanetVars[0], Convert.ToString(i))
                            .Replace("#VAR-PID", ID)
                            .Replace("#VAR-RADP", Convert.ToString(PlanetRadius))
                            .Replace("#VAR-SMA", Convert.ToString(SMA))
                            .Replace("#VAR-SEED", Convert.ToString(Seed))
                            .Replace("#VAR-HCM1", PQSColor1)
                            .Replace("#VAR-HCM2", PQSColor2);
                        File.WriteAllText(GDP + "\\Infinity\\Planets\\PlanetPQS" + ID + ".cfg", PQSConfig);
                        Console.WriteLine("PQSPlanet" + ID);
                        Logger.Lawg(Logger.LogMessage("Generated Rocky Planet " + ID));
                    }
                }
            }
            else
            {
                //NOTHING!
            }
        }
        /// <summary>
        /// Generates gas planets with Jool Templates
        /// </summary>
        /// <param name="GDP"></param>
        /// <param name="numberStar"></param>
        static void GasPlernerts(string GDP, int numberStar)
        {
            Random random = new Random();
            string[] ColorPresets = //Handpicked by Mrcarrot and Tutur, randomly selected through random number generation- you won't have gas giants with colors other than these
            {
                "0.069,0.225,0.121",
                "0.245,0.155,0.0",
                "0.220,0.070,0.070",
                "0.218,0.225,0.035",
                "0.069,0.148,0.218",
                "0.023,0.146,0.217",
                "0.245,0.083,0.250"
            };
            for (int i = 1; i <= numberStar; i++)
            {
                for(int p = 1; p <= random.Next(1, 4); p++)
                {
                    string GasPFile = LoadGasPlanetPresets(GDP);
                    string GasPConfig0;
                    string GasPConfig1;
                    int TexID = random.Next(1, 6);
                    int radius = random.Next(500000, 2000000);
                    string color = ColorPresets[random.Next(1, 7)];
                    double SMA = (random.Next(55 * p, 95 * p) * 10000000);
                    //failsafe for negative SMA
                    SMA = Math.Abs(SMA);
                    string ID = Convert.ToString(i) + "-" + Convert.ToString(p);
                    double INC = RandomInclination(90, true);
                    GasPConfig0 = GasPFile
                        //Replace pass specifier so MM applies it
                        .Replace("NEEDS[!Kopernicus]", "FOR[INFINITY-PLNTS]")
                        //replace variables with values
                        .Replace("#VAR-IDP", ID)
                        .Replace("#VAR-TEX", Convert.ToString(TexID))
                        .Replace("#VAR-RAD", Convert.ToString(radius))
                        .Replace("#VAR-RBP", ("Star " + Convert.ToString(i)))
                        .Replace("#VAR-NSC", "0.0,0.0,0.0,0.3") //WIP
                        .Replace("#VAR-ATMOC", "0.0,0.0,0.0,0.3") //WIP
                        .Replace("#VAR-SSC", color)
                        .Replace("#VAR-SMA", Convert.ToString(SMA))
                        .Replace("#VAR-OBC", color);

                    //finishing up the description
                    if (TexID != 4)
                        GasPConfig1 = GasPConfig0
                            .Replace("#VAR-DESCE", ", except for the color.");
                    else
                        GasPConfig1 = GasPConfig0
                            .Replace("#VAR-DESCE", "!");
                    File.WriteAllText(GDP + "\\Infinity\\Planets\\GasPlanet" + ID + ".cfg", GasPConfig1);
                    Console.WriteLine("GasPlanet" + ID);
                    Logger.Lawg(Logger.LogMessage("Generated Gas Planet" + ID));
                }
            }
        }
        
        static string LoadGasPlanetPresets(string GDP)
        {
            string Config = File.ReadAllText(GDP + "/Infinity/Templates/GasPlanet.cfg");
            return Config;
        }
        static string LoadPQSPlanetPresets(string GDP)
        {
            string Config = File.ReadAllText(GDP + "/Infinity/Templates/PQSPlanet.cfg");
            return Config;
        }

        static string RandomColor()
        {
            Random random = new Random();
            string color = "0." + random.Next(0, 256) + ",0." + random.Next(0, 256) + ",0." + random.Next(0, 256);
            return color;
        }
        static int VeryRandom(int param1, int param2)
        {
            Random rnd = new Random();
            int Output;
            int int1;
            int int2;
            int int3;
            int int4;
            int1 = rnd.Next(param1, param2);
            int2 = rnd.Next(param1, param2);
            int3 = rnd.Next(param1, param2);
            int4 = rnd.Next(param1, param2);
            Output = Convert.ToInt32(Math.Round((double)((int1 + int2 + int3 + int4) / 4)));
            return Output;
        }
        static float RandomECC(int MaxECC)
        {
            Random rnd = new Random();
            float output = (rnd.Next(0, MaxECC) / 10);
            return output;
        }
        static double RandomInclination(int maxINC, bool AllowRetrograde)
        {
            Random rnd = new Random();
            double output;
            output = rnd.Next((0 - maxINC), maxINC);

            if(AllowRetrograde = true && rnd.Next(1,3) == 2)
            {
                output = output + 180;
            }

            return output;
        }
        static void LaunchKSP(string RootFolder, bool UseLauncher)
        {
            if(UseLauncher)
            {
                System.Diagnostics.Process.Start(RootFolder + "\\Launcher.exe");
            }
            else
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    System.Diagnostics.Process.Start(RootFolder + "\\KSP_x64.exe");
                }
                else
                {
                    System.Diagnostics.Process.Start(RootFolder + "\\KSP.exe");
                }
            }
        }
    }
    namespace Moons
    {
        class Asteroid
        {
            static void Generate(string GDP, int Stars, string ParentID, int MoonNum)
            {
                string BaseConfig = LoadAsteroidMoonPresets(GDP);
                Random random = new Random();
                for (int p = 1; p <= 1; p++)
                {
                    string NewConfig;
                    int radius = random.Next(500000, 2000000);
                    double SMA = (random.Next(55 * p, 95 * p) * 10000000);
                    //failsafe for negative SMA
                    SMA = Math.Abs(SMA);
                    string ID = (ParentID + "-" + Convert.ToString(MoonNum));
                    NewConfig = BaseConfig
                        //Replace pass specifier so MM applies it
                        .Replace("NEEDS[!Kopernicus]", "FOR[INFINITY-PLNTS-MOONS]")
                        //replace variables with values
                        .Replace("#VAR-MNID", ID);

                    //finishing up the description
                    File.WriteAllText(GDP + "\\Infinity\\Planets\\GasPlanet" + ID + ".cfg", NewConfig);
                    Console.WriteLine("GasPlanet" + ID);
                    Logger.Lawg(Logger.LogMessage("Generated Gas Planet" + ID));
                }
            }
            static string LoadAsteroidMoonPresets(string GDP)
            {
                string Config = File.ReadAllText(GDP + "/Infinity/Templates/AsteroidMoon.cfg");
                return Config;
            }
            static double RandomRadius(int min, int max)
            {
                Random random = new Random();
                return random.Next(min, max);
            }
        }
    }
}