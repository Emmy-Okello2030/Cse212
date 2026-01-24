using System.Text.Json;
using System.Net.Http;
using System.IO;
using System.Collections.Generic;
using System;

public static class SetsAndMaps
{
    // =========================
    // Problem 1 — FindPairs
    // =========================
    public static string[] FindPairs(string[] words)
    {
        var seen = new HashSet<(char, char)>();
        var result = new List<string>();

        foreach (var word in words)
        {
            char c1 = word[0];
            char c2 = word[1];
            
            // Skip if word has the same character twice
            if (c1 == c2) continue;
            
            if (seen.Contains((c2, c1)))
            {
                result.Add($"{word} & {c2}{c1}");
            }
            else
            {
                seen.Add((c1, c2));
            }
        }

        return result.ToArray();
    }

    
    // =========================
    // Problem 2 — SummarizeDegrees
    // =========================
    public static Dictionary<string, int> SummarizeDegrees(string filename)
    {
        var degrees = new Dictionary<string, int>();

        foreach (var line in File.ReadLines(filename))
        {
            var fields = line.Split(",");
            string degree = fields[3];

            if (degrees.ContainsKey(degree))
                degrees[degree]++;
            else
                degrees[degree] = 1;
        }

        return degrees;
    }

    // =========================
    // Problem 3 — IsAnagram
    // =========================
    public static bool IsAnagram(string word1, string word2)
    {
        var counts = new Dictionary<char, int>();

        foreach (char c in word1.ToLower())
        {
            if (c == ' ') continue;
            counts[c] = counts.GetValueOrDefault(c) + 1;
        }

        foreach (char c in word2.ToLower())
        {
            if (c == ' ') continue;

            if (!counts.ContainsKey(c))
                return false;

            counts[c]--;
            if (counts[c] == 0)
                counts.Remove(c);
        }

        return counts.Count == 0;
    }

    // =========================
    // Problem 5 — EarthquakeDailySummary
    // =========================
    public static string[] EarthquakeDailySummary()
    {
        const string uri =
            "https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_day.geojson";

        using var client = new HttpClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, uri);
        using var stream = client.Send(request).Content.ReadAsStream();
        using var reader = new StreamReader(stream);

        var json = reader.ReadToEnd();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var featureCollection =
            JsonSerializer.Deserialize<FeatureCollection>(json, options);

        var results = new List<string>();

        foreach (var feature in featureCollection.Features)
        {
            if (feature.Properties.Mag.HasValue)
            {
                results.Add($"{feature.Properties.Place} - Mag {feature.Properties.Mag}");
            }
        }

        return results.ToArray();
    }
}