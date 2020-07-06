using AOSharp.Common.GameData;
using AOSharp.Core;
using System;

namespace NavmeshMovementController
{
    public class OriginNotOnNavMeshException : Exception
    {
        public OriginNotOnNavMeshException(Vector3 origin) : base ($"Unable to find NavMeshPoint for origin ({origin})")
        {
        }
    }

    public class DestinationNotOnNavMeshException : Exception
    {
        public DestinationNotOnNavMeshException(Vector3 dest) : base($"Unable to find NavMeshPoint for destination ({dest})")
        {
        }
    }
}
