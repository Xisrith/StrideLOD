using System;
using System.Collections.Generic;
using System.Text;

namespace Xisrith.StructuredLogging.Core
{
    public interface ILogQueueWriterProvider
    {
        ILogQueueWriter GetLogQueueWriter<T>() where T : Log;
    }
}
