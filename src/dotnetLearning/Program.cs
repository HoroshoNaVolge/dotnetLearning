using dotnetLearning.FactoryApp;
using dotnetLearning.JsonParserApp;
using dotnetLearning.DadataAppConsole;
using dotnetLearning.Other;

internal class Program
{
    static async Task Main(string[] args)
    {
        // Lesson1.Run();
        // Lesson2.Run();
        // JsonLearn.Run();
        // LinqLearn.Run();
        // await LearnWebApi.Run();
        StartJsonParserApp.Run();
        await StartDadataAppConsole.RunAsyncUsingDadataPackage("7728437776");

        //foreach (var i in new IEnumerableLearn()) { await Console.Out.WriteLineAsync(i.ToString()); }
        //foreach (var i in new IEnumerableLearnWithOwnEnumerator()) { await Console.Out.WriteLineAsync(i.ToString()); }
    }
}



