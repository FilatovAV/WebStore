using System.IO;
using System.Reflection;

namespace WebStore.Logger
{
    public static class Log4NetExtensions
    {
        public static Microsoft.Extensions.Logging.ILoggerFactory AddLog4Net(
            this Microsoft.Extensions.Logging.ILoggerFactory factory, 
            string configuration_file = "log4net.config")
        {
            if (!Path.IsPathRooted(configuration_file))
            {
                //Получим точку сборку из которой начинается процесс выполнения
                var assembly = Assembly.GetEntryAssembly();
                //Получим рабочую директорию
                var directory_name = Path.GetDirectoryName(assembly.Location);
                //Получим правильный путь к файлу конфигурации
                configuration_file = Path.Combine(directory_name, configuration_file);
            }

            factory.AddProvider(new Log4NetLoggerProvider(configuration_file));

            return factory;
        }
    }
}
