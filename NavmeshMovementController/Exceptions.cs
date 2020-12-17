using AOSharp.Common.GameData;
using AOSharp.Core;
using System;

namespace NavmeshMovementController
{
    public class PointNotOnNavMeshException : Exception
    {
        public PointNotOnNavMeshException(Vector3 pos) : base ($"Unable to find NavMeshPoint for ({pos})")
        {
        }
    }
}
