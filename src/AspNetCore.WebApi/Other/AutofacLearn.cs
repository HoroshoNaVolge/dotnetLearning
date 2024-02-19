using Autofac;

namespace AspNetCore.WebApi.Other
{
    public static class AutofacLearn
    {
        public static void Run()
        {
            var autofacBuilder = new ContainerBuilder();
            autofacBuilder.Register(c => new ConsoleWriter()).As<IWriter>();
            autofacBuilder.RegisterType<Logger>().As<ILogger>();

            var container = autofacBuilder.Build();

            ILogger i = container.Resolve<ILogger>();
            i.Log("Test console message using Autofac");
        }
    }
    public interface IWriter
    {
        public void Write(string value);
    }

    public interface ILogger
    {
        public void Log(string message);
    }

    public class ConsoleWriter : IWriter
    {
        public void Write(string value)
        {
            Console.WriteLine(value);
        }
    }

    public class FileWriter : IWriter
    {
        private string _filename;

        public FileWriter(string filename)
        {
            _filename = filename;
        }

        public void Write(string value)
        {
            File.AppendAllText(_filename, value);
        }
    }

    public class Logger : ILogger
    {
        IWriter _writer;

        public Logger(IWriter writer)
        {
            _writer = writer;
        }

        public void Log(string s)
        {
            _writer?.Write($"[{DateTime.Now}]:{s}");
        }
    }
}
