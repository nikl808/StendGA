using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace stend
{
    class ConfigFileParser 
    {
        //Comboboxes items
        private Dictionary<string, Dictionary<string, string[]>> parseCfg;

        public ConfigFileParser() { parseCfg = new Dictionary<string, Dictionary<string, string[]>>(); }

        public void Parse(string[] ReadFile)
        {
            int first = 0;
            int end = 0;
            string tag = "null";
            int i = 0;
            while (ReadFile[i] != null)
            {
                //Read tab tag
                if (ReadFile[i].Contains('[') || ReadFile[i].Contains(']'))
                {
                    first = ReadFile[i].IndexOf('[') + 1;
                    end = ReadFile[i].IndexOf(']');
                    tag = ReadFile[i].Substring(first, end - first);
                    parseCfg.Add(tag, new Dictionary<string, string[]>());
                }
                //Read comboboxes items for current tab
                else if (ReadFile[i] != "" && tag != "null")
                {
                    first = ReadFile[i].IndexOf('=');
                    end = ReadFile[i].IndexOf(';') + 1;
                    string[] str = new string[50];
                    int j = 0, start = first + 1;

                    while (start != end)
                    {
                        int next = ReadFile[i].IndexOf(',', start);
                        if (next == -1) next = ReadFile[i].IndexOf(';', start);
                        str[j] = ReadFile[i].Substring(start, next - start);
                        str[j].Trim();
                        start = (next + 1);
                        j++;
                    }
                    //fit the final string array
                    Array.Resize(ref str, j);
                    parseCfg[tag].Add(ReadFile[i].Substring(0, first).Trim(), str);
                }
                i++;
            }
        }

        public Dictionary<string, string[]> GetSetting(string settingName)
        {
            foreach (KeyValuePair<string, Dictionary<string, string[]>> keyVal in parseCfg)
            {
                if (keyVal.Key == settingName) return keyVal.Value;
                else Error.instance.HandleErrorMessage(settingName + " Not Found");
            }
            return null;
        }
    }
}
