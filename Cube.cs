using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing
{
    public class Cube : Model
    {
        public List<Vector3> verts { get; set; }
        public List<Vector3> faces { get; set; }

        public Material material { get; set; }

        public Cube(float sideLen, Material material)
        {
            this.material = material;
            float d = sideLen / 2; // Half the side length

            // Define vertices
            verts = new List<Vector3>
            {
                new Vector3(-d, -d, -d - 5), // 0 A
                new Vector3(d, -d, -d - 5), // 1 B
                new Vector3(d, d, -d - 5),  // 2 C
                new Vector3(-d, d, -d - 5),  // 3 D
                new Vector3(-d, -d, d - 5),  // 4 E
                new Vector3(d, -d, d - 5),  // 5 F
                new Vector3(d, d, d - 5),  // 6 G
                new Vector3(-d, d, d - 5)  // 7 H
            };

            // Define indices for 12 triangles (2 per face)
            faces = new List<Vector3>
            {
             /*   // Front face
                new Vector3(1, 3, 0), // B D A
                new Vector3(1, 2, 3),// B C D
                // Back face
                new Vector3(4, 6, 5), // E G F
                new Vector3(4, 7, 6),// E H G
                // Left face
                new Vector3(4, 3, 7), // E D H
                new Vector3(4, 0, 3),// E A D
                // Right face
                new Vector3(1, 5, 6), // B F G
                new Vector3(1, 6, 2),// B G C
                // Top face
                new Vector3(3, 2, 6), // D C G
                new Vector3(3, 6, 7),// D G H
                // Bottom face
                new Vector3(4, 1, 0), // E B A
                new Vector3(4, 5, 1),// E F B*/
                // Front face
                new Vector3(0, 3, 1), // B D A
                new Vector3(3, 2, 1),// B C D
                // Back face
                new Vector3(5, 6, 4), // E G F
                new Vector3(6, 7, 4),// E H G
                // Left face
                new Vector3(7, 3, 4), // E D H
                new Vector3(3, 0, 4),// E A D
                // Right face
                new Vector3(6, 5, 1), // B F G
                new Vector3(2, 6, 1),// B G C
                // Top face
                new Vector3(6, 2, 3), // D C G
                new Vector3(7, 6, 3),// D G H
                // Bottom face
                new Vector3(0, 1, 4), // E B A
                new Vector3(1, 5, 4),// E F B
            };
        }

      /*  public bool RayIntersect(Vector3 orig, Vector3 dir, out float t) 
        {
            t = float.MaxValue;
            bool isIntersection = false;
            for (int i = 0; i < faces.Count; i++)
            {
                float tnear;
                isIntersection = RayTriangleIntersect(0, orig, dir, out tnear);
                if (isIntersection && tnear < t)
                {
                    t = tnear;
                }
            }    
        }*/


        public override int NVerts()
        {
            return verts.Count;
        }

        public override int NFaces()
        {
            return faces.Count;
        }

        public override bool RayTriangleIntersect(int fi, Vector3 orig, Vector3 dir, out float tnear)
        {
            Vector3 edge1 = Point(Vert(fi, 1)) - Point(Vert(fi, 0));
            Vector3 edge2 = Point(Vert(fi, 2)) - Point(Vert(fi, 0));
            Vector3 pvec = Vector3.Cross(dir, edge2);
            float det = Vector3.Dot(edge1, pvec);

            if (det < 1e-5)
            {
                tnear = 0;
                return false;
            }

            Vector3 tvec = orig - Point(Vert(fi, 0));
            float u = Vector3.Dot(tvec, pvec);
            if (u < 0 || u > det)
            {
                tnear = 0;
                return false;
            }

            Vector3 qvec = Vector3.Cross(tvec, edge1);
            float v = Vector3.Dot(dir, qvec);
            if (v < 0 || u + v > det)
            {
                tnear = 0;
                return false;
            }

            tnear = Vector3.Dot(edge2, qvec) * (1.0f / det);
            return tnear > 1e-5;

            /*vec3f E1 = tri[1] - tri[0];
    vec3f E2 = tri[2] - tri[0];
    vec3f S = orig - tri[0];
    vec3f S1 = cross(d, E2);
    vec3f S2 = cross(S, E1);
    vec3f v = vec3f(S2 * E2, S1 * S, S2 * d);
    vec3f r = 1 / (S1 * E1) * v;
    if(r.x > 0 && r.y > 0 && r.z > 0 && (1 - r.y - r.z) > 0) {
        return r.x;
    } else {
        return std::nullopt;
    }
             */
        }

        public override Vector3 Point(int i)
        {
            return verts[i];
        }

        public override int Vert(int fi, int li)
        {
            return (int)faces[fi][li];
        }

        public override void GetBBox(out Vector3 min, out Vector3 max)
        {
            throw new NotImplementedException();
        }
    }



}