using Microsoft.SemanticKernel;
using mini_hack_sk_sample;

Console.WriteLine("Hello, Semantic Kernel World!\n");

// await BuildKernelSample.Execute(KernelBuilder.CreateKernelBuilder());
// await TimePluginSample.Execute(KernelBuilder.CreateKernelBuilder());
await ConversationSummary.Execute(KernelBuilder.CreateKernelBuilder());
// await PromptTemplate.Execute(KernelBuilder.CreateKernelBuilder());
// await PromptTemplate.TravelAssistant(KernelBuilder.CreateKernelBuilder());
// await PromptTemplate.FlightAssistant(KernelBuilder.CreateKernelBuilder());
// await MusicTutor.Execute(KernelBuilder.CreateKernelBuilder());


