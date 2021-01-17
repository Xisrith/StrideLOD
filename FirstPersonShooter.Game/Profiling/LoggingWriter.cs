using System;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Stride.Engine;
using Xisrith.StructuredLogging.Core;
using Xisrith.StructuredLogging.Sqlite;
using FirstPersonShooter.Profiling.Models;

namespace FirstPersonShooter.Profiling
{
    public class LoggingWriter : AsyncScript
    {
        public bool Enabled { get; set; }
        public string Filename { get; set; }

        public override async Task Execute()
        {
            var loggingManager = new LoggingManagerBuilder()
                .UseSqlite($"Data Source={Filename}")
                .Build();

            Services.AddService(loggingManager);

            while (Game.IsRunning)
            {
                if (Enabled)
                {
                    await loggingManager.SaveLogsAsync();
                    await Script.NextFrame();
                }
            }
        }
    }
}
