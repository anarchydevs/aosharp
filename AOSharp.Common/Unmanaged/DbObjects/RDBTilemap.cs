using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AOSharp.Common.GameData;
using AOSharp.Common.Unmanaged.DataTypes;
using AOSharp.Common.Unmanaged.Interfaces;

namespace AOSharp.Common.Unmanaged.DbObjects
{
    public class RDBTilemap : DbObject
    {
        public unsafe bool IsDungeon => ((RDBTilemapMemStruct*)Pointer)->IsDungeon;
        public unsafe float HeightmapScale => ((RDBTilemapMemStruct*)Pointer)->HeightmapScale;
        public unsafe short NumMainTiles => ((RDBTilemapMemStruct*)Pointer)->NumMainTiles;
        public unsafe IntPtr MainTileIds => ((RDBTilemapMemStruct*)Pointer)->MainTileIds;
        public unsafe int Width => ((RDBTilemapMemStruct*)Pointer)->Width;
        public unsafe int Height => ((RDBTilemapMemStruct*)Pointer)->Height;
        public unsafe int NumChunksX => ((RDBTilemapMemStruct*)Pointer)->GroundData->NumChunksX;
        public unsafe int NumChunksZ => ((RDBTilemapMemStruct*)Pointer)->GroundData->NumChunksZ;
        public unsafe float TileSize => ((RDBTilemapMemStruct*)Pointer)->TileSize;
        public List<Chunk> Chunks;
        public readonly IntPtr pChunkData;

        internal unsafe RDBTilemap(IntPtr pointer) : base(pointer)
        {
            pChunkData = ((RDBTilemapMemStruct*)pointer)->GroundData->pChunkData;
            Parse();
        }

        public static RDBTilemap Get(int id)
        {
            DBIdentity identity = new DBIdentity(DBIdentityType.RDBTilemap, id);
            return ResourceDatabase.GetDbObject<RDBTilemap>(identity);
        }

        public static RDBTilemap FromPointer(IntPtr pointer)
        {
            return new RDBTilemap(pointer);
        }

        private unsafe void Parse()
        {
            AnarchyGroundDataMemStruct groundData = *((RDBTilemapMemStruct*)Pointer)->GroundData;
            Chunks = new List<Chunk>();
            int numChunks = groundData.NumChunksX * groundData.NumChunksZ;

            using (UnmanagedMemoryStream unmanagedMemStream = new UnmanagedMemoryStream((byte*)groundData.pChunkData.ToPointer(), 0x54 * numChunks))
            {
                using (BinaryReader reader = new BinaryReader(unmanagedMemStream))
                {
                    for (int i = 0; i < numChunks; i++)
                    {
                        int x = reader.ReadInt32();
                        int y = reader.ReadInt32();
                        reader.ReadInt32();
                        int chunkSize = reader.ReadInt32(); //32 / 64? ChunkSize??

                        byte[] decompressedHeightMap = DecompressArray(reader.ReadInt32(), (IntPtr)reader.ReadInt32());
                        byte[,] heightMap = UnfilterHeightMap(decompressedHeightMap);

                        reader.ReadBytes(0x3C); // shortHeightMap etc

                        Chunks.Add(new Chunk
                        {
                            X = x,
                            Y = y,
                            ChunkSize = (int)Math.Sqrt(decompressedHeightMap.Length),
                            HeightMap = heightMap
                        });
                    }
                }
            }
        }

        private unsafe byte[] DecompressArray(int size, IntPtr pArray)
        {
            using (UnmanagedMemoryStream unmanagedMemStream = new UnmanagedMemoryStream((byte*)pArray.ToPointer(), size))
            {
                using (DeflateStream inflateStream = new DeflateStream(unmanagedMemStream, CompressionMode.Decompress))
                {
                    using (MemoryStream output = new MemoryStream())
                    {
                        inflateStream.BaseStream.ReadByte();
                        inflateStream.BaseStream.ReadByte();
                        inflateStream.CopyTo(output);
                        return output.ToArray();
                    }
                }
            }
        }

        private byte[,] UnfilterHeightMap(byte[] decompressedHeightMap)
        {
            var chunkSizeX = (int)Math.Sqrt(decompressedHeightMap.Length);
            var chunkSizeZ = chunkSizeX;

            BinaryReader reader = new BinaryReader(new MemoryStream(decompressedHeightMap));

            byte[] bytes = reader.ReadBytes(chunkSizeX * chunkSizeZ);

            int delta;

            for (var y = 0; y < chunkSizeZ; y++)
            {
                delta = 0;
                for (var x = 0; x < chunkSizeX; x++)
                {
                    var val = bytes[chunkSizeX * y + x];
                    bytes[chunkSizeX * y + x] = (byte)(delta + val);
                    delta = delta + val;
                }
            }

            for (var x = 0; x < chunkSizeX; x++)
            {
                delta = 0;
                for (var y = 0; y < chunkSizeZ; y++)
                {
                    var val = bytes[chunkSizeX * y + x];
                    bytes[chunkSizeX * y + x] = (byte)(delta + val);
                    delta = delta + val;
                }
            }

            byte[,] heightMap = new byte[chunkSizeX, chunkSizeZ];
            for (int x = 0; x < chunkSizeX; x++)
                for (int y = 0; y < chunkSizeZ; y++)
                    heightMap[x, y] = bytes[chunkSizeX * y + x];

            return heightMap;
        }

        public class Chunk
        {
            public int X;
            public int Y;
            public int ChunkSize;
            public byte[,] HeightMap;
        }

        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private struct RDBTilemapMemStruct
        {
            [FieldOffset(0x18)]
            public bool IsDungeon;

            [FieldOffset(0x1C)]
            public float HeightmapScale;

            [FieldOffset(0x230)]
            public short NumMainTiles;

            [FieldOffset(0x234)]
            public IntPtr MainTileIds;

            [FieldOffset(0x8258)]
            public unsafe AnarchyGroundDataMemStruct* GroundData;

            [FieldOffset(0x825C)]
            public int Width;

            [FieldOffset(0x8260)]
            public int Height;

            [FieldOffset(0x8264)]
            public float TileSize;
        }

        [StructLayout(LayoutKind.Explicit, Pack = 0)]
        private struct AnarchyGroundDataMemStruct
        {
            [FieldOffset(0x34)]
            public int Width;


            [FieldOffset(0x34)]
            public int Height;

            [FieldOffset(0x34)]
            public int Modulo;

            [FieldOffset(0x50)]
            public int NumChunksX;

            [FieldOffset(0x54)]
            public int NumChunksZ;

            [FieldOffset(0x58)]
            public IntPtr pChunkData;
        }
    }
}
