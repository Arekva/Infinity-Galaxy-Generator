using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinity
{
    class InputCheck
    {
        /// <summary>
        /// Class for input checking, in case if users are apes (lol)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        
        public static bool GameData(string input)
        {
            bool ok = false;

            string safetyFile = input + "\\Squad\\squadcore.ksp";

            if (File.Exists(safetyFile))
            {
                ok = true;
            }

            return ok;
        }

        public static void StarNumber(string input, out bool ok, out int output)
        {
            ok = Int32.TryParse(input, out output);
        }

        public static void GalaxySize(string input, out bool ok, out double output)
        {
            input = input.Replace(".", ","); //Converts to French / German decimal system (don't ask me why C# has this one and not USA's)
            ok = Double.TryParse(input, out output);
        }

        public static void AdvancedSettings(string input, out bool ok, out string output)
        {
            ok = false;
            output = "";

            if(input.Equals("y") || input.Equals("Y"))
            {
                ok = true;
                output = "y";
            }
            else if (input.Equals("n") || input.Equals("N"))
            {
                ok = true;
                output = "n";
            }
            else
            {
                ok = false;
            }

        }

        public static void GalaxyChoice(string input, out bool ok, out int output)
        {
            ok = Int32.TryParse(input, out output);
        }

        public static void LastChoice(string input,out bool ok, out string output)
        {
            ok = false;
            output = "";

            if (input.Equals("y") || input.Equals("Y"))
            {
                ok = true;
                output = "y";
            }
            else if (input.Equals("n") || input.Equals("N"))
            {
                ok = true;
                output = "n";
            }
            else
            {
                ok = false;
            }
        }

        public static void Seed(string input, out bool ok, out int output)
        {
            ok = Int32.TryParse(input, out output);
        }
    }
}
