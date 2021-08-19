namespace AOSharp.Common.GameData
{
    public class Mesh
    {
        public Vector3[] Vertices;
        public int[] Triangles;

        public Mesh(Vector3[] vertices, int[] triangles)
        {
            Vertices = vertices;
            Triangles = triangles;
        }
    }
}
