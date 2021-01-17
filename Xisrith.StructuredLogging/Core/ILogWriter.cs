using System.Collections.Generic;

namespace Xisrith.StructuredLogging.Core
{
    public interface ILogWriter
    {
        void WriteLogs(IEnumerable<Log> logs);
    }
}
