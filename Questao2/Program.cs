using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class Program
{
    public static async Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);
    }

    public static async Task<int> getTotalScoredGoals(string team, int year)
    {
        int totalGoals = 0;
        using (var httpClient = new HttpClient())
        {
            int page = 1;
            while (true)
            {
                string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}&page={page}";
                var response = await httpClient.GetStringAsync(url);
                var result = JsonConvert.DeserializeObject<dynamic>(response);

                foreach (var match in result.data)
                {
                    totalGoals += int.Parse(match.team1goals.Value);
                }

                url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team2={team}&page={page}";
                response = await httpClient.GetStringAsync(url);
                result = JsonConvert.DeserializeObject<dynamic>(response);

                foreach (var match in result.data)
                {
                    totalGoals += int.Parse(match.team2goals.Value);
                }

                if (result.data.Count == 0)
                {
                    break;
                }

                page++;
            }
        }
        return totalGoals;
    }
}
