using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RayTracing
{
    public class AABB
    {
        public Vector3 min { get; set; }
        public Vector3 max { get; set; }

        public AABB()
        {
            min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        }

        public bool Intersect(Vector3 origin, Vector3 dir)
        {
             float tMinX = (min.X - origin.X) / dir.X;
             float tMinY = (min.Y - origin.Y) / dir.Y;
             float tMinZ = (min.Z - origin.Z) / dir.Z;

             float tMaxX = (max.X - origin.X) / dir.X;
             float tMaxY = (max.Y - origin.Y) / dir.Y;
             float tMaxZ = (max.Z - origin.Z) / dir.Z;

             if (dir.X < 0)
             {
                 (tMaxX, tMinX) = (tMinX, tMaxX);
             }
             if (dir.Y < 0)
             {
                 (tMaxY, tMinY) = (tMinY, tMaxY);
             }
             if (dir.Z < 0)
             {
                 (tMaxZ, tMinZ) = (tMinZ, tMaxZ);
             }

             float tEnter = Math.Max(tMinX, Math.Max(tMinY, tMinZ));
             float tExit = Math.Min(tMaxX, Math.Min(tMaxY, tMaxZ));
             return tExit > tEnter && tExit >= 0;
          /*  float tmin = 0.0f;
            float tmax = float.PositiveInfinity;

            for (int d = 0; d < 3; ++d)
            {
                bool sign = dir[d] < 0;
                float bmin = sign ? max[d] : min[d];
                float bmax = sign ? min[d] : max[d];

                float dmin = (bmin - origin[d]) * dir[d];
                float dmax = (bmax - origin[d]) * dir[d];

                tmin = Math.Max(dmin, tmin);
                tmax = Math.Min(dmax, tmax);
            }

            return tmin < tmax;*/
            /* float tmin = 0.0f, tmax = float.PositiveInfinity;

            for (int d = 0; d < 3; ++d)
            {
                float t1 = (min[d] - origin[d]) * dir[d];
                float t2 = (max[d] - origin[d]) * dir[d];

                tmin = Math.Max(tmin, Math.Min(Math.Min(t1, t2), tmax));
                tmax = Math.Min(tmax, Math.Max(Math.Max(t1, t2), tmin));
            }

            return tmin < tmax;*/
        }
    }
}
