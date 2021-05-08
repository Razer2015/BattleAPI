using System;

namespace Shared.Interfaces {
    public interface ILoggingService {
        void Write(string type, string message);
        void WriteInfo(string message);
        void WriteWarning(string message);
        void WriteError(string message);
        void WriteException(Exception ex);
        void WriteException(string message, Exception ex);
    }
}
