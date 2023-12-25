using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RayTracing
{
    public partial class Form1 : Form
    {
        Bitmap bitmap;
        int width;
        int height;
        double fov = Math.PI / 2;
        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bitmap;
            width = bitmap.Width; height = bitmap.Height;
            Render();
        }

        private void Render()
        {
            Material ivory = new Material(new Vector3(0.6f,  0.3f, 0), new Vector3(0.4f, 0.4f, 0.3f),   50.0f);
            Material red_rubber = new Material(new Vector3(0.9f,  0.1f, 0), new Vector3(0.3f, 0.1f, 0.1f),   10.0f);
            List<Sphere> spheres = new List<Sphere>
            {
                new Sphere(new Vector3(-3, 0, -16), 2, ivory),
                new Sphere(new Vector3(-1.0f, -1.5f, -12), 2, red_rubber),
                new Sphere(new Vector3(1.5f, -0.5f, -18), 3, red_rubber),
                new Sphere(new Vector3(7, 5, -18), 4, ivory)
        };

            List<Light> lights = new List<Light>
            {
               // new Light(new Vector3(0, 0, -1), 1.5f),
                new Light(new Vector3(-20, 20, 20), 1.5f),
                new Light(new Vector3( 30, 50, -25), 1.8f),
                new Light(new Vector3(30, 20, 30), 1.7f)
        };

            Color[] buffer = new Color[pictureBox1.Width * pictureBox1.Height];
            {
                // Parallel.For(0, pictureBox1.Height, j =>
                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        float x = (2 * (i + 0.5f) / width - 1) * (float)Math.Tan(fov / 2) * width / height;
                        float y = -(2 * (j + 0.5f) / height - 1) * (float)Math.Tan(fov / 2);
                        Vector3 dir = Vector3.UnitVector(new Vector3(x, y, -1));
                        Vector3 coefs = CastRay(new Vector3(0, 0, 0), dir, spheres, lights);
                        float max = Math.Max(coefs[0], Math.Max(coefs[1], coefs[2]));
                        if (max > 1) coefs = coefs * (1.0f/ max);
                        buffer[i + j * width] = Color.FromArgb((int)(255 * coefs.X), (int)(255 * coefs.Y), (int)(255 * coefs.Z));
                    }
                }
            }

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    bitmap.SetPixel(x, y, buffer[x + y * pictureBox1.Width]);
                }
            }
            pictureBox1.Invalidate();
        }

        public Vector3 CastRay(Vector3 orig, Vector3 dir, List<Sphere> spheres, List<Light> lights)
        {
            Vector3 point, N;
            Material material;
            if (!SceneIntersect(orig, dir, spheres, out point, out N, out material))
            {
                return new Vector3(0.2f, 0.7f, 0.8f);
            }
            

            float diffuseLightIntensity = 0;
            float specularLightIntensity = 0;

            foreach (Light l in lights)
            {
                Vector3 lightDir = Vector3.UnitVector(l.position - point);
                float lightDistance = (l.position - point).Length();
                Vector3 shadowOrig = Vector3.Dot(lightDir, N) < 0 ? point - N * 1e-3f : point + N * 1e-3f;

                Vector3 shadow_pt, shadow_N;
                Material tmpmaterial;
                if (SceneIntersect(shadowOrig, lightDir, spheres,out shadow_pt, out shadow_N, out tmpmaterial) && (shadow_pt - shadowOrig).Length() < lightDistance)
                    continue;


                diffuseLightIntensity += l.intensity * Math.Max(0, Vector3.Dot(lightDir, N));
                specularLightIntensity += (float)Math.Pow(Math.Max(0.0f, Vector3.Dot(reflect(lightDir, N), dir)), material.specularExponent) * l.intensity;
            }

            return material.diffuseColor * diffuseLightIntensity * material.albedo[0] + new Vector3(1.0f, 1.0f, 1.0f) * specularLightIntensity * material.albedo[1];
        }

        public static bool SceneIntersect(Vector3 orig, Vector3 dir, List<Sphere> spheres, out Vector3 hit, out Vector3 N, out Material material)
        {
            float spheresDist = float.MaxValue;
            hit = new Vector3(0, 0, 0);
            N = new Vector3(0, 0, 0);
            material = new Material(new Vector3(0, 0, 0), new Vector3(0, 0, 0), 0);

            foreach (Sphere sphere in spheres)
            {
                float distI;
                if (sphere.RayIntersect(orig, dir, out distI) && distI < spheresDist)
                {
                    spheresDist = distI;
                    hit = orig + dir * distI;
                    N = Vector3.UnitVector((hit - sphere.Center));
                    material = sphere.material;
                }
            }

            return spheresDist < 1000.0f;
        }

        Vector3 reflect(Vector3 I, Vector3 N) {
            return I - N * 2.0f * (Vector3.Dot(I, N));
        }
    }
}
