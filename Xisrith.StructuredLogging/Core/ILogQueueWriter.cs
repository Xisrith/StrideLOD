using System;
using System.Collections.Generic;
using System.Text;

namespace Xisrith.StructuredLogging.Core
{
    public interface ILogQueueWriter
    {
        void EnqueueLog(Log log);
        void WriteLogs();
    }
}
