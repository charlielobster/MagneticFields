﻿using System.Collections.Generic;
using UnityEngine;

namespace MagneticFields.Geometry
{
    public class Axis : MonoBehaviour
    {
        private List<LineRenderer> m_axes;

        private struct Data
        {
            public Vector3 vector;
            public Color color;
        };

        // draw 3 unit-length lines for x (red), y (green), and z (blue) axes
        public Axis()
        {
            m_axes = new List<LineRenderer>();

            foreach (var d in new List<Data>() {
                    new Data { vector = new Vector3(1f, 0, 0), color = Color.red },
                    new Data { vector = new Vector3(0, 1f, 0), color = Color.green },
                    new Data { vector = new Vector3(0, 0, 1f), color = Color.blue }
                } ) {

                var a = Utils.InitializeLineRenderer(new GameObject(), d.color);
                a.SetPosition(1, d.vector);
                a.gameObject.transform.parent = this.gameObject.transform;
                m_axes.Add(a);
            }
        }

        public void OnDestroy()
        {
            foreach (var a in m_axes)
            {
                Destroy(a.gameObject);
            }
        }
    }
}

