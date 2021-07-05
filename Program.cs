using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Wyklad1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentException("URL is needed");
            }

            string url = args[0];

            Regex urlRegex = new Regex("(http|ftp|https)://([\\w_-]+(?:(?:\\.[\\w_-]+)+))([\\w.,@?^=%&:/~+#-]*[\\w@?^=%&/~+#-])?",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Regex emailRegex = new Regex("\\w+@\\w+(\\.\\w+)+",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            MatchCollection matches = urlRegex.Matches(url);
            if(matches.Count<1)
            {
                throw new ArgumentException("That's not URL");
            }

            using (var httpClient = new HttpClient())
            {

                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    HashSet<string> set = new HashSet<string>();

                    matches = emailRegex.Matches(content);


                    foreach(Match match in matches)
                    {
                        set.Add(match.Value);
                    }


                    if (set.Count > 0)
                    {
                        foreach (String match in set)
                        {
                            Console.WriteLine(match);
                        }
                    }
                    else
                        Console.WriteLine("Nie znaleziono adresów email");
                }
                else
                    Console.WriteLine("Błąd w czasie pobierania strony");
            }

            //Console.WriteLine("Hello World!");
        }
    }
}
