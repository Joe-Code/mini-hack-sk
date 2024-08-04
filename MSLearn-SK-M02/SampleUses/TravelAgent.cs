#pragma warning disable SKEXP0050
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Core;
using Microsoft.SemanticKernel.ChatCompletion;

namespace MSLearn_SK_M02
{
    public class TravelAgent
    {
        public static async Task Execute(IKernelBuilder kernelBuilder)
        {
            Console.WriteLine("Your Travel Agent is ready to help you!\n");

            // kernelBuilder.Plugins.AddFromType<ConversationSummaryPlugin>();
            var kernel = kernelBuilder.Build();

            kernel.ImportPluginFromType<ConversationSummaryPlugin>();
            var prompts = kernel.ImportPluginFromPromptDirectory("Prompts/TravelPlugins");

            try
            {
                ChatHistory history = [];
                string input = @"I'm planning an anniversary trip with my spouse. We like hiking, 
                    mountains, and beaches. Our travel budget is $15000";

                Console.WriteLine("The input is: " + input + "\n");

                var result = await kernel.InvokeAsync<string>(prompts["SuggestDestinations"], new() {{ "input", input }});
                if (string.IsNullOrEmpty(result))
                {
                    throw new InvalidOperationException("Suggest Destinations did not return a result.");
                }

                Console.WriteLine($"Result: {result}" + "\n\n");
                history.AddUserMessage(input);
                history.AddAssistantMessage(result);

                Console.WriteLine("Where would you like to go?");
                input = Console.ReadLine() ?? string.Empty; // Ensure input is not null;

                result = await kernel.InvokeAsync<string>(prompts["SuggestActivities"], new() {{ "history", history },{ "destination", input },});
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}