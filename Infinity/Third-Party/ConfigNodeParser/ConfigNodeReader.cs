using System;
using System.IO;
using System.Text;

namespace ConfigNodeParser
{
    public class ConfigNodeReader
    {
        public static ConfigNode StringToConfigNode(string inputString)
        {
            ConfigNode node = null;
            using (StringReader sr = new StringReader(inputString))
            {
                node = BuildConfigNode(sr, null);
            }
            return node;
        }

        public static ConfigNode BuildConfigNode(StringReader sr, string name)
        {
            ConfigNode node = new ConfigNode();
            node.name = name;
            string previousLine = null;
            string currentLine = null;
            while ((currentLine = sr.ReadLine()) != null)
            {
                string trimmedLine = currentLine.TrimStart();
                if (trimmedLine == "{")
                {
                    node.AddConfigNode(BuildConfigNode(sr, previousLine));
                }
                if (trimmedLine == "}")
                {
                    return node;
                }
                if (trimmedLine.Contains(" = "))
                {
                    string pairKey = trimmedLine.Substring(0, trimmedLine.IndexOf(" = "));
                    string pairValue = trimmedLine.Substring(trimmedLine.IndexOf(" = ") + 3);
                    node.AddValue(pairKey, pairValue);
                }
                previousLine = trimmedLine;
            }
            return node;
        }

        public static ConfigNode FileToConfigNode(string inputFile)
        {
            string configNodeText = File.ReadAllText(inputFile);
            return StringToConfigNode(configNodeText);
        }
    }
}

