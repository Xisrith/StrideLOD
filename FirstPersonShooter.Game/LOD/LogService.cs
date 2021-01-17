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

namespace FirstPersonShooter.LOD
{
    public class LodSettings : StartupScript
    {
        public TransformComponent FocalPoint { get; set; }

        public override void Start()
        {
            LodService.FocalPoint = FocalPoint;
        }
    }

    public static class LodService
    {
        public static TransformComponent FocalPoint { get; set; }
    }
}
