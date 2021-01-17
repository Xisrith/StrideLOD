using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Mathematics;
using Stride.Engine;
using Xisrith.StructuredLogging;
using Xisrith.StructuredLogging.Core;
using FirstPersonShooter.Profiling.Models;

namespace FirstPersonShooter
{
    public class TestFactory : StartupScript
    {
        private const string PARAMETER_NAME_PREFAB = "case";
        private const string PARAMETER_NAME_HEIGHT = "height";
        private const string PARAMETER_NAME_LENGTH = "length";
        private const string PARAMETER_NAME_WIDTH = "width";

        private ILogger<TestFactory> _logger;

        private string _testCase;

        private Prefab _spherePrefab;

        private Scene _rootScene;

        private int _sphereHeight;
        private int _sphereLength;
        private int _sphereWidth;

        private int _floorCountX;
        private int _floorCountZ;
        private int _floorWidth;
        private int _floorLength;

        public static string Label { get; } = Guid.NewGuid().ToString();

        public bool Enabled { get; set; }

        [Display(10)] public CameraTrack CameraTrack { get; set; }
        [Display(11)] public Prefab CameraTrackPointPrefab { get; set; }

        [Display(20)] public Prefab FloorPrefab { get; set; }
        [Display(21)] public int FloorSpacing { get; set; }

        [Display(30)] public Dictionary<string, Prefab> SpherePrefabs { get; set; } = new Dictionary<string, Prefab>();
        [Display(31)] public string DefaultSpherePrefab { get; set; }
        [Display(32)] public int SphereSpacing { get; set; }

        [Display(40)] public int TestHeight { get; set; }
        [Display(41)] public int TestLength { get; set; }
        [Display(42)] public int TestWidth { get; set; }

        public override void Start()
        {
            if (Enabled)
            {
                _logger = Services.GetService<LoggingManager>().GetLogger<TestFactory>();

                ApplyLaunchParameters();

                _rootScene = SceneSystem.SceneInstance.RootScene;

                _sphereHeight = SphereSpacing * TestHeight;
                _sphereLength = SphereSpacing * TestLength;
                _sphereWidth = SphereSpacing * TestWidth;

                _floorCountX = _sphereWidth / FloorSpacing + 1;
                _floorCountZ = _sphereLength / FloorSpacing + 1;
                _floorLength = _floorCountX * FloorSpacing;
                _floorWidth = _floorCountZ * FloorSpacing;

                CreateCameraTrack();
                CreateFloor();
                CreateSpheres();

                _logger.Log(new Run
                {
                    Label = Label,
                    Case = _testCase,
                    Height = TestHeight,
                    Length = TestLength,
                    Width = TestWidth
                });
            }
        }

        private void ApplyLaunchParameters()
        {
            if (Game.LaunchParameters.ContainsKey(PARAMETER_NAME_PREFAB) && SpherePrefabs.ContainsKey(Game.LaunchParameters[PARAMETER_NAME_PREFAB]))
                _testCase = Game.LaunchParameters[PARAMETER_NAME_PREFAB];
            else
                _testCase = DefaultSpherePrefab;

            _spherePrefab = SpherePrefabs[_testCase];

            if (Game.LaunchParameters.ContainsKey(PARAMETER_NAME_HEIGHT))
                TestHeight = int.Parse(Game.LaunchParameters[PARAMETER_NAME_HEIGHT]);

            if (Game.LaunchParameters.ContainsKey(PARAMETER_NAME_LENGTH))
                TestLength = int.Parse(Game.LaunchParameters[PARAMETER_NAME_LENGTH]);

            if (Game.LaunchParameters.ContainsKey(PARAMETER_NAME_WIDTH))
                TestWidth = int.Parse(Game.LaunchParameters[PARAMETER_NAME_WIDTH]);
        }

        private void CreateCameraTrack()
        {
            // Create the camera track points.
            var cameraMinX = 0;
            var cameraMinZ = 0;
            var cameraMaxX = _floorWidth;
            var cameraMaxZ = _floorLength;
            var cameraY = _sphereHeight;

            var trackPoints = new Entity[] {
                CameraTrackPointPrefab.Instantiate().First(),
                CameraTrackPointPrefab.Instantiate().First(),
                CameraTrackPointPrefab.Instantiate().First(),
                CameraTrackPointPrefab.Instantiate().First()
            };

            SceneSystem.SceneInstance.RootScene.Entities.AddRange(trackPoints);

            trackPoints[0].Transform.Position = new Vector3(cameraMinX, cameraY, cameraMinZ);
            trackPoints[1].Transform.Position = new Vector3(cameraMaxX, cameraY, cameraMinZ);
            trackPoints[2].Transform.Position = new Vector3(cameraMaxX, cameraY, cameraMaxZ);
            trackPoints[3].Transform.Position = new Vector3(cameraMinX, cameraY, cameraMaxZ);

            CameraTrack.CameraTrackPoints.AddRange(trackPoints.Select(e => e.Transform));

            // Set the camera focal point.
            var focalPoint = new Vector3(
                (cameraMaxX - cameraMinX) / 2,
                cameraY / 2,
                (cameraMaxZ - cameraMinZ) / 2
            );
            CameraTrack.CameraFocalPoint.Position = focalPoint;
        }

        private void CreateFloor()
        {
            var floorCountX = _sphereWidth / FloorSpacing + 1;
            var floorCountZ = _sphereLength / FloorSpacing + 1;

            for (var x = 0; x < floorCountX; x++)
            {
                for (var z = 0; z < floorCountZ; z++)
                {
                    var floor = FloorPrefab.Instantiate().First();
                    _rootScene.Entities.Add(floor);

                    floor.Transform.Position = new Vector3(
                        floor.Transform.Position.X + x * FloorSpacing,
                        0,
                        floor.Transform.Position.Z + z * FloorSpacing
                    );
                }
            }
        }

        private void CreateSpheres()
        {
            var marginWidth = ((float)(_floorWidth - _sphereWidth) / 2) + ((float)SphereSpacing / 2);
            var marginHeight = (float)SphereSpacing / 2;
            var marginLength = ((float)(_floorLength - _sphereLength) / 2) + ((float)SphereSpacing / 2);
            for (var x = 0; x < TestWidth; x++)
            {
                for (var y = 0; y < TestHeight; y++)
                {
                    for (var z = 0; z < TestLength; z++)
                    {
                        var sphere = _spherePrefab.Instantiate().First();
                        _rootScene.Entities.Add(sphere);

                        sphere.Transform.Position = new Vector3(
                            marginWidth + x * SphereSpacing,
                            marginHeight + y * SphereSpacing,
                            marginLength + z * SphereSpacing
                        );
                    }
                }
            }
        }

        private enum TestCase
        {
            Static20,
            Static80,
            Static320,
            Static1280,
            Static5120
        }
    }
}
