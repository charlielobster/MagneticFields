using UnityEngine;
using MagneticFields.Geometry;

namespace MagneticFields.Reading
{
    public class CameraReading : MonoBehaviour
    {
        private Material vectorMaterial = default(Material);
        private Vector3 reading;
        private Vector3 position;
        private Color color;

        public float Magnitude { get { return reading.magnitude; } }

        protected Material VectorMaterial
        {
            get
            {
                if (vectorMaterial == default(Material))
                {
                    vectorMaterial = new Material(Shader.Find("Diffuse"));
                }
                return vectorMaterial;
            }
        }

        public Vector3 Reading
        {
            set
            {
                const float MAX = 500f;
                reading = value;
                float r = Magnitude / MAX;
                float g = 1f - Magnitude / MAX;
                Color = new Color(r, g, 0.34f);
            }
            get { return reading; }
        }

        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                VectorMaterial.SetColor("_Color", color);
            }
        }

        void Start()
        {
            VectorMaterial.SetColor("_Color", color);

            var stem = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            stem.GetComponent<Renderer>().material = vectorMaterial;
            stem.transform.localScale = new Vector3(.25f, 1f, .25f);
            stem.transform.parent = this.gameObject.transform;

            var arrowHeadBase = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            arrowHeadBase.GetComponent<Renderer>().material = vectorMaterial;
            arrowHeadBase.transform.localScale = new Vector3(.5f, .01f, .5f);
            arrowHeadBase.transform.position = new Vector3(0, 1f, 0);
            arrowHeadBase.transform.parent = this.gameObject.transform;

            var cone = Cone.CreateCone();
            cone.transform.rotation = Quaternion.Euler(90f, 0, 0);
            cone.transform.position = new Vector3(0, 1.5f, 0);
            cone.transform.localScale = new Vector3(.25f, .25f, .5f);
            cone.transform.parent = this.gameObject.transform;
            cone.GetComponent<MeshRenderer>().material = vectorMaterial;
        
            Reading = new Vector3(0, 1f, 0);

            this.gameObject.transform.localScale = new Vector3(.5f, .5f, .5f);
        }
    }
}

