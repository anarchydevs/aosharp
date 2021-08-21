using AOSharp.Common.GameData;
using AOSharp.Common.Unmanaged.DbObjects;
using AOSharp.Core;
using AOSharp.Core.UI;
using org.critterai.geom;
using org.critterai.nav;
using org.critterai.nmbuild;
using org.critterai.nmgen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using NavVector3 = org.critterai.Vector3;

namespace AOSharp.Recast
{
    public class NavmeshGenerator
    {
        public static async Task<Navmesh> BakeAsync(NMGenParams navmeshGenParams)
        {
            return await Task.Run(() =>
            {
                List<Mesh> terrainChunks = Terrain.CreateFromTilemap(Playfield.RDBTilemap);
                List<SurfaceResource> surfaces = Playfield.Zones.Select(x => SurfaceResource.Get(Playfield.ModelIdentity.Instance << 16 | x.Instance)).ToList();

                TriangleMesh triMesh = CreateTriangleMesh(terrainChunks, surfaces);

                byte[] areas = NMGen.CreateDefaultAreaBuffer(triMesh.triCount);
                InputGeometryBuilder gbuilder = InputGeometryBuilder.Create(triMesh, areas, navmeshGenParams.WalkableSlope);

                if (gbuilder == null)
                    throw new Exception($"Failed to create InputGeometryBuilder");

                gbuilder.BuildAll();
                InputGeometry geom = gbuilder.Result;
                TileSetDefinition tdef = TileSetDefinition.Create(geom.BoundsMin, geom.BoundsMax, navmeshGenParams, geom);

                List<TileBuildAssets> tiles = new List<TileBuildAssets>();
                int maxPolys = 0;

                Navmesh navmesh = null;

                Parallel.For(0, tdef.Width, x => 
                {
                    Parallel.For(0, tdef.Depth, z =>
                    {
                        IncrementalBuilder builder = IncrementalBuilder.Create(x, z, NMGenAssetFlag.PolyMesh | NMGenAssetFlag.DetailMesh, tdef, ProcessorSet.CreateStandard(ProcessorSet.StandardOptions));
                        builder.BuildAll();

                        switch (builder.State)
                        {
                            case NMGenState.Aborted:
                                //Is this fatal??
                                throw new Exception($"MultiTile Creation aborted");
                        }

                        NMGenAssets assets = builder.Result;
                        TileBuildTask task = TileBuildTask.Create(x, z, assets.PolyMesh.GetData(false), assets.DetailMesh.GetData(false), ConnectionSet.CreateEmpty(), false, false, 0);
                        task.Run();
                        tiles.Add(task.Result);
                        maxPolys = Math.Max(maxPolys, task.Result.PolyCount);
                    });
                });

                NavmeshParams nconfig = new NavmeshParams(tdef.BoundsMin, tdef.TileWorldSize,
                                                          tdef.TileWorldSize, tiles.Count, maxPolys);

                var status = Navmesh.Create(nconfig, out navmesh);
                if ((status & NavStatus.Sucess) == 0)
                    throw new Exception($"Navmesh creation was not successful. {status}");

                if (navmesh == null)
                    throw new Exception($"Navmesh is null?");

                // Add the tiles to the navigation mesh.
                foreach (TileBuildAssets tile in tiles)
                    navmesh.AddTile(tile.Tile, Navmesh.NullTile, out _);

                return navmesh;
            });
        }

        private static TriangleMesh CreateTriangleMesh(List<Mesh> terrainChunks, List<SurfaceResource> surfaces)
        {
            List<NavVector3> verts = new List<NavVector3>();
            List<int> indices = new List<int>();

            var numVerts = 0;
            foreach (var mesh in terrainChunks)
            {
                foreach (var i in mesh.Triangles)
                    indices.Add(numVerts + i);

                foreach (var vert in mesh.Vertices)
                {
                    var worldPos = mesh.LocalToWorldMatrix.MultiplyPoint3x4(vert);
                    verts.Add(worldPos.ToCAIVector3());

                    numVerts++;
                }
            }

            foreach (var surface in surfaces)
            {
                if (surface == null)
                    continue;

                foreach(Mesh mesh in surface.Meshes)
                {
                    foreach (var i in mesh.Triangles)
                        indices.Add(numVerts + i);

                    foreach (var vert in mesh.Vertices)
                    {
                        verts.Add(vert.ToCAIVector3());
                        numVerts++;
                    }
                }
            }

            return new TriangleMesh(verts.ToArray(), verts.Count, indices.ToArray(), indices.Count / 3);
        }
    }
}
