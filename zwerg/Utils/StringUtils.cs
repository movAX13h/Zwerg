using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Utils
{
    class StringUtils
    {
        public static List<string> CubemapFilenames(string startName)
        {
            List<string> names = new List<string>();

            Match match = Regex.Match(startName, @"^(.*_)([0-9])(\.[a-z]*)$", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                string a = match.Groups[1].Value;
                //string b = match.Groups[2].Value;
                string c = match.Groups[3].Value;
                for (int i = 0; i < 6; i++) names.Add(a + i.ToString() + c);
            }

            return names;
        }

        

    }
}
