namespace LisLasParser
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    internal class Program
    {
        private static int Main(string[] args)
        {
            var allFiles = GetDirectoryFiles().ToList();
            if(!allFiles.Any())
                return CLIError("Not found files");
            Console.WriteLine(string.Join("\n", allFiles));
            return GetLog(allFiles);
        }

        private static IEnumerable<string> GetDirectoryFiles() =>
            Directory.GetFiles(Directory.GetCurrentDirectory(), "*.las", SearchOption.AllDirectories);

        private static int GetLog(IEnumerable<string> directory)
        {
            foreach (var i in directory)
            {
                using var sr = new StreamReader(i);


                var line = sr.ReadToEnd();
                var rgxLog =
                    new Regex(
                        @"~PARAMETER\ INFORMATION\ \(log\)([\s\S]*LTYP\.\s*([\s\S]*):\s*LOG\ TYPE[\s\S]*)~Curve\ Information\ Block");
                var matches = rgxLog.Matches(line);
                if (matches.Count <= 0)
                    return CLIError("Not found log");

                foreach (Match match in matches)//тут будет сохранение в кэш
                {
                    Console.WriteLine(match.Groups[1].Value);
                    Console.WriteLine("Log Type : {0}", match.Groups[2].Value);
                }
            }

            return 0;
        }
        
        public static int CLIError(string text)
        {
            lock (guarder)
            {
                var tmpColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(text);
                Console.ForegroundColor = tmpColor;
            }

            return 1;
        }

        private static readonly object guarder = new object();

        private class Cash
        {
            private string directory;
            private string logType;
            private string log;
        }
    }
}