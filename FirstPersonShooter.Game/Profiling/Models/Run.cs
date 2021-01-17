using System.ComponentModel.DataAnnotations.Schema;
using Xisrith.StructuredLogging;

namespace FirstPersonShooter.Profiling.Models
{
    [Table("Runs")]
    public class Run : Log
    {
        [Column] public string Case { get; set; }
        [Column] public int Height { get; set; }
        [Column] public int Length { get; set; }
        [Column] public int Width { get; set; }
    }
}
