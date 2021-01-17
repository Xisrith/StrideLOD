using System;
using System.Collections.Generic;
using System.Text;
using Stride.Engine;

namespace FirstPersonShooter.Utility
{
    public class FpsCounter : SyncScript
    {
        private int _lastTickCount = Environment.TickCount;

        public override void Update()
        {
            DebugText.Enabled = true;
        }
    }
}
