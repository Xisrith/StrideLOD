using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xisrith.StructuredLogging.Core
{
    public interface ILogSaver
    {
        void SaveLogs();
        Task SaveLogsAsync();
    }
}
