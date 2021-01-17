using System;
using System.Collections.Generic;
using System.Text;

namespace Xisrith.StructuredLogging
{
    public interface ILogger
    {
        void Log<T>(T log) where T : Log;
    }

    public interface ILogger<S> : ILogger { }
}
