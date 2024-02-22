namespace dotnetLearning.FactoryApp.View
{
    public class ConsoleView : IView
    {
        public string? GetUserInput() { return Console.ReadLine(); }

        public void ShowMessage(string? message) => Console.WriteLine(message);
    }
}