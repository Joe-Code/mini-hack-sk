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

            var plugins = kernel.CreatePluginFromPromptDirectory("Prompts");
            string input = "G, C";

            var result = await kernel.InvokeAsync(plugins["SuggestChords"], new() {{ "startingChords", input }});

            Console.WriteLine(result);
        }
    }
}