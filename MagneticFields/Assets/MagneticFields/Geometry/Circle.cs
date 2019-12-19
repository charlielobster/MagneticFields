using UnityEngine;
using static System.Math;

namespace MagneticFields.Geometry
{
    public class Circle : MonoBehaviour
    {
        public static readonly int VERTEX_COUNT = 32;
        private LineRenderer m_renderer;

        // draw a unit-length yellow circle in the xz-plane
        public Circle()
        {
            m_renderer = gameObject.AddComponent<LineRenderer>();
            m_renderer.gameObject.transform.parent = this.gameObject.transform;
            m_renderer.material = new Material(Shader.Find("Sprites/Default"));
            m_renderer.material.SetColor("_Color", Color.yellow);
            m_renderer.widthMultiplier = 0.025f;
            m_renderer.loop = true;
            m_renderer.useWorldSpace = false;
            m_renderer.positionCount = VERTEX_COUNT;

            double theta = 0;
            for (int i = 0; i < VERTEX_COUNT; i++)
            {
                theta = i * (2.0 * PI) / VERTEX_COUNT;
                m_renderer.SetPosition(i, new Vector3((float)Cos(theta), 0, (float)Sin(theta)));
            }
        }

        public float widthMultiplier
        {
            set
            {
                m_renderer.widthMultiplier = value;
            }
            get
            {
                return m_renderer.widthMultiplier;
            }
        }
    }
}
