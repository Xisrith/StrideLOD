using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xisrith.StructuredLogging.Core
{
    public interface ILogQueueWriterProcessor
    {
        void Process(IEnumerable<ILogQueueWriter> writers);
        Task ProcessAsync(IEnumerable<ILogQueueWriter> writers);
    }
}
