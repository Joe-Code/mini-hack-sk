
using Microsoft.SemanticKernel;

namespace MSLearn_SK_M03
{
    public class MusicLibrarySample
    {
        public static async Task AddToRecentlyPlayed(IKernelBuilder kernelBuilder)
        {
            Console.WriteLine("Adding a song to your Music Library!\n");

            Kernel kernel = kernelBuilder.Build();
            kernel.ImportPluginFromType<MusicLibraryPlugin>();

            var result = await kernel.InvokeAsync(
                "MusicLibraryPlugin",
                "AddToRecentlyPlayed",
                new()
                {
                    ["artist"] = "Dire Straits",
                    ["song"] = "Money for Nothing",
                    ["genre"] = "rock, pop"
                }
            );

            Console.WriteLine(result);
        }

        public static async Task SuggestSongFromPlaylist(IKernelBuilder kernelBuilder)
        {
            Console.WriteLine("Suggesting a song from your Music Library!\n");

            Kernel kernel = kernelBuilder.Build();
            kernel.ImportPluginFromType<MusicLibraryPlugin>();

            string prompt = @"This is a list of music available to the user:
                            {{MusicLibraryPlugin.GetMusicLibrary}} 

                            This is a list of music the user has recently played:
                            {{MusicLibraryPlugin.GetRecentPlays}}

                            Based on their recently played music, suggest a song from
                            the list to play next";

            var result = await kernel.InvokePromptAsync(prompt);

            Console.WriteLine(result);
        }
    }
}