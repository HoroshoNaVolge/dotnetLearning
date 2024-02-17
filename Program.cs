using dotnetLearning.Lesson1;
using dotnetLearning.Lesson2;
using dotnetLearning.Lesson3;
using dotnetLearning.Other;

internal class Program
{
    static void Main(string[] args)
    {
        // Lesson1.Run();
        // Lesson2.Run();
        // JsonLearn.Run();
        // LinqLearn.Run();
        LearnWebApi.Run();
        Lesson3.RunAsyncUsingDadataPackage("7728437776").Wait();
    }

   
}



