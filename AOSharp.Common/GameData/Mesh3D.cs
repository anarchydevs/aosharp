namespace AOSharp.Common.GameData
{
    public class Mesh3D
    {
        public Vector3[] Vertices;
        public int[] Triangles;

        public Mesh3D(Vector3[] vertices, int[] triangles)
        {
            Vertices = vertices;
            Triangles = triangles;
        }
    }
}
