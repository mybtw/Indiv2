using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing
{
    public class Sphere : SceneObject
    {
        public Vector3 Center { get; private set; }
        public float Radius { get; private set; }

        public Material material { get;  set; }

        public Sphere(Vector3 center, float radius, Material material)
        {
            Center = center;
            Radius = radius;
            this.material = material;
        }
        public override bool RayIntersect(Vector3 orig, Vector3 dir, out float t0)
        {
            t0 = 0;
            Vector3 L = Center - orig;
            float tca = Vector3.Dot(L, dir);
            float d2 = Vector3.Dot(L, L) - tca * tca;
            if (d2 > Radius * Radius) return false;
            float thc = (float)Math.Sqrt(Radius * Radius - d2);
            t0 = tca - thc;
            float t1 = tca + thc;
            if (t0 < 0) t0 = t1;
            if (t0 < 0) return false;
            return true;
        }

   

        public override Vector3 getNormal(Vector3 hit)
        {
            return Vector3.UnitVector((hit - this.Center));
        }

        public override Material getMaterial()
        {
            return material;
        }
    }
}
