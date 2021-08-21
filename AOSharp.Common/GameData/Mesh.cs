using System.Collections.Generic;

namespace AOSharp.Common.GameData
{
    public class Mesh
    {
        public List<Vector3> Vertices;
        public List<int> Triangles;
        public Quaternion Rotation;
        public Vector3 Position;
        public Vector3 Scale;

        public Quaternion ParentRotation;
        public Vector3 ParentPosition;

        public Matrix4x4 ParentMatrix
        {
            get
            {
                var translation = Matrix4x4.Translate(ParentPosition);
                var rotation = Matrix4x4.Rotate(ParentRotation);
                var scale = Matrix4x4.Scale(Scale);

                var finalMat = rotation * scale * translation;

                return finalMat;
            }
        }

        public Matrix4x4 LocalToWorldMatrix
        {
            get
            {
                var translation = Matrix4x4.Translate(Position);
                var rotation = Matrix4x4.Rotate(Rotation);


                var finalMat = rotation * translation;

                return finalMat * ParentMatrix;
            }
        }
    }
}
