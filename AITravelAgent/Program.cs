using AITravelAgent.Agents;
using SharedKernel;

Console.WriteLine("Hello, Semantic Kernel World!\n");

await CurrencyAgent.ConvertCurrency(KernelBuilder.CreateKernelBuilder());