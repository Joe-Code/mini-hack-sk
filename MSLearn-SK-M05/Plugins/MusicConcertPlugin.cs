using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace MSLearn_SK_M05
{
    public class MusicConcertPlugin
    {
        [KernelFunction, Description("Get a list of upcoming concerts")]
        public static string GetTours()
        {
            string dir = Directory.GetCurrentDirectory();
            string content = File.ReadAllText($"{dir}/data/concertdates.txt");
            return content;
        }
    }
}