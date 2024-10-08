#pragma warning disable SKEXP0050
using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Core;

namespace MSLearn_SK_M02
{
    public class ConversationSummary
    {
        public static async Task Execute(IKernelBuilder kernelBuilder)
        {
            Console.WriteLine("Your Conversation Summary is ready to help you!\n");

            kernelBuilder.Plugins.AddFromType<ConversationSummaryPlugin>();
            var kernel = kernelBuilder.Build();

            string input = @"I'm a vegan in search of new recipes. I love spicy food! Can you give me a list of breakfast recipes that are vegan friendly?";
            Console.WriteLine("The input is: " + input + "\n");

            try
            {
                Console.WriteLine("Invoking ConversationSummaryPlugin...\n");
                
                var result = await kernel.InvokeAsync("ConversationSummaryPlugin", "GetConversationActionItems", new() { { "input", input } });

                Console.WriteLine("Invocation completed.\n");
                Console.WriteLine($"Result: {result}" + "\n\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}