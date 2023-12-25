using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing
{
    public abstract class SceneObject
    {
        public abstract bool RayIntersect(Vector3 orig, Vector3 dir,out float t);
        public abstract Vector3 getNormal(Vector3 hit);
        public abstract Material getMaterial();
    }
}
