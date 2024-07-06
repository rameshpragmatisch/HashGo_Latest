namespace HashGo.Core.Contracts.Services
{
    public interface ILoggingService : IApplicationService
    {
        void Info(string message);

        void Info( string message, params object[] args);

        void Debug(string message);

        void Debug(string message, params object[] args);

        void Trace(string message);

        void Trace(string message, params object[] args);

        void TraceException(Exception ex, string message = "");
    }

}