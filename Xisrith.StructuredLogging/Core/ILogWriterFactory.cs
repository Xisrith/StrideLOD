using System;
using System.Collections.Generic;
using System.Text;

namespace Xisrith.StructuredLogging.Core
{
    public interface ILogWriterFactory
    {
        ILogWriter CreateLogWriter<T>() where T : Log;
    }
}
