using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Core;

namespace mini_hack_sk_sample
{
    public class PromptTemplate
    {
        private static Kernel? _kernel;

        public static async Task Execute(IKernelBuilder kernelBuilder)
        {
            Console.WriteLine("Your Prompt Template is ready to help you!\n");

            _kernel = kernelBuilder.Build();

            string history = @"In the heart of my bustling kitchen, I have embraced 
            the challenge of satisfying my family's diverse taste buds and 
            navigating their unique tastes. With a mix of picky eaters and 
            allergies, my culinary journey revolves around exploring a plethora 
            of vegetarian recipes.

            One of my kids is a picky eater with an aversion to anything green, 
            while another has a peanut allergy that adds an extra layer of complexity 
            to meal planning. Armed with creativity and a passion for wholesome 
            cooking, I've embarked on a flavorful adventure, discovering plant-based 
            dishes that not only please the picky palates but are also heathy and 
            delicious.";

            string prompt = @"This is some information about the user's background: 
            {{$history}}

            Given this user's background, provide a list of relevant recipes.";

            try
            {
                Console.WriteLine("Invoking InvokePromptAsync...");
                if (_kernel != null)
                {
                    var result = await _kernel.InvokePromptAsync(prompt, new KernelArguments { { "history", history } });
                    Console.WriteLine($"Result: {result}");
                }
                else
                {
                    Console.WriteLine("Kernel is null. Unable to invoke prompt.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public static async Task TravelAssistant(IKernelBuilder kernelBuilder)
        {
            Console.WriteLine("Your Travel Assistant is ready to help you!\n");

            _kernel = kernelBuilder.Build();

            string input = @"I'm planning an anniversary trip with my spouse. We like hiking, mountains, and beaches. Our travel budget is $15000";

            string prompt = @$"
                The following is a conversation with an AI travel assistant. 
                The assistant is helpful, creative, and very friendly.

                <message role=""user"">Can you give me some travel destination suggestions?</message>

                <message role=""assistant"">Of course! Do you have a budget or any specific 
                activities in mind?</message>

                <message role=""user"">${input}</message>";

            var result = await _kernel.InvokePromptAsync(prompt);
            Console.WriteLine(result);
        }

        public static async Task FlightAssistant(IKernelBuilder kernelBuilder)
        {
            Console.WriteLine("Your Flight Assistant is ready to help you!\n");
            
            _kernel = kernelBuilder.Build();

            string input = @"I have a vacation from June 1 to July 22. I want to go to Greece. I live in Chicago.";

            string prompt = @$"
                <message role=""system"">Instructions: Identify the from and to destinations 
                and dates from the user's request</message>

                <message role=""user"">Can you give me a list of flights from Seattle to Tokyo? 
                I want to travel from March 11 to March 18.</message>

                <message role=""assistant"">Seattle|Tokyo|03/11/2025|03/18/2025</message>

                <message role=""user"">${input}</message>";

            var result = await _kernel.InvokePromptAsync(prompt);
            Console.WriteLine(result);
        }
    }
}