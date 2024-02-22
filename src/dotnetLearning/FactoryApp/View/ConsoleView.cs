namespace dotnetLearning.FactoryApp.View
{
    public class ConsoleView : IView
    {
        public string? GetUserInput()
        {
            System.Console.WriteLine();
            
        }

        public void ShowMessage(string message)
        {
            throw new NotImplementedException();
        }
    }

}