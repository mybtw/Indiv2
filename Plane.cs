using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing
{
    public class Plane : SceneObject
    {
        private Vector3 normal;
        private float distance;
        private Material material;

        public Plane(Vector3 normal, float distance, Material material)
        {
            this.normal = normal;
            this.distance = distance;
            this.material = material;
        }

        public override bool RayIntersect(Vector3 orig, Vector3 dir, out float t)
        {
            t = 0;
            float denom = Vector3.Dot(normal, dir);

            if (Math.Abs(denom) > 0.0001f) // Avoid division by zero
            {
                Vector3 p0l0 = -normal * distance - orig;
                t = Vector3.Dot(p0l0, normal) / denom;
                return t >= 0;
            }

            return false;
        }

        public override Vector3 getNormal(Vector3 hit)
        {
            return normal;
        }

        public override Material getMaterial()
        {
            return material;
        }
    }

}
