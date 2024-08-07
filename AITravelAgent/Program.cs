using AITravelAgent.Agents;
using SharedKernel;

Console.WriteLine("Hello, Semantic Kernel World!\n");

await TravelAgent.ConvertCurrency(KernelBuilder.CreateKernelBuilder());