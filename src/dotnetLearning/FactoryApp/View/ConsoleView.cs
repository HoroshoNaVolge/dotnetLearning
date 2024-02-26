namespace dotnetLearning.FactoryApp.View
{
    public class ConsoleView : IView
    {
        public event EventHandler<UserInputEventArgs> InputReceived;
        public string? GetUserInput(string prompt)
        {
            Console.WriteLine(prompt);

            string userInput = Console.ReadLine();

            var userInputEventArgs = new UserInputEventArgs
            {
                UserInput = userInput,
                InputTime = DateTime.Now
            };

            // Вызываем событие и передаем информацию о вводе
            OnUserInputReceived(userInputEventArgs);

            return userInput;
        }

        protected virtual void OnUserInputReceived(UserInputEventArgs e)
        {
            InputReceived?.Invoke(this, e);
        }

        public void ShowMessage(string? message) => Console.WriteLine(message);
    }

}