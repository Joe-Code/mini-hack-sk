
using Microsoft.SemanticKernel;

namespace MSLearn_SK_M03
{
    public class MusicLibrarySample
    {
        public static async Task Execute(IKernelBuilder kernelBuilder)
        {
            Console.WriteLine("Your Music Library is ready to help you!\n");

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
    }
}