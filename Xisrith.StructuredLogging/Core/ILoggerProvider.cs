using System;
using System.Collections.Generic;
using System.Text;

namespace Xisrith.StructuredLogging.Core
{
    public interface ILoggerProvider
    {
        ILogger<S> GetLogger<S>();
    }
}
