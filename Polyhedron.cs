using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Windows.Forms;
using System.Drawing;

namespace RayTracing
{
    public class Edge
    {
        public List<Vector3> points;
        public Color color = Color.Brown;
        public Edge()
        {
            this.points = new List<Vector3> { };
            this.color = Color.Brown;
        }
        public Edge(List<Vector3> p)
        {
            this.points = p;
        }
    }
    public class Polyhedron : SceneObject
    {
        public List<Edge> edges;
        public double[] materialK = new double[5];
        public Material material;
        public Polyhedron()
        {
            this.edges = new List<Edge> { };
        }
        public Polyhedron(List<Edge> e)
        {
            this.edges = e;
        }
        public bool RayIntersectsTriangle(Vector3 orig, Vector3 dir, Vector3 p0, Vector3 p1, Vector3 p2, out double intersect)
        {
            var eps = 0.0001;
            Vector3 edge1 = p1 - p0;
            Vector3 edge2 = p2 - p0;
            Vector3 h = dir * edge2;
            double a = Vector3.Dot(edge1, h);
            intersect = -1;
            if (a > -eps && a < eps)
                return false;

            Vector3 s = orig - p0;
            double u = Vector3.Dot(s, h) / a;
            if (u < 0 || u > 1)
                return false;

            Vector3 q = s * edge1;
            double v = Vector3.Dot(dir, q) / a;
            if (v < 0 || u + v > 1)
                return false;

            double t = Vector3.Dot(edge2, q) / a;
            if (t <= eps)
                return false;

            intersect = t;
            return true;
        }
        public virtual bool FigureIntersection(Vector3 orig, Vector3 dir, out double intersect, out Vector3 normal)
        {
            intersect = 0;
            normal = null;
            Edge side = null;
            foreach (var figure_side in edges)
            {
                //треугольная сторона
                if (figure_side.points.Count == 3)
                {
                    if (RayIntersectsTriangle(orig, dir, figure_side.points[0], figure_side.points[1], figure_side.points[2], out double t) && (intersect == 0 || t < intersect))
                    {
                        intersect = t;
                        side = figure_side;
                    }
                }

                //четырехугольная сторона
                else if (figure_side.points.Count == 4)
                {
                    if (RayIntersectsTriangle(orig, dir, figure_side.points[0], figure_side.points[1], figure_side.points[3], out double t) && (intersect == 0 || t < intersect))
                    {
                        intersect = t;
                        side = figure_side;
                    }
                    else if (RayIntersectsTriangle(orig, dir, figure_side.points[1], figure_side.points[2], figure_side.points[3], out t) && (intersect == 0 || t < intersect))
                    {
                        intersect = t;
                        side = figure_side;
                    }
                }
            }
            if (intersect != 0)
            {
                normal = Vector3.norm(side);
                //material_color = new Vector3(side.color.R / 255f, side.color.G / 255f, side.color.B / 255f);
                return true;
            }
            return false;
        }

        public static Polyhedron Hex(int size)
        {
            var hc = size / 2;
            Polyhedron p = new Polyhedron();
            Edge e = new Edge();
            // 1-2-3-4
            e.points = new List<Vector3> {
                new Vector3(-hc, hc, -hc), // 1
                new Vector3(hc, hc, -hc), // 2
                new Vector3(hc, -hc, -hc), // 3
                new Vector3(-hc, -hc, -hc) // 4
            };
            p.edges.Add(e);
            e = new Edge();

            // 1-2-6-5
            e.points = new List<Vector3> {

                new Vector3(-hc, hc, -hc), // 1
                new Vector3(-hc, hc, hc), // 5
                new Vector3(hc, hc, hc), // 6 
                new Vector3(hc, hc, -hc) // 2
            };
            p.edges.Add(e);
            e = new Edge();

            // 5-6-7-8
            e.points = new List<Vector3> {
                new Vector3(-hc, hc, hc), // 5
                new Vector3(-hc, -hc, hc), // 8
                new Vector3(hc, -hc, hc), // 7
                new Vector3(hc, hc, hc) // 6 
            };
            p.edges.Add(e);
            e = new Edge();

            // 6-2-3-7
            e.points = new List<Vector3> {
                new Vector3(hc, hc, hc), // 6 
                new Vector3(hc, -hc, hc), // 7
                new Vector3(hc, -hc, -hc), // 3
                new Vector3(hc, hc, -hc) // 2
            };
            p.edges.Add(e);
            e = new Edge();

            // 5-1-4-8
            e.points = new List<Vector3> {
                new Vector3(-hc, hc, hc), // 5
                new Vector3(-hc, hc, -hc), // 1
                new Vector3(-hc, -hc, -hc), // 4
                new Vector3(-hc, -hc, hc) // 8
            };
            p.edges.Add(e);
            e = new Edge();

            // 4-3-7-8
            e.points = new List<Vector3> {
                new Vector3(-hc, -hc, -hc), // 4
                new Vector3(hc, -hc, -hc), // 3
                new Vector3(hc, -hc, hc), // 7
                new Vector3(-hc, -hc, hc) // 8
            };
            p.edges.Add(e);
            e = new Edge();

            return p;
        }

        public override bool RayIntersect(Vector3 orig, Vector3 dir, out float t)
        {
            throw new NotImplementedException();
        }

        public override Vector3 getNormal(Vector3 hit)
        {
            throw new NotImplementedException();
        }

        public override Material getMaterial()
        {
            return material;
        }
    }
}
