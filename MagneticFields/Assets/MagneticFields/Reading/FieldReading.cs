using System;
using UnityEngine;
using MagneticFields.Geometry;
using MagneticFields.UI;

namespace MagneticFields.Reading
{
    public class FieldReading : MonoBehaviour
    {
        //private Axis axis;
        private Material vectorMaterial;
        private Vector3 reading;
        private Vector3 position;
        private Color color;
        private WorldCanvas canvas;
        private GameObject canvasObject;

        private GameObject stem;
        private GameObject arrowHeadBase;
        private GameObject cone;

        public FieldReading()
        {
            vectorMaterial = new Material(Shader.Find("Diffuse"));
            vectorMaterial.SetColor("_Color", color);

            stem = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            stem.GetComponent<Renderer>().material = vectorMaterial;
            stem.transform.localScale = new Vector3(.25f, 1f, .25f);
            stem.transform.parent = this.gameObject.transform;

            arrowHeadBase = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            arrowHeadBase.GetComponent<Renderer>().material = vectorMaterial;
            arrowHeadBase.transform.localScale = new Vector3(.5f, .01f, .5f);
            arrowHeadBase.transform.position = new Vector3(0, 1f, 0);
            arrowHeadBase.transform.parent = this.gameObject.transform;

            cone = Cone.CreateCone();
            cone.transform.rotation = Quaternion.Euler(90f, 0, 0);
            cone.transform.position = new Vector3(0, 1.5f, 0);
            cone.transform.localScale = new Vector3(.25f, .25f, .5f);
            cone.transform.parent = this.gameObject.transform;
            cone.GetComponent<MeshRenderer>().material = vectorMaterial;

            //var axisObject = new GameObject();
            //axis = axisObject.AddComponent<Axis>();
            //axis.unitLength = .03125f;
            //axis.widthMultiplier = 0.001f;

            //canvasObject = new GameObject();
            //canvas = canvasObject.AddComponent<WorldCanvas>();
            //canvasObject.SetActive(false);

        }

        public Vector3 Reading
        {
            set
            {
                float MAX = 500f;
                reading = value;
                float r = reading.magnitude / MAX;
                float g = 1f - reading.magnitude / MAX;
                Color = new Color(r, g, 0.34f);
                Vector3 a = new Vector3(0, 1f, 0);
                var b = reading.normalized;
                Quaternion q = Utils.RotateA2B(a, b);
                gameObject.transform.rotation = q;
                gameObject.transform.localScale = 
                    new Vector3(.3f, .075f * (float)Math.Log(reading.magnitude), .3f);
                //canvas.Text = Utils.DebugVector("reading", this.reading) +
                //    "\n" + Utils.DebugVector("position", this.position);
            }
            get { return reading; }
        }

        public Vector3 Position
        {
            set
            {
                position = value;
                gameObject.transform.position = position;
                //axis.Translate(position);
                //canvasObject.transform.position = position;
            }
            get { return position; }
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
                vectorMaterial.SetColor("_Color", color);
            }
        }

        void OnDisable()
        {
            cone.SetActive(false);
            arrowHeadBase.SetActive(false);
            stem.SetActive(false);
        }

        void OnEnable()
        {
            cone.SetActive(true);
            arrowHeadBase.SetActive(true);
            stem.SetActive(true);
        }

        public void OnDestroy()
        {
            Destroy(stem.gameObject);
            Destroy(arrowHeadBase.gameObject);
            Destroy(cone.gameObject);
        }
    }
}

