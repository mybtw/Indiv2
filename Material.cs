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

        public float refractiveIndex { get; set; }


        public float albedo4 { get; set; }

        public Material() 
        {
            this.refractiveIndex = 1;
            this.albedo = new Vector3(1, 0, 0);
            this.albedo4 = 0;
        }

        public Material(float refractiveIndex, float albedo4, Vector3 albedo, Vector3 diffuseColor, float specularExponent)
        {
            this.diffuseColor = diffuseColor;
            this.albedo = albedo;
            this.specularExponent = specularExponent;
            this.refractiveIndex = refractiveIndex;
            this.albedo4 = albedo4;
        }

    }
}
