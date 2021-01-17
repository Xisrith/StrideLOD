using System;
using System.Collections.Generic;
using System.Text;

namespace Xisrith.StructuredLogging.Core
{
    public interface ILogWriterProcessor
    {
        void ProcessWriters(IEnumerable<ILogWriter> writers);
    }
}
