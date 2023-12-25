using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing
{
    public class Light
    {
        public Vector3 position { get; set; }
        public float intensity { get; set; }


        public Light(Vector3 position, float intensity)
        {
            this.position = position;
            this.intensity = intensity;
        }
    }
}
