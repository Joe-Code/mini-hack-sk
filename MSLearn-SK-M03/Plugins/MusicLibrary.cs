using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.SemanticKernel;

public class MusicLibraryPlugin
{
    [KernelFunction, Description("Get a list of music recently played by the user")]
    public static string GetRecentPlays()
    {
        string dir = Directory.GetCurrentDirectory();
        string filePath = $"{dir}/data/recentlyplayed.txt";

        if (!File.Exists(filePath))
        {
            return "No recently played songs found.";
        }

        string content = File.ReadAllText(filePath);
        return content;
    }

    [KernelFunction, Description("Add a song to the recently played list")]
    public static string AddToRecentlyPlayed(
        [Description("The name of the artist")] string artist,
        [Description("The title of the song")] string song,
        [Description("The song genre")] string genre)
    {
        string filePath = "data/recentlyplayed.txt";

        // Ensure the file exists
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }

        string jsonContent = File.ReadAllText(filePath);
        JsonArray recentlyPlayed = JsonNode.Parse(jsonContent) as JsonArray ?? new JsonArray();

        var newSong = new JsonObject
        {
            ["title"] = song,
            ["artist"] = artist,
            ["genre"] = genre
        };

        recentlyPlayed.Insert(0, newSong);
        try
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(recentlyPlayed, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch (Exception ex)
        {
            return $"An error occurred while writing to the file: {ex.Message}";
        }

        return $"Added '{song}' to recently played";
    }
}