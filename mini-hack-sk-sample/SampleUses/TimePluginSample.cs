using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Core;

namespace mini_hack_sk_sample
{
    public class TimePluginSample
    {
        public static async Task Execute(IKernelBuilder kernelBuilder)
        {
            Console.WriteLine("Your Time Plugin Sample is ready to help you!\n");

        #pragma warning disable SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            kernelBuilder.Plugins.AddFromType<TimePlugin>();
        #pragma warning restore SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

            var kernel = kernelBuilder.Build();

            var currentDay = await kernel.InvokeAsync("TimePlugin", "DayOfWeek");
            Console.WriteLine("Enjoy your: " + currentDay + "\n\n");

        }
    }
}