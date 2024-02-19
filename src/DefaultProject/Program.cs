using dotnetLearning.Lesson1;
using dotnetLearning.Lesson2;
using dotnetLearning.Lesson3;
using dotnetLearning.Other;

internal class Program
{
    static async Task Main(string[] args)
    {
        // Lesson1.Run();
        // Lesson2.Run();
        // JsonLearn.Run();
        // LinqLearn.Run();
        await LearnWebApi.Run();
        await Lesson3.RunAsyncUsingDadataPackage("7728437776");

        //foreach (var i in new IEnumerableLearn()) { await Console.Out.WriteLineAsync(i.ToString()); }
        //foreach (var i in new IEnumerableLearnWithOwnEnumerator()) { await Console.Out.WriteLineAsync(i.ToString()); }
    }


}



