using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
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
            /*    Material      ivory(1.0, Vec4f(0.6,  0.3, 0.1, 0.0), Vec3f(0.4, 0.4, 0.3),   50.);
    Material      glass(1.5, Vec4f(0.0,  0.5, 0.1, 0.8), Vec3f(0.6, 0.7, 0.8),  125.);
    Material red_rubber(1.0, Vec4f(0.9,  0.1, 0.0, 0.0), Vec3f(0.3, 0.1, 0.1),   10.);
    Material     mirror(1.0, Vec4f(0.0, 10.0, 0.8, 0.0), Vec3f(1.0, 1.0, 1.0), 1425.);*/
            Material transparentMaterial = new Material(
    refractiveIndex: 10f, // Показатель преломления для стекла
    albedo4: 1f, // Высокая прозрачность
    albedo: new Vector3(0.0f, 0.3f, 0.1f), // Низкое поглощение света
    diffuseColor: new Vector3(1.0f, 1.0f, 1.0f), // Слабый или отсутствующий цвет
    specularExponent: 125.0f // Высокий блеск
);
            Material ivory = new Material(1.0f, 0.0f, new Vector3(0.6f,  0.3f, 0.1f), new Vector3(0.4f, 0.4f, 0.3f),   50.0f);
            Material glass = new Material(1.5f, 0.8f, new Vector3(0.0f, 0.5f, 0.1f), new Vector3(0.6f, 0.7f, 0.8f),  125.0f);
            Material red_rubber = new Material(1.0f, 0.0f, new Vector3(0.8f,  0.1f, 0), new Vector3(0.3f, 0.1f, 0.1f),   10.0f);
            Material blue_rubber = new Material(1.0f, 0.0f, new Vector3(0.6f, 0.1f, 0), new Vector3(0.051f, 0.212f, 0.8f), 10.0f);
            Material white_rubber = new Material(1.0f, 0.0f, new Vector3(0.6f, 0.1f, 0), new Vector3(1f, 1f, 1f), 10.0f);
            Material yellow_rubber = new Material(1.0f, 0.0f, new Vector3(0.6f, 0.1f, 0), new Vector3(0.882f, 0.929f, 0.09f), 10.0f);
            Material mirror = new Material(1.0f, 0.0f, new Vector3(0.0f, 10.0f, 0.8f), new Vector3(1.0f, 1.0f, 1.0f), 1425.0f);

            /*    
    Material      glass(1.5, Vec4f(0.0,  0.5, 0.1, 0.8), Vec3f(0.6, 0.7, 0.8),  125.);
    Material red_rubber(1.0, Vec4f(0.9,  0.1, 0.0, 0.0), Vec3f(0.3, 0.1, 0.1),   10.);
    Material     mirror(1.0, Vec4f(0.0, 10.0, 0.8, 0.0), Vec3f(1.0, 1.0, 1.0), 1425.);*/
            Cube cube = new Cube(1.2f, yellow_rubber);
           // cube.Translate(new Vector3(0, -2, 0));
            cube.RotateAroundOwnAxisY(50);
            cube.Translate(new Vector3(0, 0,-3.4f));
            cube.Translate(new Vector3(-1, -1.7f, 0));
            cube.ScaleY(2f);

            Cube cube1 = new Cube(1.1f, yellow_rubber);
            // cube.Translate(new Vector3(0, -2, 0));
            cube1.RotateAroundOwnAxisY(15);
            cube1.Translate(new Vector3(0, 0, -2.8f));
            cube1.Translate(new Vector3(1, -1.8f, 0));
            // Front face
            // Back face
            // Left face
            // Right face
            // Top face
            // Bottom face

            Room room = new Room(5, new List<Material> { white_rubber, white_rubber, white_rubber, white_rubber, red_rubber, red_rubber, blue_rubber, blue_rubber, white_rubber, white_rubber, white_rubber, white_rubber });
            room.Translate(new Vector3(0, 0, -2.2f));
           // room.Translate(new Vector3(0, 2f, 0));
            //  cube.RotateVerticesAroundX(10);
            //cube.RotateVerticesAroundY(40);
            //cube.RotateAroundOwnAxisZ(50);
            // cube.RotateVerticesAroundY(-10);


            List<SceneObject> spheres = new List<SceneObject>
            {
                room,
                cube,
                cube1,
                new Sphere(new Vector3(1,-0.5f,-2.8f), 0.6f, red_rubber),
               // new Sphere(new Vector3(-1.0f, -1.5f, -12), 2, glass),
               // new Sphere(new Vector3(1.5f, -0.5f, -18), 3, red_rubber),
               // new Sphere(new Vector3(7, 5, -18), 4, mirror),
        };
   

            List<Light> lights = new List<Light>
            {
              //  new Light(new Vector3(0, 0.2f, -1), 0.7f),
                new Light(new Vector3(0, 2.4f, -2.4f), 1.3f),
               // new Light(new Vector3( 30, 50, -25), 1.8f),
               // new Light(new Vector3(30, 20, 30), 1.7f)
            };

            Color[] buffer = new Color[pictureBox1.Width * pictureBox1.Height];
            {
                //Parallel.For(0, pictureBox1.Height, j =>
                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        float x = (2 * (i + 0.5f) / width - 1) * (float)Math.Tan(fov / 2) * width / height;
                        float y = -(2 * (j + 0.5f) / height - 1) * (float)Math.Tan(fov / 2);
                        Vector3 dir = Vector3.UnitVector(new Vector3(x, y, -1));
                        Vector3 coefs = CastRay(new Vector3(0, 0, 0), dir, spheres, lights);
                        float max = Math.Max(coefs[0], Math.Max(coefs[1], coefs[2]));
                        if (max > 1) coefs = coefs * (1.0f / max);
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

        public Vector3 CastRay(Vector3 orig, Vector3 dir, List<SceneObject> spheres, List<Light> lights, int depth = 0)
        {
            Vector3 point, N;
            Material material;
        
            if (depth > 5 || !SceneIntersect(orig, dir, spheres,  out point, out N, out material))
            {
                return new Vector3(0.2f, 0.7f, 0.8f);
            }

            Vector3 reflectDir = Vector3.UnitVector(reflect(dir, N));
            Vector3 refractDir = Vector3.UnitVector(refract(dir, N, material.refractiveIndex));

            Vector3 reflectOrig = Vector3.Dot(reflectDir, N) < 0 ? point - N * 1e-3f : point + N * 1e-3f; // offset the original point to avoid occlusion by the object itself
            Vector3 reflectColor = CastRay(reflectOrig, reflectDir, spheres,  lights, depth + 1);

            Vector3 refractOrig = Vector3.Dot(refractDir, N) < 0 ? point - N * 1e-3f : point + N * 1e-3f;
            Vector3 refractColor = CastRay(refractOrig, refractDir, spheres, lights, depth + 1);

            float diffuseLightIntensity = 0;
            float specularLightIntensity = 0;

            foreach (Light l in lights)
            {
                Vector3 lightDir = Vector3.UnitVector(l.position - point);
                float lightDistance = (l.position - point).Length();
                Vector3 shadowOrig = Vector3.Dot(lightDir, N) < 0 ? point - N * 1e-3f : point + N * 1e-3f;

                Vector3 shadow_pt, shadow_N;
                Material tmpmaterial;
                if (SceneIntersect(shadowOrig, lightDir, spheres, out shadow_pt, out shadow_N, out tmpmaterial) && (shadow_pt - shadowOrig).Length() < lightDistance)
                    continue;


                diffuseLightIntensity += l.intensity * Math.Max(0, Vector3.Dot(lightDir, N));
                specularLightIntensity += (float)Math.Pow(Math.Max(0.0f, Vector3.Dot(reflect(lightDir, N), dir)), material.specularExponent) * l.intensity;
            }

            return material.diffuseColor * diffuseLightIntensity * material.albedo[0] + new Vector3(1.0f, 1.0f, 1.0f) * specularLightIntensity * material.albedo[1] + reflectColor * material.albedo[2] + refractColor * material.albedo4; ;
        }

        public static bool SceneIntersect(Vector3 orig, Vector3 dir, List<SceneObject> spheres, out Vector3 hit, out Vector3 N, out Material material)
        {
            float spheresDist = float.MaxValue;
            hit = new Vector3(0, 0, 0);
            N = new Vector3(0, 0, 0);
            material = new Material(0, 0, new Vector3(0, 0, 0), new Vector3(0, 0, 0), 0);
            foreach (SceneObject sphere in spheres)
            {
                float distI;
                if (sphere.RayIntersect(orig, dir, out distI) && distI < spheresDist)
                {
                    spheresDist = distI;
                    hit = orig + dir * distI;
                    N = sphere.getNormal(hit);
                    material = sphere.getMaterial();
                }
               
            }
            /* float triangles_dist = float.MaxValue;
             for (int i = 0; i < model.NFaces(); i++)
             {
                 Vector3 edge1 = model.Point(model.Vert(i, 1)) - model.Point(model.Vert(i, 0));
                 Vector3 edge2 = model.Point(model.Vert(i, 2)) - model.Point(model.Vert(i, 0));
                 var normalVal = Vector3.UnitVector(Vector3.Cross(edge1, edge2));
                 float tnear;
                 if (model.RayTriangleIntersect(i, orig, dir, out tnear) && tnear < triangles_dist)
                 {
                     triangles_dist = tnear;
                     hit = orig + tnear * dir;
                     N = normalVal;
                     material = model.material;
                 }
             }*/

            //return Math.Min(spheresDist, triangles_dist) < 1000.0f;
            return spheresDist < 1000.0f;
        }

        /*Vec3f refract(const Vec3f &I, const Vec3f &N, const float &refractive_index) { // Snell's law
    float cosi = - std::max(-1.f, std::min(1.f, I*N));
    float etai = 1, etat = refractive_index;
    Vec3f n = N;
    if (cosi < 0) { // if the ray is inside the object, swap the indices and invert the normal to get the correct result*/

        Vector3 reflect(Vector3 I, Vector3 N) {
            return I - N * 2.0f * (Vector3.Dot(I, N));
        }

        Vector3 refract(Vector3 I, Vector3 N, float eta_t, float eta_i = 1.0f) { // Snell's law
            float cosi = - Math.Max(-1.0f, Math.Min(1.0f, Vector3.Dot(I, N)));
            if (cosi < 0) { // if the ray is inside the object, swap the indices and invert the normal to get the correct result
                return refract(I, -N, eta_i, eta_t);
            }
            float eta = eta_i / eta_t;
            float k = 1 - eta * eta * (1 - cosi * cosi);
            return k < 0 ? new Vector3(1,0,0) : I * eta + N * (eta * cosi - (float)Math.Sqrt(k));
        }
    }
}
