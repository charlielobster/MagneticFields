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
        private List<LineRenderer> axes;
        public float unitLength = 0.5f;
        

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


        public float widthMultiplier
        {
            set
            {
                foreach (var a in axes)
                {
                    a.widthMultiplier = value;
                }
            }
            get
            {
                return axes[0].widthMultiplier;
            }
        }
        /*
        public Axis()
        {
            axes = new List<LineRenderer>();

            var xAxisObject = new GameObject();
            xAxisObject.transform.parent = this.gameObject.transform;
            axes.Add(xAxisObject.AddComponent<LineRenderer>());
            axes[0].material = new Material(Shader.Find("Sprites/Default"));
            axes[0].material.SetColor("_Color", Color.red);

            var yAxisObject = new GameObject();
            yAxisObject.transform.parent = this.gameObject.transform;
            axes.Add(yAxisObject.AddComponent<LineRenderer>());
            axes[1].material = new Material(Shader.Find("Sprites/Default"));
            axes[1].material.SetColor("_Color", Color.green);

            var zAxisObject = new GameObject();
            zAxisObject.transform.parent = this.gameObject.transform;
            axes.Add(zAxisObject.AddComponent<LineRenderer>());
            axes[2].material = new Material(Shader.Find("Sprites/Default"));
            axes[2].material.SetColor("_Color", Color.blue);

            foreach (var a in axes)
            {
                a.widthMultiplier = 0.02f;
                a.positionCount = 2;
            }
        }
        */
        public void OnDisable()
        {
            foreach (var a in axes)
            {
                a.enabled = false;
            }
        }

        public void OnEnable()
        {
            foreach (var a in axes)
            {
                a.enabled = true;
            }

        }

        public void Translate(Vector3 t)
        {
            foreach (var a in axes)
            {
                a.SetPosition(0, t);
            }
            axes[0].SetPosition(1, new Vector3(unitLength, 0, 0) + t);
            axes[1].SetPosition(1, new Vector3(0, unitLength, 0) + t);
            axes[2].SetPosition(1, new Vector3(0, 0, unitLength) + t);
        }
        
    }
}
