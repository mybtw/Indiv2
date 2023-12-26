using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing
{
    public abstract class Model
    {
        public abstract int NVerts(); // Number of vertices
        public abstract int NFaces(); // Number of triangles

        public abstract bool RayTriangleIntersect(int fi, Vector3 orig, Vector3 dir, out float tnear);

        public abstract Vector3 Point(int i); // Coordinates of the vertex i
        public abstract int Vert(int fi, int li); // Index of the vertex for the triangle fi and local index li
        public abstract void GetBBox(out Vector3 min, out Vector3 max); // Bounding box for all the vertices, including isolated ones
    }
}
