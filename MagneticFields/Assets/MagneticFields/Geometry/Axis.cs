using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagneticFields.Geometry
{
    public class Axis : MonoBehaviour
    {
        private List<LineRenderer> axes;
        public float unitLength = 0.5f;

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

