using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace MSLearn_SK_M05
{
    public class MusicHelper
    {
        public static async Task SuggestConcert(IKernelBuilder kernelBuilder)
        {
            Console.WriteLine("Suggesting a concert for you!\n");

            Kernel kernel = kernelBuilder.Build();
            kernel.ImportPluginFromType<MusicLibraryPlugin>();
            kernel.ImportPluginFromType<MusicConcertPlugin>();
            kernel.ImportPluginFromPromptDirectory("Prompts");

            // Set the ToolCallBehavior property
            OpenAIPromptExecutionSettings settings = new()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            string location = "Tampa FL USA";
            string prompt = @$"Based on the user's recently played music, suggest a concert for the user living in ${location}";

            Console.WriteLine("The prompt used: " + prompt);

            // Use the settings to automatically invoke plugins based on the prompt
            var result = await kernel.InvokePromptAsync(prompt, new(settings));

            Console.WriteLine(result);
        }
    }
}