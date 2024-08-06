#pragma warning disable SKEXP0050
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
#else
            string workingDirectory = @"Prompts";
#endif

            Console.WriteLine("The working directory is: " + workingDirectory + "\n");

            Kernel kernel = kernelBuilder.Build();
            kernel.ImportPluginFromType<CurrencyConverter>();
            // kernel.ImportPluginFromType<ConversationSummaryPlugin>();
            var prompts = kernel.ImportPluginFromPromptDirectory(workingDirectory);

            // Set the ToolCallBehavior property
            OpenAIPromptExecutionSettings settings = new()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            // var result = await kernel.InvokeAsync("CurrencyConverter",
            //     "ConvertAmount",
            //     new() {
            //         {"targetCurrencyCode", "USD"},
            //         {"amount", "52000"},
            //         {"baseCurrencyCode", "VND"}
            //     }
            // );

            // var result = await kernel.InvokeAsync(prompts["GetTargetCurrencies"], new() {{"input", "How many Australian Dollars is 140,000 Korean Won worth?"}});

            Console.WriteLine("What would you like to do?");
            var input = Console.ReadLine();

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
                case "SuggestActivities":
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
    }
}