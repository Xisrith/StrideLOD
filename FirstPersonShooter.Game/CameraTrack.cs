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
using StrideToolkit.Engine;

namespace FirstPersonShooter
{
    public class CameraTrack : AsyncScript
    {
        private readonly double _distanceCutoff = 0.1;

        private bool _isActive = false;

        public TransformComponent CameraFocalPoint { get; set; }

        public List<TransformComponent> CameraTrackPoints { get; set; } = new List<TransformComponent>();

        public double Speed { get; set; }
        
        public override async Task Execute()
        {
            var last = 0;
            var next = 0;
            Func<int> getNext = () => last < CameraTrackPoints.Count - 1 ? last + 1 : 0;

            var currentDistance = 0d;
            var totalDistance = 0d;

            while (Game.IsRunning)
            {
                if(CameraTrackPoints.Count == 1)
                {
                    Entity.Transform.SetWorldPosition(CameraTrackPoints[0].GetWorldPosition());
                    last = 0;
                    currentDistance = 0;
                    totalDistance = 0;
                }
                else if(CameraTrackPoints.Count > 1)
                {
                    if (last == next)
                        next = getNext();

                    var lastPoint = CameraTrackPoints[last];
                    var nextPoint = CameraTrackPoints[next];

                    var delta = Speed * Game.UpdateTime.Elapsed.TotalSeconds;
                    currentDistance += delta;

                    var ratio = (float)(currentDistance / totalDistance);
                    var position = Vector3.Lerp(lastPoint.GetWorldPosition(), nextPoint.GetWorldPosition(), ratio);

                    Entity.Transform.SetWorldPosition(position);
                    Entity.Transform.LookAt(CameraFocalPoint);

                    if (totalDistance - currentDistance < _distanceCutoff)
                    {
                        last = next;
                        next = getNext();
                        currentDistance = 0;
                        totalDistance = Vector3.Distance(CameraTrackPoints[last].GetWorldPosition(), CameraTrackPoints[next].GetWorldPosition());
                    }
                }

                await Script.NextFrame();
            }
        }
    }
}
