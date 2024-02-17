using Dadata;
using Dadata.Model;
using System.Net.Sockets;
using dotnetLearning.Other;
using System.IO;
using System.Net.Http;
using System.Net.Mail;

namespace dotnetLearning.Lesson3
{
    // Задание: реализовать консольное приложение, которое единственным параметром будет принимать ИНН организации,
    // и печатать название организации, которое оно получит от сервиса DaData.
    // То есть нужно разобраться в их API, зарегистрироваться (сервис бесплатный для небольших масштабов),
    // составить запрос и реализовать это на C# Рекомендуется установить и использовать Fiddler/Postman или другой аналогичный инструмент.
    internal static class Lesson3
    {

        /// <summary>
        /// Вызов через пакет Dadata
        /// </summary>
        /// <param name="companyInn">ИНН компании</param>
        /// <returns></returns>
        internal static async Task RunAsyncUsingDadataPackage(string companyInn)
        {
            {
                var token = "bbc8e57adbc7bebaf5265017740c30fce634cdc0";
                var api = new SuggestClientAsync(token);
                var response = await api.FindParty(companyInn);

                Console.WriteLine(response.suggestions[0].value);
            }
        }
    }
}
