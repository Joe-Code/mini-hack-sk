using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.SemanticKernel;

public static class KernelBuilder
{
    public static IKernelBuilder CreateKernelBuilder()
    {
        // Set the Azure OpenAI endpoint
        var azureOpenAIEndpoint = Environment.GetEnvironmentVariable("AZUREOPENAI_ENDPOINT");
        if (string.IsNullOrEmpty(azureOpenAIEndpoint))
        {
            throw new InvalidOperationException("Environment variable AZUREOPENAI_ENDPOINT is not set.");
        }
        Console.WriteLine("Azure OpenAI endpoint was set in the KernelBuilder.cs file: " + azureOpenAIEndpoint);

        // Set the model deployment name
        var modelDeploymentName = Environment.GetEnvironmentVariable("AZUREOPENAI_MODEL_DEPLOYMENT_NAME");
        if (string.IsNullOrEmpty(modelDeploymentName))
        {
            throw new InvalidOperationException("Environment variable AZUREOPENAI_MODEL_DEPLOYMENT_NAME is not set.");
        }
        Console.WriteLine("Azure OpenAI model deployment name was set in the KernelBuilder.cs file: " + modelDeploymentName + "\n\n");

        // Use DefaultAzureCredential to get the token
        var openAIClient = new OpenAIClient(new Uri(azureOpenAIEndpoint), new DefaultAzureCredential());

        // Create Kernel Builder
        var kernelBuilder = Kernel.CreateBuilder();
        kernelBuilder.AddAzureOpenAIChatCompletion(deploymentName: modelDeploymentName, openAIClient: openAIClient);

        return kernelBuilder;
    }
}