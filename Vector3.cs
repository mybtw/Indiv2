using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing
{
    public class Vector3
    {
        private float[] e = new float[3];


        public float this[int i]
        {
            get { return e[i]; }
            set { e[i] = value; }
        }
        public float X => e[0];
        public float Y => e[1];
        public float Z => e[2];

        public Vector3(float e0, float e1, float e2)
        {
            e[0] = e0;
            e[1] = e1;
            e[2] = e2;
        }
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        public double LengthSquared()
        {
            return e[0] * e[0] + e[1] * e[1] + e[2] * e[2];
        }

        public Vector3 Clone()
        {
            return new Vector3(X, Y, Z);
        }
        public float Length()
        {
            return (float)Math.Sqrt(LengthSquared());
        }

        public static Vector3 UnitVector(Vector3 v)
        {
            return v / v.Length();
        }


        public static Vector3 operator *(float t, Vector3 v)
        {
            return new Vector3(t * v.X, t * v.Y, t * v.Z);
        }

        public Vector3 Normalized()
        {
            float length = (float)Math.Sqrt(X * X + Y * Y + Z * Z);
            return new Vector3(X / length, Y / length, Z / length);
        }

        public static Vector3 Cross(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X);
        }

        public static Vector3 norm(Edge S)
        {
            if (S.points.Count() < 3)
                return new Vector3(0, 0, 0);
            Vector3 U = S.points[1] - S.points[0];
            Vector3 V = S.points[S.points.Count - 1] - S.points[0];
            Vector3 normal = U * V;
            return UnitVector(normal);
        }

        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v.X, -v.Y, -v.Z);
        }

        public static Vector3 operator -(Vector3 u, Vector3 v)
        {
            return new Vector3(u.X - v.X, u.Y - v.Y, u.Z - v.Z);
        }

        public static Vector3 operator *(Vector3 u, Vector3 v)
        {
            return new Vector3(u.X * v.X, u.Y * v.Y, u.Z * v.Z);
        }

        public static Vector3 operator *(Vector3 v, float t)
        {
            return new Vector3(v.X * t, v.Y * t, v.Z * t);
        }

  

        public static Vector3 operator /(Vector3 v, float t)
        {
            return new Vector3(v.X / t, v.Y / t, v.Z / t);
        }

        public static float Dot(Vector3 u, Vector3 v)
        {
            return u.X * v.X + u.Y * v.Y + u.Z * v.Z;
        }

    }
}
