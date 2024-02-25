namespace dotnetLearning.FactoryApp.View
{
    public interface IView
    {
        public void ShowMessage(string? message);
        public string? GetUserInput();
    }
}