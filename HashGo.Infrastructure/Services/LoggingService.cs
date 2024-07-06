using HashGo.Core.Contracts.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGo.Infrastructure.Services
{
    public class LoggingService : ILoggingService
    {

        private readonly Logger logger;

        public LoggingService()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Debug(string message, params object[] args)
        {
            logger.Debug(message, args);
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Info(string message, params object[] args)
        {
            logger.Info(message, args);
        }

        public void Trace(string message)
        {
            logger.Trace(message);
        }

        public void Trace(string message, params object[] args)
        {
            logger.Trace(message, args);
        }

        public void TraceException(Exception ex, string message = "")
        {
            logger.Trace(ex,message);
        }
    }
}
