using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Infinity.Datas.Querry;
using Infinity;

namespace Infinity.Generators
{
    public class PlanetGenerator
    {
        public static void MakePlernerts(int NumberStars, int MaxNumberMoonsPQS, string gameDataPath, int MaxNumberGasPlanets, int MaxNumberMoonsGas, int MaxNumberPQSPlanets = 5)
        {
            for (int s = 1; s <= NumberStars; s++)
            {
                Random random = new Random();
                int RadMultiplier = random.Next(55, 71);
                int PQSPlanets = random.Next(1, (MaxNumberPQSPlanets + 1));
                int GasPlanets = random.Next(1, (MaxNumberGasPlanets + 1));
                int MoonsNumPQS1 = 0;
                int MoonsNumPQS2 = 0;
                int MoonsNumPQS3 = 0;
                int MoonsNumPQS4 = 0;
                int MoonsNumPQS5 = 0;
                string[] MoonVars =
                {
                    "#VAR-STAR",  //1
                    "#VAR-PLNT",  //2
                    "#VAR-MN",    //3
                    "#VAR-RADM",  //4
                    "#VAR-REFM",  //5
                    "#VAR-SMAM",  //6
                    "#VAR-OCM",   //7
                    "#VAR-INCM",  //8
                    "#VAR-ECCM",  //9
                    "#VAR-LANM",  //10
                    "#VAR-AOPM",  //11
                    "#VAR-MAEM",  //12
                    "#VAR-DEFM",  //13
                    "#VAR-FRQ",   //14
                    "#VAR-OCT",   //15
                    "#VAR-PER",   //16
                    "#VAR-SEED"   //17
                };

                for (int i = 1; i <= PQSPlanets; i++)
                {
                    if (i == 1)
                        MoonsNumPQS1 = random.Next(0, (MaxNumberMoonsPQS + 1));
                    for (int m = 1; m <= MoonsNumPQS1; m++)
                    {
                        double inclination = RandomInclinationGeneration();
                        double radius = (random.Next(23, 56) * 1000);
                        double eccentricity = RandomEccentricityGeneration();
                        double SMA = (random.Next(1, 60) * 100000);
                        double MAE = RandomMeanAnomalyAtEpochGeneration();
                        double LAN = RandomLongitudeOfAscendingNodeGeneration();
                        double AOP = random.Next(0, 101) / 100;
                        float OrbitR = RandomColorGen();
                        float OrbitG = RandomColorGen();
                        float OrbitB = RandomColorGen();
                        string OrbitColor = Convert.ToString(OrbitR) + "," + Convert.ToString(OrbitG) + "," + Convert.ToString(OrbitB) + ",1";
                        int deformity = random.Next(1, 201);
                        int PQSSeed = random.Next(1000, 50001);
                        int frequency = random.Next(1, 4);
                        int Octaves = random.Next(1, 4);
                        float persistence = random.Next(1, 8) / 10;

                        string MoonConfig = LoadMoonPresets(gameDataPath);

                        MoonConfig = MoonConfig
                            .Replace("NEEDS[!Kopernicus]", "FOR[Infinity]")
                            .Replace(MoonVars[1], Convert.ToString(s))
                            .Replace(MoonVars[2], Convert.ToString(i))
                            .Replace(MoonVars[3], Convert.ToString(m))
                            .Replace(MoonVars[4], Convert.ToString(radius))
                            .Replace(MoonVars[5], ("Planet" + Convert.ToString(i)))
                            .Replace(MoonVars[6], Convert.ToString(SMA))
                            .Replace(MoonVars[7], OrbitColor)
                            .Replace(MoonVars[8], Convert.ToString(inclination))
                            .Replace(MoonVars[9], Convert.ToString(eccentricity))
                            .Replace(MoonVars[10], Convert.ToString(LAN))
                            .Replace(MoonVars[11], Convert.ToString(AOP))
                            .Replace(MoonVars[12], Convert.ToString(MAE))
                            .Replace(MoonVars[13], Convert.ToString(deformity))
                            .Replace(MoonVars[14], Convert.ToString(frequency))
                            .Replace(MoonVars[15], Convert.ToString(Octaves))
                            .Replace(MoonVars[16], Convert.ToString(persistence))
                            .Replace(MoonVars[17], Convert.ToString(PQSSeed));
                        File.WriteAllText(gameDataPath + "\\Infinity\\Planets\\Moons\\" + Convert.ToString(s) +  Convert.ToString(i) + Convert.ToString(m) + ".cfg", MoonConfig);
                    }
                    if (i == 2)
                        MoonsNumPQS2 = random.Next(0, (MaxNumberMoonsPQS + 1));
                    for (int m = 1; m <= MoonsNumPQS2; m++)
                    {
                        double inclination = RandomInclinationGeneration();
                        double radius = (random.Next(23, 56) * 1000);
                        double eccentricity = RandomEccentricityGeneration();
                        double SMA = (random.Next(1, 60) * 100000);
                        double MAE = RandomMeanAnomalyAtEpochGeneration();
                        double LAN = RandomLongitudeOfAscendingNodeGeneration();
                        double AOP = random.Next(0, 101) / 100;
                        float OrbitR = RandomColorGen();
                        float OrbitG = RandomColorGen();
                        float OrbitB = RandomColorGen();
                        string OrbitColor = Convert.ToString(OrbitR) + "," + Convert.ToString(OrbitG) + "," + Convert.ToString(OrbitB) + ",1";
                        int deformity = random.Next(1, 201);
                        int PQSSeed = random.Next(1000, 50001);
                        int frequency = random.Next(1, 4);
                        int Octaves = random.Next(1, 4);
                        float persistence = random.Next(1, 8) / 10;

                        string MoonConfig = LoadMoonPresets(gameDataPath);

                        MoonConfig = MoonConfig
                            .Replace("NEEDS[!Kopernicus]", "FOR[Infinity]")
                            .Replace(MoonVars[1], Convert.ToString(s))
                            .Replace(MoonVars[2], Convert.ToString(i))
                            .Replace(MoonVars[3], Convert.ToString(m))
                            .Replace(MoonVars[4], Convert.ToString(radius))
                            .Replace(MoonVars[5], ("Planet" + Convert.ToString(i)))
                            .Replace(MoonVars[6], Convert.ToString(SMA))
                            .Replace(MoonVars[7], OrbitColor)
                            .Replace(MoonVars[8], Convert.ToString(inclination))
                            .Replace(MoonVars[9], Convert.ToString(eccentricity))
                            .Replace(MoonVars[10], Convert.ToString(LAN))
                            .Replace(MoonVars[11], Convert.ToString(AOP))
                            .Replace(MoonVars[12], Convert.ToString(MAE))
                            .Replace(MoonVars[13], Convert.ToString(deformity))
                            .Replace(MoonVars[14], Convert.ToString(frequency))
                            .Replace(MoonVars[15], Convert.ToString(Octaves))
                            .Replace(MoonVars[16], Convert.ToString(persistence))
                            .Replace(MoonVars[17], Convert.ToString(PQSSeed));
                        File.WriteAllText(gameDataPath + "\\Infinity\\Planets\\Moons\\" + Convert.ToString(s) + Convert.ToString(i) + Convert.ToString(m) + ".cfg", MoonConfig);
                    }
                    if (i == 3)
                        MoonsNumPQS3 = random.Next(0, (MaxNumberMoonsPQS + 1));
                    for (int m = 1; m <= MoonsNumPQS3; m++)
                    {
                        double inclination = RandomInclinationGeneration();
                        double radius = (random.Next(23, 56) * 1000);
                        double eccentricity = RandomEccentricityGeneration();
                        double SMA = (random.Next(1, 60) * 100000);
                        double MAE = RandomMeanAnomalyAtEpochGeneration();
                        double LAN = RandomLongitudeOfAscendingNodeGeneration();
                        double AOP = random.Next(0, 101) / 100;
                        float OrbitR = RandomColorGen();
                        float OrbitG = RandomColorGen();
                        float OrbitB = RandomColorGen();
                        string OrbitColor = Convert.ToString(OrbitR) + "," + Convert.ToString(OrbitG) + "," + Convert.ToString(OrbitB) + ",1";
                        int deformity = random.Next(1, 201);
                        int PQSSeed = random.Next(1000, 50001);
                        int frequency = random.Next(1, 4);
                        int Octaves = random.Next(1, 4);
                        float persistence = random.Next(1, 8) / 10;

                        string MoonConfig = LoadMoonPresets(gameDataPath);

                        MoonConfig = MoonConfig
                            .Replace("NEEDS[!Kopernicus]", "FOR[Infinity]")
                            .Replace(MoonVars[1], Convert.ToString(s))
                            .Replace(MoonVars[2], Convert.ToString(i))
                            .Replace(MoonVars[3], Convert.ToString(m))
                            .Replace(MoonVars[4], Convert.ToString(radius))
                            .Replace(MoonVars[5], ("Planet" + Convert.ToString(i)))
                            .Replace(MoonVars[6], Convert.ToString(SMA))
                            .Replace(MoonVars[7], OrbitColor)
                            .Replace(MoonVars[8], Convert.ToString(inclination))
                            .Replace(MoonVars[9], Convert.ToString(eccentricity))
                            .Replace(MoonVars[10], Convert.ToString(LAN))
                            .Replace(MoonVars[11], Convert.ToString(AOP))
                            .Replace(MoonVars[12], Convert.ToString(MAE))
                            .Replace(MoonVars[13], Convert.ToString(deformity))
                            .Replace(MoonVars[14], Convert.ToString(frequency))
                            .Replace(MoonVars[15], Convert.ToString(Octaves))
                            .Replace(MoonVars[16], Convert.ToString(persistence))
                            .Replace(MoonVars[17], Convert.ToString(PQSSeed));
                        File.WriteAllText(gameDataPath + "\\Infinity\\Planets\\Moons\\" + Convert.ToString(s) + Convert.ToString(i) + Convert.ToString(m) + ".cfg", MoonConfig);
                    }
                    if (i == 4)
                        MoonsNumPQS4 = random.Next(0, (MaxNumberMoonsPQS + 1));
                    for (int m = 1; m <= MoonsNumPQS4; m++)
                    {
                        double inclination = RandomInclinationGeneration();
                        double radius = (random.Next(23, 56) * 1000);
                        double eccentricity = RandomEccentricityGeneration();
                        double SMA = (random.Next(1, 60) * 100000);
                        double MAE = RandomMeanAnomalyAtEpochGeneration();
                        double LAN = RandomLongitudeOfAscendingNodeGeneration();
                        double AOP = random.Next(0, 101) / 100;
                        float OrbitR = RandomColorGen();
                        float OrbitG = RandomColorGen();
                        float OrbitB = RandomColorGen();
                        string OrbitColor = Convert.ToString(OrbitR) + "," + Convert.ToString(OrbitG) + "," + Convert.ToString(OrbitB) + ",1";
                        int deformity = random.Next(1, 201);
                        int PQSSeed = random.Next(1000, 50001);
                        int frequency = random.Next(1, 4);
                        int Octaves = random.Next(1, 4);
                        float persistence = random.Next(1, 8) / 10;

                        string MoonConfig = LoadMoonPresets(gameDataPath);

                        MoonConfig = MoonConfig
                            .Replace("NEEDS[!Kopernicus]", "FOR[Infinity]")
                            .Replace(MoonVars[1], Convert.ToString(s))
                            .Replace(MoonVars[2], Convert.ToString(i))
                            .Replace(MoonVars[3], Convert.ToString(m))
                            .Replace(MoonVars[4], Convert.ToString(radius))
                            .Replace(MoonVars[5], ("Planet" + Convert.ToString(i)))
                            .Replace(MoonVars[6], Convert.ToString(SMA))
                            .Replace(MoonVars[7], OrbitColor)
                            .Replace(MoonVars[8], Convert.ToString(inclination))
                            .Replace(MoonVars[9], Convert.ToString(eccentricity))
                            .Replace(MoonVars[10], Convert.ToString(LAN))
                            .Replace(MoonVars[11], Convert.ToString(AOP))
                            .Replace(MoonVars[12], Convert.ToString(MAE))
                            .Replace(MoonVars[13], Convert.ToString(deformity))
                            .Replace(MoonVars[14], Convert.ToString(frequency))
                            .Replace(MoonVars[15], Convert.ToString(Octaves))
                            .Replace(MoonVars[16], Convert.ToString(persistence))
                            .Replace(MoonVars[17], Convert.ToString(PQSSeed));
                        File.WriteAllText(gameDataPath + "\\Infinity\\Planets\\Moons\\" + Convert.ToString(s) + Convert.ToString(i) + Convert.ToString(m) + ".cfg", MoonConfig);
                    }
                    if (i == 5)
                        MoonsNumPQS5 = random.Next(0, (MaxNumberMoonsPQS + 1));
                    for (int m = 1; m <= MoonsNumPQS5; m++)
                    {
                        double inclination = RandomInclinationGeneration();
                        double radius = (random.Next(23, 56) * 1000);
                        double eccentricity = RandomEccentricityGeneration();
                        double SMA = (random.Next(1, 60) * 100000);
                        double MAE = RandomMeanAnomalyAtEpochGeneration();
                        double LAN = RandomLongitudeOfAscendingNodeGeneration();
                        double AOP = random.Next(0, 101) / 100;
                        float OrbitR = RandomColorGen();
                        float OrbitG = RandomColorGen();
                        float OrbitB = RandomColorGen();
                        string OrbitColor = Convert.ToString(OrbitR) + "," + Convert.ToString(OrbitG) + "," + Convert.ToString(OrbitB) + ",1";
                        int deformity = random.Next(1, 201);
                        int PQSSeed = random.Next(1000, 50001);
                        int frequency = random.Next(1, 4);
                        int Octaves = random.Next(1, 4);
                        float persistence = random.Next(1, 8) / 10;

                        string MoonConfig = LoadMoonPresets(gameDataPath);

                        MoonConfig = MoonConfig
                            .Replace("NEEDS[!Kopernicus]", "FOR[Infinity]")
                            .Replace(MoonVars[1], Convert.ToString(s))
                            .Replace(MoonVars[2], Convert.ToString(i))
                            .Replace(MoonVars[3], Convert.ToString(m))
                            .Replace(MoonVars[4], Convert.ToString(radius))
                            .Replace(MoonVars[5], ("Planet" + Convert.ToString(i)))
                            .Replace(MoonVars[6], Convert.ToString(SMA))
                            .Replace(MoonVars[7], OrbitColor)
                            .Replace(MoonVars[8], Convert.ToString(inclination))
                            .Replace(MoonVars[9], Convert.ToString(eccentricity))
                            .Replace(MoonVars[10], Convert.ToString(LAN))
                            .Replace(MoonVars[11], Convert.ToString(AOP))
                            .Replace(MoonVars[12], Convert.ToString(MAE))
                            .Replace(MoonVars[13], Convert.ToString(deformity))
                            .Replace(MoonVars[14], Convert.ToString(frequency))
                            .Replace(MoonVars[15], Convert.ToString(Octaves))
                            .Replace(MoonVars[16], Convert.ToString(persistence))
                            .Replace(MoonVars[17], Convert.ToString(PQSSeed));
                        File.WriteAllText(gameDataPath + "\\Infinity\\Planets\\Moons\\" + Convert.ToString(s) + Convert.ToString(i) + Convert.ToString(m) + ".cfg", MoonConfig);
                    }
                }
            }
        }
        static string LoadMoonPresets(string path)
        {
            string Temp_MoonFile = File.ReadAllText(path + "\\Infinity\\Templates\\Asteroid_Moon.cfg");
            return Temp_MoonFile;
        }
        static string LoadGasPPresets(string path)
        {
            string Temp_GasFile = File.ReadAllText(path + "\\Infinity\\Templates\\Planets_Template_Gas.cfg");
            return Temp_GasFile;
        }
        /*static string LoadPQSPPresets(string path)
        {
            string Temp_PQSFile = File.ReadAllText(path + "\\Infinity\\Templates\\Planets_Template_PQS.cfg");
            return Temp_PQSFile;
        }*/
        //--------------ORBITAL GENERATORS--------------//
        public static double RandomInclinationGeneration(double minInclination = -90, double maxInclination = 90)
        {
            Random random = new Random();
            return random.NextDouble() * (maxInclination - minInclination) + minInclination;
        }
        public static double RandomEccentricityGeneration(double minEccentricity = 0, double maxEccentricity = 0.05)
        {
            Random random = new Random();
            return random.NextDouble() * (maxEccentricity - minEccentricity) + minEccentricity;
        }

        public static double RandomSemiMajorAxisGeneration(double minSMA, double galaxyRadius)
        {
            double lightYear = 9.461e+15; //1 Ly in meter

            Random random = new Random();
            return random.NextDouble() * ((galaxyRadius * lightYear) - minSMA) + minSMA;
        }

        public static double RandomMeanAnomalyAtEpochGeneration(double minMAE = 0, double maxMAE = 360)
        {
            Random random = new Random();
            return random.NextDouble() * (((maxMAE) - minMAE + minMAE) * (Math.PI * 2));
        }

        public static double RandomLongitudeOfAscendingNodeGeneration(double minLAN = 0, double maxLAN = 360)
        {
            Random random = new Random();
            return random.NextDouble() * (((maxLAN) - minLAN + minLAN) * (Math.PI * 10));
        }
        public static int RandomColorGen()
        {
            Random random = new Random();
            return (random.Next(0, 256) / 100);
        }
    }

}
