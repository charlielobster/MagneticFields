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
    public class SkinnyReading : MonoBehaviour
    {
        private Material vectorMaterial;
        private Vector3 reading;
        private Vector3 position;
        private Color color;

        public SkinnyReading()
        {
            vectorMaterial = new Material(Shader.Find("Diffuse"));
            vectorMaterial.SetColor("_Color", color);

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
                this.gameObject.transform.rotation = q;
                this.gameObject.transform.localScale =
                    new Vector3(.3f, .075f * (float)Math.Log(reading.magnitude), .3f);             
            }
            get { return reading; }
        }

        public Vector3 Position
        {
            set
            {
                position = value;
                this.gameObject.transform.position = position;
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
    }
}
