using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing
{
    public class Material
    {
        public Vector3 diffuseColor { get; set; }

        public Vector3 albedo { get; set; }

        public float specularExponent { get; set; }

        public Material(Vector3 albedo, Vector3 diffuseColor, float specularExponent)
        {
            this.diffuseColor = diffuseColor;
            this.albedo = albedo;
            this.specularExponent = specularExponent;
        }

    }
}
