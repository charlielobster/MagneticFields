using System.Collections.Generic;
using UnityEngine;

namespace MagneticFields.Geometry
{
    public class Axis : MonoBehaviour
    {
        private List<LineRenderer> m_axes;

        private struct Data
        {
            public Data(Vector3 v, Color c) { vector = v; color = c; }
            public Vector3 vector;
            public Color color;
        };

        // draw 3 unit-length lines for x (red), y (green), and z (blue) axes
        public Axis()
        {
            m_axes = new List<LineRenderer>();

            var l = new List<Data>()
            {
                new Data(new Vector3(1f, 0, 0), Color.red),
                new Data(new Vector3(0, 1f, 0), Color.green),
                new Data(new Vector3(0, 0, 1f), Color.blue)
            };

            foreach (var d in l) {
                var a = Utils.InitializeLineRenderer(new GameObject(), d.color);
                a.SetPosition(1, d.vector);
                a.gameObject.transform.parent = this.gameObject.transform;
                m_axes.Add(a);
            }
        }

        public float widthMultiplier
        {
            set
            {
                foreach (var a in m_axes)
                    a.widthMultiplier = value;
            }
            get
            {
                return m_axes[0].widthMultiplier;
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

