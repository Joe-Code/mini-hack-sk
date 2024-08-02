using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Core;

namespace mini_hack_sk_sample
{
    public class MusicTutor
    {
        public static async Task Execute(IKernelBuilder kernelBuilder)
        {
            Console.WriteLine("Your Music Tutor is ready to help you!\n");
            
            //kernelBuilder.Plugins.AddFromPromptDirectory("Prompts");
            Kernel kernel = kernelBuilder.Build();

            Console .WriteLine("The current working directory is: " + Directory.GetCurrentDirectory() + "\n");
            
// Implement this code so the plugin will run in the correct directory while debugging and in the correct directory while in release mode.
// #if DEBUG
// string workingDirectory = @"C:\path\to\your\debug\directory";
// #else
// string workingDirectory = @"C:\path\to\your\release\directory";
// #endif

            var plugins = kernel.CreatePluginFromPromptDirectory("Prompts");
            string input = "D, A";
            Console.WriteLine("The input is: " + input + "\n");

            var result = await kernel.InvokeAsync(plugins["SuggestChords"], new() {{ "startingChords", input }});

            Console.WriteLine(result + "\n\n");
        }
    }
}