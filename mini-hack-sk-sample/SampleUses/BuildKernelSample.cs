using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Core;
using Microsoft.Extensions.Azure;
using Azure.Identity;
using Azure.Core;
using System;
using System.Threading.Tasks;
using Azure.AI.OpenAI;

namespace mini_hack_sk_sample
{
    public class BuildKernelSample
    {
        public static async Task Execute(IKernelBuilder kernelBuilder)
        {
            Console.WriteLine("Your Build Kernel Sample is ready to help you!\n");
            
            var kernel = kernelBuilder.Build();
            var input = "Give me a list of breakfast foods with eggs and cheese";
            
            Console.WriteLine("The input is: " + input + "\n");

            var result = await kernel.InvokePromptAsync(input);
            Console.WriteLine(result + "\n\n");
        }
    }
}
