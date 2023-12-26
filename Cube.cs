using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                new Vector3(d, d, -d- 5),  // 2 C
                new Vector3(-d, d, -d- 5 ),  // 3 D
                new Vector3(-d, -d, d - 5),  // 4 E
                new Vector3(d, -d, d- 5),  // 5 F
                new Vector3(d, d, d- 5),  // 6 G
                new Vector3(-d, d, d- 5)  // 7 H
            };

            // Define indices for 12 triangles (2 per face)
            faces = new List<Vector3>
            {
                // Front face
             /*   new Vector3(1, 3, 0), // B D A
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
                new Vector3(4, 5, 1), // E F B
                // Front face*/
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

        public void RotateVerticesAroundY(float angleInDegrees)
        {
            float angleInRadians = (float)Math.PI / 180 * angleInDegrees;
            float cosTheta = (float)Math.Cos(angleInRadians);
            float sinTheta = (float)Math.Sin(angleInRadians);

            for (int i = 0; i < verts.Count; i++)
            {
                Vector3 vertex = verts[i];
                float x = vertex.X;
                float z = vertex.Z;

                // Apply the rotation matrix
                verts[i] = new Vector3(
                    cosTheta * x + sinTheta * z,
                    vertex.Y,
                    -sinTheta * x + cosTheta * z
                );
            }
        }
        public void RotateVerticesAroundZ(float angleInDegrees)
        {
            float angleInRadians = (float)Math.PI / 180 * angleInDegrees;
            float cosTheta = (float)Math.Cos(angleInRadians);
            float sinTheta = (float)Math.Sin(angleInRadians);

            for (int i = 0; i < verts.Count; i++)
            {
                Vector3 vertex = verts[i];
                float x = vertex.X;
                float y = vertex.Y;

                // Apply the rotation matrix
                verts[i] = new Vector3(
                    cosTheta * x - sinTheta * y,
                    sinTheta * x + cosTheta * y,
                    vertex.Z
                );
            }
        }


        public void RotateVerticesAroundX(float angleInDegrees)
        {
            float angleInRadians = (float)Math.PI / 180 * angleInDegrees;
            float cosTheta = (float)Math.Cos(angleInRadians);
            float sinTheta = (float)Math.Sin(angleInRadians);

            for (int i = 0; i < verts.Count; i++)
            {
                Vector3 vertex = verts[i];
                float y = vertex.Y;
                float z = vertex.Z;

                // Apply the rotation matrix
                verts[i] = new Vector3(
                    vertex.X,
                    cosTheta * y - sinTheta * z,
                    sinTheta * y + cosTheta * z
                );
            }
        }




        public override int NVerts()
        {
            return verts.Count;
        }

        public override int NFaces()
        {
            return faces.Count;
        }

        private const float kEpsilon = 1e-8f;

        public override bool RayTriangleIntersect(int fi, Vector3 orig, Vector3 dir, out float t)
        {
            var v1 = Point(Vert(fi, 1));
            var v0 = Point(Vert(fi, 0));
            var v2 = Point(Vert(fi, 2));
            // Compute the plane's normal
            Vector3 v0v1 = v1 - v0;
            Vector3 v0v2 = v2 - v0;
            // No need to normalize
            Vector3 N = Vector3.Cross(v0v1, v0v2); // N
            float area2 = N.Length();

            // Step 1: finding P

            // Check if the ray and plane are parallel
            float NdotRayDirection = Vector3.Dot(N, dir);
            if (Math.Abs(NdotRayDirection) < kEpsilon) // Almost 0
            {
                t = 0;
                return false; // They are parallel, so they don't intersect!
            }

            // Compute d parameter using equation 2
            float d = -Vector3.Dot(N, v0);

            // Compute t (equation 3)
            t = -(Vector3.Dot(N, orig) + d) / NdotRayDirection;

            // Check if the triangle is behind the ray
            if (t < 0) return false; // The triangle is behind

            // Compute the intersection point using equation 1
            Vector3 P = orig + t * dir;

            // Step 2: inside-outside test
            Vector3 C; // Vector perpendicular to triangle's plane

            // Edge 0
            Vector3 edge0 = v1 - v0;
            Vector3 vp0 = P - v0;
            C = Vector3.Cross(edge0, vp0);
            if (Vector3.Dot(N, C) < 0) return false; // P is on the right side

            // Edge 1
            Vector3 edge1 = v2 - v1;
            Vector3 vp1 = P - v1;
            C = Vector3.Cross(edge1, vp1);
            if (Vector3.Dot(N, C) < 0) return false; // P is on the right side

            // Edge 2
            Vector3 edge2 = v0 - v2;
            Vector3 vp2 = P - v2;
            C = Vector3.Cross(edge2, vp2);
            if (Vector3.Dot(N, C) < 0) return false; // P is on the right side

            return true; // This ray hits the triangle
        }

        public void RotateAroundOwnAxisY(float angleDegrees)
        {
            Vector3 center = CalculateCenter();
            MoveToOrigin(center);

            RotateY(angleDegrees);

            MoveBackFromOrigin(center);
        }


        public void RotateAroundOwnAxisX(float angleDegrees)
        {
            Vector3 center = CalculateCenter();
            MoveToOrigin(center);

            RotateX(angleDegrees);

            MoveBackFromOrigin(center);
        }

        private void RotateX(float angleDegrees)
        {
            float angleRadians = (float)(Math.PI / 180 * angleDegrees);
            float cosTheta = (float)Math.Cos(angleRadians);
            float sinTheta = (float)Math.Sin(angleRadians);

            for (int i = 0; i < verts.Count; i++)
            {
                Vector3 vertex = verts[i];
                verts[i] = new Vector3(
                    vertex.X,
                    cosTheta * vertex.Y - sinTheta * vertex.Z,
                    sinTheta * vertex.Y + cosTheta * vertex.Z
                );
            }
        }

        public void RotateAroundOwnAxisZ(float angleDegrees)
        {
            Vector3 center = CalculateCenter();
            MoveToOrigin(center);

            RotateZ(angleDegrees);

            MoveBackFromOrigin(center);
        }

        private void RotateZ(float angleDegrees)
        {
            float angleRadians = (float)(Math.PI / 180 * angleDegrees);
            float cosTheta = (float)Math.Cos(angleRadians);
            float sinTheta = (float)Math.Sin(angleRadians);

            for (int i = 0; i < verts.Count; i++)
            {
                Vector3 vertex = verts[i];
                verts[i] = new Vector3(
                    cosTheta * vertex.X - sinTheta * vertex.Y,
                    sinTheta * vertex.X + cosTheta * vertex.Y,
                    vertex.Z
                );
            }
        }


        private Vector3 CalculateCenter()
        {
            Vector3 sum = new Vector3(0, 0, 0);
            foreach (var vertex in verts)
            {
                sum += vertex;
            }
            return sum / verts.Count;
        }

        private void MoveToOrigin(Vector3 center)
        {
            for (int i = 0; i < verts.Count; i++)
            {
                verts[i] -= center;
            }
        }

        private void RotateY(float angleDegrees)
        {
            float angleRadians = (float)(Math.PI / 180 * angleDegrees);
            float cosTheta = (float)Math.Cos(angleRadians);
            float sinTheta = (float)Math.Sin(angleRadians);

            for (int i = 0; i < verts.Count; i++)
            {
                Vector3 vertex = verts[i];
                verts[i] = new Vector3(
                    cosTheta * vertex.X + sinTheta * vertex.Z,
                    vertex.Y,
                    -sinTheta * vertex.X + cosTheta * vertex.Z
                );
            }
        }

        private void MoveBackFromOrigin(Vector3 center)
        {
            for (int i = 0; i < verts.Count; i++)
            {
                verts[i] += center;
            }
        }

        /* public override bool RayTriangleIntersect(int fi, Vector3 orig, Vector3 dir, out float tnear)
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
         }*/
        /* public bool RayIntersect(Vector3 orig, Vector3 dir)
         {
             float t = float.MaxValue;
             Vector3 face;
             bool isIntersection = false;
             for (int i = 0; i < faces.Count; i++)
             {
                 float tri;
                 var r = RayTriangleIntersect(i, orig, dir,out tri);
                 if (r && tri < t)
                 {
                     t = tri;
                     face = faces[i];
                     isIntersection = true;
                 }
             }
             if (isIntersection)
             {
                 Vector3 hitPoint = orig + t * dir;
                 Vector3 normal = Vector3.UnitVector(Vector3.Cross(((verts[face.Y] - verts[face.X]), (verts[face.Z] - verts[face.X])));
                 return true;
             }
         }*/

        /*std::optional<IntersectResult> Model::rayIntersectOld(const Ray& ray) const {
    if(box.intersect(ray)) {
        float t = std::numeric_limits<float>::max();
        vec3i face;
        bool isIntersection = false;
        std::array<vec3f,3> tri;
        for(const auto& f: faces) {
            tri[0] = vertices[f.x];
            tri[1] = vertices[f.y];
            tri[2] = vertices[f.z];
            auto r = rayIntersectWithTriangle(ray, tri);
            if(r.has_value() && r.value() < t) {
                t = r.value();
                isIntersection = true;
                face = f;
            }
        }
        
        if(isIntersection) {
            vec3f hitPoint = ray.o + t * ray.d;
            vec3f normal = cross((vertices[face.y] - vertices[face.x]), (vertices[face.z] - vertices[face.x])).normalize();
            return IntersectResult{t, hitPoint, material, normal};
        }
    }
    return std::nullopt;
}*/

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