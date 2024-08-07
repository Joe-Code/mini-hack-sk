#pragma warning disable SKEXP0050
using System.Text;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Plugins.Core;

namespace AITravelAgent.Agents
{
    public class CurrencyAgent
    {
        public static async Task ConvertCurrency(IKernelBuilder kernelBuilder)
        {
            Console.WriteLine("Converting currency for you!\n");
            Console.WriteLine("The current working directory is: " + Directory.GetCurrentDirectory() + "\n");

            // Implement this code so the plugin will run in the correct directory while debugging and in the correct directory while in release mode.
#if DEBUG
            string workingDirectory = @"C:\repos\mini-hack-sk\aitravelagent\prompts";
#elif RELEASE
            string workingDirectory = @"Prompts";
#endif

            Console.WriteLine("The working directory is: " + workingDirectory + "\n");

            Kernel kernel = kernelBuilder.Build();
            kernel.ImportPluginFromType<CurrencyConverter>();
            kernel.ImportPluginFromType<ConversationSummaryPlugin>();

            var prompts = kernel.ImportPluginFromPromptDirectory(workingDirectory);

            StringBuilder chatHistory = new();

            // Set the ToolCallBehavior property
            OpenAIPromptExecutionSettings settings = new() { ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions };

            string? input;

            do
            {
                Console.WriteLine("What would you like to do?");
                input = Console.ReadLine();

                var intent = await kernel.InvokeAsync<string>(prompts["GetIntent"], new() { { "input", input } });

                FunctionResult? result = null;

                switch (intent)
                {
                    case "ConvertCurrency":
                        var currencyText = await kernel.InvokeAsync<string>(
                            prompts["GetTargetCurrencies"],
                            new() { { "input", input } }
                        );
                        var currencyInfo = currencyText!.Split("|");
                        result = await kernel.InvokeAsync("CurrencyConverter",
                            "ConvertAmount",
                            new() {
                            {"targetCurrencyCode", currencyInfo[0]},
                            {"baseCurrencyCode", currencyInfo[1]},
                            {"amount", currencyInfo[2]},
                            }
                        );
                        Console.WriteLine(result);
                        break;
                    case "SuggestDestinations":
                        chatHistory.AppendLine("User:" + input);
                        var recommendations = await kernel.InvokePromptAsync(input!);
                        Console.WriteLine(recommendations);
                        break;
                    case "SuggestActivities":
                        var chatSummary = await kernel.InvokeAsync("ConversationSummaryPlugin", "SummarizeConversation", new() { { "input", chatHistory.ToString() } });
                        if (input != null)
                        {
                            var activities = await kernel.InvokePromptAsync(
                                input,
                                new() {
                                {"input", input},
                                {"history", chatSummary},
                                {"ToolCallBehavior", ToolCallBehavior.AutoInvokeKernelFunctions}
                            });

                            chatHistory.AppendLine("User:" + input);
                            chatHistory.AppendLine("Assistant:" + activities.ToString());

                            Console.WriteLine(activities);
                        }
                        break;
                    case "HelpfulPhrases":
                    case "Translate":
                        var autoInvokeResult = await kernel.InvokePromptAsync(input!, new(settings));
                        Console.WriteLine(autoInvokeResult);
                        break;
                    default:
                        Console.WriteLine("Sure, I can help with that.");
                        var otherIntentResult = await kernel.InvokePromptAsync(input!, new(settings));
                        Console.WriteLine(otherIntentResult);
                        break;
                }
            }
            while (!string.IsNullOrWhiteSpace(input));
        }
    }
}