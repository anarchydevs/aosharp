using AOSharp.Core;
using AOSharp.Core.Movement;
using AOSharp.Common.GameData;
using System.IO;
using System.Collections.Generic;
using System;
using Path = AOSharp.Core.Movement.Path;
using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

namespace AOSharp.Pathfinding
{
    public class NavMeshMovementController : MovementController
    {
        private const float PathUpdateInterval = 2f;

        private Pathfinder _pathfinder = null;
        private readonly string _navMeshFolderPath;
        private double _nextPathUpdate = 0;

        public NavMeshMovementController(string navMeshFolderPath, bool drawPath = false) : base(drawPath)
        {
            _navMeshFolderPath = navMeshFolderPath;
            LoadPather((uint)Playfield.ModelIdentity.Instance);

            Game.PlayfieldInit += OnPlayfieldInit;
        }

        public override void Update()
        {
            if (IsNavigating && _paths.Peek() is NavMeshPath path && Time.NormalTime > _nextPathUpdate)
            {
                path.UpdatePath(_pathfinder);
                _nextPathUpdate = Time.NormalTime + PathUpdateInterval;
            }

            base.Update();
        }

        public void SetNavMeshDestination(Vector3 destination)
        {
            SetNavMeshDestination(destination, out _);
        }

        public void AppendNavMeshDestination(Vector3 destination)
        {
            AppendNavMeshDestination(destination, out _);
        }

        public bool SetNavMeshDestination(Vector3 destination, out NavMeshPath path)
        {
            _paths.Clear();
            return AppendNavMeshDestination(destination, out path);
        }

        public bool AppendNavMeshDestination(Vector3 destination, out NavMeshPath path)
        {
            path = new NavMeshPath(destination);

            if (_pathfinder == null)
                return false;

            if (IsNavigating && _paths.Peek() is NavMeshPath navPath && navPath.Destination == destination)
                return false;

            path.UpdatePath(_pathfinder);
            base.AppendPath(path);
            return true;
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

    public class NavMeshPath : Path
    {
        public readonly Vector3 Destination;

        public NavMeshPath(Vector3 dstPos) : base(new List<Vector3>())
        {
            Destination = dstPos;
        }

        internal void UpdatePath(Pathfinder pathfinder)
        {
            List<Vector3> waypoints = pathfinder.GeneratePath(DynelManager.LocalPlayer.Position, Destination);
            base.SetWaypoints(waypoints);
        }
    }
}
