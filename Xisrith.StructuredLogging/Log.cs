using System.ComponentModel.DataAnnotations.Schema;

namespace Xisrith.StructuredLogging
{
    public class Log
    {
        [Column]
        public string Source { get; set; }

        [Column]
        public string Label { get; set; }
    }
}
