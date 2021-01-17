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
    /// Based on ModelLodComponent from EmptyKeys, but made to be more generic and configurable.
    /// </summary>
    public class GenericLodComponent : AsyncScript
    {
        private readonly int _updateMinFrequency = 0;   // Inclusive.
        private readonly int _updateMaxFrequency = 11;  // Exclusive.

        private int _framesUntilUpdate;

        public bool Enabled { get; set; }

        [DataMember(20)]
        public ModelComponent ModelComponent { get; set; }

        [DataMember(30)]
        public List<LodModel> LodModels = new List<LodModel>();

        // TODO: Find performance bottleneck.
        // Using the following test case: case:lod_generic  height:3 lenght:10 width:10
        // Whenever this script exits before the while loop, I get around 200fps
        // when the script is allowed to run, I get around 30fps
        // I've tried:
        // - Setting Enabled = false
        // - Disabling Model assignment
        // - Disabling logging
        // None of which had more than a 1 or 2 fps impact.
        // I've also tried these tests using case:lod_model which uses the model LOD
        // script from the Stride GitHub issue, but that had similar results hovering
        // around 30 regardless of what I changed.
        // I'm wondering if the LOD is running so fast that we're getting killed
        // by the overhead of the threading system...
        public override async Task Execute()
        {
            Random random = new Random();
            _framesUntilUpdate = random.Next(_updateMinFrequency, _updateMaxFrequency);

            var lods = LodModels.OrderBy(l => l.Distance);
            while (Game.IsRunning)
            {
                if(CancellationToken.IsCancellationRequested)
                {
                    break;
                }

                if(!Enabled)
                {
                    await Script.NextFrame();
                    continue;
                }

                if(_framesUntilUpdate > 0)
                {
                    _framesUntilUpdate--;
                    await Script.NextFrame();
                    continue;
                }

                var distanceSquared = Vector3.DistanceSquared(Entity.Transform.Position, LodService.FocalPoint.Position);
                var previous = lods.First();
                foreach(var lod in lods)
                {
                    if(distanceSquared < lod.DistanceSquared)
                        break;

                    previous = lod;
                }

                ModelComponent.Model = previous.Model;

                _framesUntilUpdate = random.Next(_updateMinFrequency, _updateMaxFrequency);

                await Script.NextFrame();
            }
        }
    }

    [DataContract]
    public class LodModel
    {
        private float _distanceSquared;

        [DataMember]
        public float Distance { get; set; }

        [DataMember]
        public Model Model { get; set; }

        public float DistanceSquared
        {
            get
            {
                if (_distanceSquared == 0)
                    _distanceSquared = Distance * Distance;
                return _distanceSquared;
            }
        }
    }
}
