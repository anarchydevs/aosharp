using AOSharp.Core;
using AOSharp.Core.Movement;
using AOSharp.Common.GameData;
using System.IO;
using System.Collections.Generic;

namespace AOSharp.Pathfinding
{
    public class NavMeshMovementController : MovementController
    {
        private const float PATH_UPDATE_INTERVAL = 2f;

        private Pathfinder _pathfinder = null;
        private readonly string _navMeshFolderPath;
        private float _lastPathUpdate = 0f;
        private Vector3 _goalPos = Vector3.Zero;
        private bool _usingNavmesh = true;

        public NavMeshMovementController(string navMeshFolderPath, bool drawPath = false) : base(drawPath)
        {
            _navMeshFolderPath = navMeshFolderPath;
            LoadPather((uint)Playfield.ModelIdentity.Instance);

            Game.PlayfieldInit += OnPlayfieldInit;
        }

        protected override void Update(object s, float deltaTime)
        {
            if (_usingNavmesh && IsNavigating && _lastPathUpdate > PATH_UPDATE_INTERVAL)
            {
                List<Vector3> path = _pathfinder.GeneratePath(DynelManager.LocalPlayer.Position, _goalPos);
                RunPath(path);
                _lastPathUpdate = 0;
            }

            _lastPathUpdate += deltaTime;

            base.Update(s, deltaTime);
        }

        public void MoveTo(Vector3 pos, bool useNavmesh = true)
        {
            if (useNavmesh)
            {
                if (_pathfinder == null)
                    return;

                List<Vector3> path = _pathfinder.GeneratePath(DynelManager.LocalPlayer.Position, pos);
                _goalPos = pos;
                _usingNavmesh = useNavmesh;
                RunPath(path);
            }
            else
            {
                base.MoveTo(pos);
            }
        }

        private void OnPlayfieldInit(object s, uint id)
        {
            LoadPather(id);
        }

        private void LoadPather(uint id)
        {
            string navFile = $"{_navMeshFolderPath}\\{Playfield.ModelIdentity.Instance}.navmesh";
            if (!File.Exists(navFile))
                throw new FileNotFoundException($"Unable to find navmesh file: {navFile}");

            _pathfinder = Pathfinder.Create(navFile);
        }
    }
}
