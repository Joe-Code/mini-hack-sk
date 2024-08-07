#pragma warning disable SKEXP0050
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Plugins.Core;

namespace AITravelAgent.Agents
{
    public class TravelAgent
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

            // add logging
            kernelBuilder.Services.AddLogging(configure => configure.AddConsole());
            kernelBuilder.Services.AddLogging(configure => configure.SetMinimumLevel(LogLevel.Information));

            Kernel kernel = kernelBuilder.Build();
            kernel.ImportPluginFromType<CurrencyConverter>();
            kernel.ImportPluginFromType<ConversationSummaryPlugin>();

            var prompts = kernel.ImportPluginFromPromptDirectory(workingDirectory);

            StringBuilder chatHistory = new();

            // Set the ToolCallBehavior property
            OpenAIPromptExecutionSettings settings = new() { ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions };

            string? input;
            Console.WriteLine("JT's Travel Agency is here to help with all your travel questions. What can I help you with?\n");
            input = Console.ReadLine();

            while (!string.IsNullOrWhiteSpace(input))
            {
                var intent = await kernel.InvokeAsync<string>(prompts["GetIntent"], new() { { "input", input } });

                FunctionResult? result = null;

                switch (intent)
                {
                    case "ConvertCurrency":
                        Console.WriteLine("Converting currency for you!\n");
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
                        Console.WriteLine(result + "\n");
                        break;
                    case "SuggestDestinations":
                        Console.WriteLine("Suggesting Destinations:\n");
                        chatHistory.AppendLine("User:" + input);
                        var recommendations = await kernel.InvokePromptAsync(input!);
                        Console.WriteLine(recommendations + "\n");
                        break;
                    case "SuggestActivities":
                        Console.WriteLine("Suggesting Activities:\n");
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

                            Console.WriteLine(activities + "\n");
                        }
                        break;
                    case "HelpfulPhrases":
                    case "Translate":
                        Console.WriteLine("Sure, I will try to translate that for you.\n");
                        var autoInvokeResult = await kernel.InvokePromptAsync(input!, new(settings));
                        Console.WriteLine(autoInvokeResult + "\n");
                        break;
                    default:
                        Console.WriteLine("Sure, I can help with that default request.\n");
                        var otherIntentResult = await kernel.InvokePromptAsync(input!, new(settings));
                        Console.WriteLine(otherIntentResult + "\n");
                        break;
                }

                Console.WriteLine("Is there anything else I can help you with?\n");
                input = Console.ReadLine();
            }
        }
    }
}