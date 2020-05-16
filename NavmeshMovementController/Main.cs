using AOSharp.Core;
using AOSharp.Core.Movement;
using AOSharp.Common.GameData;
using System.IO;
using System.Collections.Generic;

namespace AOSharp.Pathfinding
{
    public class NavMeshMovementController : MovementController
    {
        private Pathfinder _pathfinder = null;
        private readonly string _navMeshFolderPath;

        public NavMeshMovementController(string navMeshFolderPath, bool drawPath = false) : base(drawPath)
        {
            _navMeshFolderPath = navMeshFolderPath;
            LoadPather((uint)Playfield.Identity.Instance);

            Game.PlayfieldInit += OnPlayfieldInit;
        }

        public override void MoveTo(Vector3 pos)
        {
            if (_pathfinder == null)
                return;

            List<Vector3> path = _pathfinder.GeneratePath(DynelManager.LocalPlayer.Position, pos);

            RunPath(path);
        }

        private void OnPlayfieldInit(object s, uint id)
        {
            LoadPather(id);
        }

        private void LoadPather(uint id)
        {
            string navFile = $"{_navMeshFolderPath}\\{id}.navmesh";
            if (!File.Exists(navFile))
                throw new FileNotFoundException($"Unable to find navmesh file: {navFile}");

            _pathfinder = Pathfinder.Create(navFile);
        }
    }
}
