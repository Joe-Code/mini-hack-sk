using MSLearn_SK_M05;
using SharedKernel;

Console.WriteLine("Hello, Semantic Kernel World!\n");

await MusicHelper.SuggestConcert(KernelBuilder.CreateKernelBuilder());