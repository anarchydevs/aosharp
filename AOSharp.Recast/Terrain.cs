using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using AOSharp.Common.GameData;
using AOSharp.Common.Unmanaged.DbObjects;
using AOSharp.Core;

namespace AOSharp.Recast
{
    public class Terrain
    {
        public static List<Mesh> CreateFromTilemap(RDBTilemap tilemap)
        {
            List<Mesh> chunkMeshes = new List<Mesh>();

            foreach(RDBTilemap.Chunk chunk in tilemap.Chunks)
                chunkMeshes.Add(CreateMesh(chunk, tilemap.TileSize, tilemap.HeightmapScale));
 
            return chunkMeshes;
        }

        private static Mesh CreateMesh(RDBTilemap.Chunk chunk, float tileSize, float heightMapScale)
        {
            var lod = 0;
            var index = 0;
            var triIdx = 0;

            if (chunk.ChunkSize == 16)
                lod = 0;

            var sizeMultiplier = 1;
            var physicalSize = chunk.ChunkSize / sizeMultiplier;

            for (int i = 0; i < lod; i++)
            {
                physicalSize /= 2;
                sizeMultiplier *= 2;
            }

            if (lod != 0)
            {
                physicalSize++;
            }

            var vertices = new Vector3[physicalSize * physicalSize];

            for (var y = 0; y < physicalSize; y++)
            {
                for (var x = 0; x < physicalSize; x++)
                {
                    var heightPosX = (int)x * sizeMultiplier;
                    var heightPosY = (int)y * sizeMultiplier;

                    vertices[index] = new Vector3(
                        (x * sizeMultiplier) * tileSize,
                        (float)chunk.HeightMap[heightPosX, heightPosY] * heightMapScale,
                        (y * sizeMultiplier) * tileSize);

                    index++;
                }
            }

            var indices = new List<int>();
            for (int y = 0; y < physicalSize - 1; y++)
            {
                for (int x = 0; x < physicalSize - 1; x++)
                {
                    indices.Add((y * physicalSize) + x + 1);
                    indices.Add((y * physicalSize) + x);
                    indices.Add(((y + 1) * physicalSize) + x);

                    indices.Add((y * physicalSize) + x + 1);
                    indices.Add(((y + 1) * physicalSize) + x);
                    indices.Add(((y + 1) * physicalSize) + x + 1);

                    triIdx += 6;
                }
            }

            return new Mesh
            {
                Triangles = indices,
                Vertices = vertices.ToList(),
                Position = new Vector3((float)(chunk.X * (chunk.ChunkSize - 1)) * tileSize, 0, (float)(chunk.Y * (chunk.ChunkSize - 1)) * tileSize),
                Rotation = Quaternion.Identity,
                Scale = new Vector3(1, 1, 1)
            };
        }
    }
}