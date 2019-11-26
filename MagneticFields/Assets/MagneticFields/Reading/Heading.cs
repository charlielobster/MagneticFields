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
    public class Heading : MonoBehaviour
    {
        public static readonly int VERTEX_COUNT = 32;

        private Material vectorMaterial;
        private Vector3 forward;
        private float angle;

        private LineRenderer headingRenderer;
        private LineRenderer circleRenderer;

        private LineRenderer xRenderer;
        private LineRenderer yRenderer;
        private LineRenderer zRenderer;

        public Heading()
        {
            headingRenderer = Utils.InitializeLineRenderer(new GameObject(), Color.white);
            headingRenderer.SetPosition(1, new Vector3(0, 0, 1f));
            headingRenderer.gameObject.transform.parent = this.gameObject.transform;

            circleRenderer = new GameObject().AddComponent<LineRenderer>();
            circleRenderer.gameObject.transform.parent = this.gameObject.transform;
            circleRenderer.material = new Material(Shader.Find("Sprites/Default"));
            circleRenderer.material.SetColor("_Color", Color.yellow);
            circleRenderer.widthMultiplier = 0.025f;
            circleRenderer.loop = true;
            circleRenderer.useWorldSpace = false;
            circleRenderer.positionCount = VERTEX_COUNT;
            double theta = 0;
            for (int i = 0; i < VERTEX_COUNT; i++)
            {
                theta = i * (2.0 * Math.PI) / VERTEX_COUNT;
                circleRenderer.SetPosition(i, new Vector3((float)Math.Cos(theta), 0, (float)Math.Sin(theta)));
            }

            xRenderer = Utils.InitializeLineRenderer(new GameObject(), Color.red);
            xRenderer.SetPosition(1, new Vector3(1f, 0, 0));
            xRenderer.gameObject.transform.parent = this.gameObject.transform;
            yRenderer = Utils.InitializeLineRenderer(new GameObject(), Color.green);
            yRenderer.SetPosition(1, new Vector3(0, 1f, 0));
            yRenderer.gameObject.transform.parent = this.gameObject.transform;
            zRenderer = Utils.InitializeLineRenderer(new GameObject(), Color.blue);
            zRenderer.SetPosition(1, new Vector3(0, 0, 1f));
            zRenderer.gameObject.transform.parent = this.gameObject.transform;

            this.gameObject.transform.localScale = new Vector3(.5f, .5f, .5f);
        }
      
        public float Angle
        {
            set
            {
                headingRenderer.gameObject.transform.Rotate(new Vector3(0, 1f, 0), angle);
                angle = value;
                headingRenderer.gameObject.transform.Rotate(new Vector3(0, 1f, 0), -angle);
            }
        }

        public void OnDestroy()
        {
            Destroy(headingRenderer);
            Destroy(circleRenderer);
            Destroy(xRenderer);
            Destroy(yRenderer);
            Destroy(zRenderer);
        }
    }
}
