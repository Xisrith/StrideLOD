using System.ComponentModel.DataAnnotations.Schema;
using Xisrith.StructuredLogging;

namespace FirstPersonShooter.Profiling.Models
{
    [Table("Readings")]
    public class Reading : Log
    {
        [Column] public int FPS { get; set; }
    }
}
