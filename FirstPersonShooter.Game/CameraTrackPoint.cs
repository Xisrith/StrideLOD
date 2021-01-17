using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Rendering;


namespace FirstPersonShooter
{
    public class CameraTrackPoint : StartupScript
    {
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }

        public override void Start()
        {
            Position = Entity.Transform.Position;
            Rotation = Entity.Transform.Rotation;
        }
    }
}
