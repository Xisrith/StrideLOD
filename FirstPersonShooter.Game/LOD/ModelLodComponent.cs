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
    /// <summary>
    /// Original script provided by EmptyKeys in GitHub issue: https://github.com/stride3d/stride/issues/37
    /// </summary>
    public class ModelLodComponent : AsyncScript
    {
        private readonly int updateFrequency = 10;
        private readonly float lod1DistanceSqrd = 10 * 10;
        private readonly float lod2DistanceSqrd = 20 * 20;
        private readonly float lod3DistanceSqrd = 30 * 30;

        private int currentUpdateFrame;

        [DataMemberCustomSerializer]
        [DataMember(10)]
        public Model ModelLod0 { get; set; }

        [DataMemberCustomSerializer]
        [DataMember(20)]
        public Model ModelLod1 { get; set; }

        [DataMemberCustomSerializer]
        [DataMember(30)]
        public Model ModelLod2 { get; set; }

        [DataMemberCustomSerializer]
        [DataMember(40)]
        public Model ModelLod3 { get; set; }

        public ModelComponent ModelComponent { get; set; }

        public bool Enabled { get; set; }

        public override async Task Execute()
        {
            Random random = new Random();
            currentUpdateFrame = random.Next(0, 11);
            while (Game.IsRunning)
            {
                await Script.NextFrame();

                if (!Enabled)
                {
                    continue;
                }

                if (updateFrequency > currentUpdateFrame)
                {
                    currentUpdateFrame++;
                    continue;
                }

                currentUpdateFrame = 0;
                float distanceSqrd = Vector3.DistanceSquared(this.Entity.Transform.Position, LodService.FocalPoint.Position);
                
                if (distanceSqrd < lod1DistanceSqrd)
                {
                    ModelComponent.Model = ModelLod0;
                }
                else if (distanceSqrd >= lod1DistanceSqrd && distanceSqrd < lod2DistanceSqrd && ModelLod1 != null)
                {
                    ModelComponent.Model = ModelLod1;
                }
                else if (distanceSqrd >= lod2DistanceSqrd && distanceSqrd < lod3DistanceSqrd && ModelLod2 != null)
                {
                    ModelComponent.Model = ModelLod2;
                }
                else if (distanceSqrd >= lod3DistanceSqrd && ModelLod3 != null)
                {
                    ModelComponent.Model = ModelLod3;
                }
            }
        }
    }
}