using System;
using System.Reflection;
using System.Xml;
using log4net;
using log4net.Core;
using Microsoft.Extensions.Logging;

namespace WebStore.Logger
{
    public class Log4NetLogger : Microsoft.Extensions.Logging.ILogger
    {
        private readonly ILog _Log;
        public Log4NetLogger(string category_name, XmlElement configuration)
        {
            //Получение репозитория с типом Hierarchy
            var logger_repository = LoggerManager.CreateRepository(
                Assembly.GetEntryAssembly(),
                typeof(log4net.Repository.Hierarchy.Hierarchy)
                );
            //Объект для ведения журнала (Для заданной категории)
            _Log = LogManager.GetLogger(logger_repository.Name, category_name);
            //Добавим конфигурацию
            log4net.Config.XmlConfigurator.Configure(logger_repository, configuration);


        }
        public IDisposable BeginScope<TState>(TState state) => null;
        /// <summary>
        /// Активность логгера для разных уровней
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return _Log.IsDebugEnabled;

                case LogLevel.Information:
                    return _Log.IsInfoEnabled;

                case LogLevel.Warning:
                    return _Log.IsWarnEnabled;

                case LogLevel.Error:
                    return _Log.IsErrorEnabled;

                case LogLevel.Critical:
                    return _Log.IsFatalEnabled;

                case LogLevel.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) { return; }
            if (formatter is null) { throw new ArgumentNullException(nameof(formatter)); }

            var msg = formatter(state, exception);

            if(String.IsNullOrEmpty(msg) && exception is null) { return; }

            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    _Log.Debug(msg);
                    break;

                case LogLevel.Information:
                    _Log.Info(msg);
                    break;

                case LogLevel.Warning:
                    _Log.Warn(msg);
                    break;

                case LogLevel.Error:
                    _Log.Error(msg ?? exception.ToString());
                    break;

                case LogLevel.Critical:
                    _Log.Fatal(msg ?? exception.ToString());
                    break;

                case LogLevel.None:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
        }
    }
}