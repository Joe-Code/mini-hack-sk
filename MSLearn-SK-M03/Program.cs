using MSLearn_SK_M03;
using SharedKernel;

Console.WriteLine("Hello, World!");

// await MusicLibrarySample.AddToRecentlyPlayed(KernelBuilder.CreateKernelBuilder());
await MusicLibrarySample.SuggestSongFromPlaylist(KernelBuilder.CreateKernelBuilder());