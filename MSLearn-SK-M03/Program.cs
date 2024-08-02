using MSLearn_SK_M03;

Console.WriteLine("Hello, World!");

// await MusicLibrarySample.AddToRecentlyPlayed(KernelBuilder.CreateKernelBuilder());
await MusicLibrarySample.SuggestSongFromPlaylist(KernelBuilder.CreateKernelBuilder());