using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using MagneticFields.Geometry;
using MagneticFields.UI;

namespace MagneticFields.Reading
{
    public class CameraReading : MonoBehaviour
    {
        //private Axis axis;
        private Material vectorMaterial = default(Material);
        private Vector3 reading;
        private Vector3 position;
        private Color color;
        //private WorldCanvas canvas;
        //private GameObject canvasObject;

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
                //canvas.Text = Utils.DebugVector("reading", this.reading) +
                //    "\n" + Utils.DebugVector("position", this.position);
            }
            get { return reading; }
        }

        //public Vector3 Position
        //{
        //    set
        //    {
        //        position = value;
        //        this.gameObject.transform.position = position;
        //        //canvasObject.transform.position = position;
        //        //                canvasObject.transform.rotation = Camera.current.transform.rotation;
        //    }
        //    get { return position; }
        //}

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

            //var axisObject = new GameObject();
            //axis = axisObject.AddComponent<Axis>();
            //axisObject.gameObject.transform.parent = this.gameObject.transform;

            //canvasObject = new GameObject();
            //canvas = canvasObject.AddComponent<WorldCanvas>();
            //canvas.Text = Utils.DebugVector("reading", this.reading);

            Reading = new Vector3(0, 1f, 0);

            this.gameObject.transform.localScale = new Vector3(.5f, .5f, .5f);
        }

        //private void PlaceVector()
        //{
        //    var a = new Vector3(0, 1f, 0);
        //    var b = Reading.normalized;
        //    Quaternion q = Utils.RotateA2B(a, b);
        //    this.gameObject.transform.rotation = q;
        //    this.gameObject.transform.localScale = new Vector3(.4f, .1f * (float)Math.Log(Magnitude), .4f);
        //}

    }
}

