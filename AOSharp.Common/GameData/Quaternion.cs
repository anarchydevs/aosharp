using System;
using SmokeLounge.AOtomation.Messaging.Serialization.MappingAttributes;

namespace AOSharp.Common.GameData
{
    public class Quaternion
    {
        #region Variables

        [AoMember(0)]
        public float X { get; set; }

        [AoMember(1)]
        public float Y { get; set; }

        [AoMember(2)]
        public float Z { get; set; }

        [AoMember(3)]
        public float W { get; set; }

        #endregion

        #region Constructor

        public Quaternion()
        {
        }

        public Quaternion(double x, double y, double z, double w)
        {
            this.X = (float)x;
            this.Y = (float)y;
            this.Z = (float)z;
            this.W = (float)w;
        }

        public Quaternion(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public Quaternion(Vector3 v, float angle)
        {
            double sinAngle;
            Vector3 vNormalized;

            vNormalized = v.Normalize();

            sinAngle = Math.Sin(angle / 2);
            X = (float)(vNormalized.X * sinAngle);
            Y = (float)(vNormalized.Y * sinAngle);
            Z = (float)(vNormalized.Z * sinAngle);

            W = (float)Math.Cos(angle / 2);
        }

        public Quaternion(Vector3 v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = 0;
        }
        #endregion

        #region Methods

        public void Update(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public double Yaw
        {
            get
            {
                double _yaw = Math.Atan2((2 * Y * W) - (2 * X * Z), 1 - (2 * Y * Y) - (2 * Z * Z));
                if (_yaw < 0) // So we get a positive number
                    _yaw += 2 * Math.PI;
                return _yaw;
            }
        }

        public double Pitch
        {
            get { return -2 * Math.Atan2((2 * X * W) - (2 * Y * Z), 1 - (2 * X * Y) - (2 * Z * Z)); }
        }

        public double Roll
        {
            get
            {
                return Math.Asin((2 * X * Y) + (2 * Z * W));
            }
        }

        public double Magnitude
        {
            get { return Math.Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W)); }
        }

        public static Quaternion Conjugate(Quaternion q1)
        {
            return new Quaternion(-q1.X, -q1.Y, -q1.Z, q1.W);
        }

        public Quaternion Conjugate()
        {
            return Conjugate(this);
        }

        public static Quaternion Hamilton(Quaternion vLeft, Quaternion vRight)
        {
            double w = (vLeft.W * vRight.W) - (vLeft.X * vRight.X) - (vLeft.Y * vRight.Y) - (vLeft.Z * vRight.Z);
            double x = (vLeft.W * vRight.X) + (vLeft.X * vRight.W) + (vLeft.Y * vRight.Z) - (vLeft.Z * vRight.Y);
            double y = (vLeft.W * vRight.Y) - (vLeft.X * vRight.Z) + (vLeft.Y * vRight.W) + (vLeft.Z * vRight.X);
            double z = (vLeft.W * vRight.Z) + (vLeft.X * vRight.Y) - (vLeft.Y * vRight.X) + (vLeft.Z * vRight.W);

            return new Quaternion(x, y, z, w);
        }

        public void Rotate(float heading, float attitude, float bank)
        {
            // Assuming the angles are in radians.
            double c1 = Math.Cos(heading / 2);
            double s1 = Math.Sin(heading / 2);
            double c2 = Math.Cos(attitude / 2);
            double s2 = Math.Sin(attitude / 2);
            double c3 = Math.Cos(bank / 2);
            double s3 = Math.Sin(bank / 2);
            double c1c2 = c1 * c2;
            double s1s2 = s1 * s2;
            W = (float)(c1c2 * c3 - s1s2 * s3);
            X = (float)(c1c2 * s3 + s1s2 * c3);
            Y = (float)(s1 * c2 * c3 + c1 * s2 * s3);
            Z = (float)(c1 * s2 * c3 - s1 * c2 * s3);
        }

        public static Quaternion CreateFromAxisAngle(double xx, double yy, double zz, double a)
        {
            // Here we calculate the sin( theta / 2) once for optimization
            double result = Math.Sin(a / 2.0);

            // Calculate the x, y and z of the quaternion
            double y = xx * result;
            double x = yy * result;
            double z = zz * result;

            // Calculate the w value by cos( theta / 2 )
            double w = Math.Cos(a / 2.0);

            return new Quaternion(x, y, z, w).Normalize();
        }

        public Quaternion Hamilton(Quaternion vRight)
        {
            return Hamilton(this, vRight);
        }

        public static Quaternion Normalize(Quaternion q1)
        {
            double mag = q1.Magnitude;

            return new Quaternion(q1.X / mag, q1.Y / mag, q1.Z / mag, q1.W / mag);
        }

        public Quaternion Normalize()
        {
            return Normalize(this);
        }

        public static Vector3 RotateVector3(Quaternion q1, Vector3 v2)
        {
            Quaternion QuatVect = new Quaternion(v2.X, v2.Y, v2.Z, 0);
            Quaternion QuatNorm = q1.Normalize();
            Quaternion Result = Hamilton(Hamilton(QuatNorm, QuatVect), QuatNorm.Conjugate());
            return new Vector3(Result.X, Result.Y, Result.Z);
        }

        public Vector3 RotateVector3(Vector3 v1)
        {
            return RotateVector3(this, v1);
        }

        public static Vector3 VectorRepresentation(Quaternion q1)
        {
            return new Vector3(q1.X, q1.Y, q1.Z);
        }

        public Vector3 VectorRepresentation()
        {
            return VectorRepresentation(this);
        }

        #endregion
    }
}
