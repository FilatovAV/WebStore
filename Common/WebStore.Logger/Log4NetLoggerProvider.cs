using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace WebStore.Logger
{
    public class Log4NetLoggerProvider : ILoggerProvider
    {
        private readonly string _Configuration_File;
        private readonly ConcurrentDictionary<string, Log4NetLogger> _Loggers = new ConcurrentDictionary<string, Log4NetLogger>();
        public Log4NetLoggerProvider(string configuration_file)
        {
            _Configuration_File = configuration_file;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return _Loggers.GetOrAdd(categoryName, category =>
            {
                var xml = new XmlDocument();
                xml.Load(_Configuration_File);
                return new Log4NetLogger(category, xml["log4net"]);
            });
        }
        public void Dispose() => _Loggers.Clear();
    }
}
