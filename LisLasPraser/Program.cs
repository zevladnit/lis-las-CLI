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
            if(allFiles.Contains(null)) CLIError("Not found files");
            Console.WriteLine(string.Join("\n", allFiles));
            GetLog(allFiles);
        }

        private static IEnumerable<string> GetDirectoryFiles() =>
            Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.*", SearchOption.AllDirectories)
                .Where(p => p.EndsWith(".LIS") || p.EndsWith(".LAS") || p.EndsWith(".lis") || p.EndsWith(".las"));

        private static void GetLog(List<string> directory)
        {
            foreach (var i in directory)
            {
                using (var sr = new StreamReader(i))
                {
                    var line = sr.ReadToEnd();
                    var rgxLog =
                        new Regex(
                            @"~PARAMETER\ INFORMATION\ \(log\)([\s\S]*LTYP\.\s*([\s\S]*):\s*LOG\ TYPE[\s\S]*)~Curve\ Information\ Block");
                    var matches = rgxLog.Matches(line);
                    if (matches.Count <= 0) CLIError("Not found log");

                    foreach (Match match in matches)//тут будет сохранение в кэш
                    {
                        Console.WriteLine(match.Groups[1].Value);
                        Console.WriteLine("Log Type : {0}", match.Groups[2].Value);
                    }
                }
            }
        }

        public static void CLIError(string text)
        {
            var tmpColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = tmpColor;
            Environment.Exit(0);
        }

        private class Cash
        {
            private string directory;
            private string logType;
            private string log;
        }
    }
}