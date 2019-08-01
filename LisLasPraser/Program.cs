using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LisLasParser
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var allFiles = GetDirectoryFiles().ToList();
            Console.WriteLine(string.Join("\n", allFiles));
            GetLog(allFiles[int.Parse(Console.ReadLine())]);
        }

        private static IEnumerable<string> GetDirectoryFiles() =>
            Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.*", SearchOption.AllDirectories)
                .Where(p => p.EndsWith(".LIS") || p.EndsWith(".LAS"));

        private static void GetLog(string directory)
        {
            using (var sr = new StreamReader(directory))
            {
                var line = sr.ReadToEnd();
                var rgxLog = new Regex(@"~PARAMETER\ INFORMATION\ \(log\)([\s\S]*LTYP\.\s*([\s\S]*):\s*LOG\ TYPE[\s\S]*)~Curve\ Information\ Block");
                var matches = rgxLog.Matches(line);
                if (matches.Count <= 0)// Error "Not found" if Matches result is empty 
                {
                    var tmpColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Not found log");
                    Console.ForegroundColor = tmpColor;
                    return;
                }

                foreach (Match match in matches)
                {
                    Console.WriteLine(match.Groups[1].Value);
                    Console.WriteLine("Log Type : {0}", match.Groups[2].Value);
                }
            }
        }
    }
}


