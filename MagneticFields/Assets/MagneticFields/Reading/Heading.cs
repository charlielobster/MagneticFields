using UnityEngine;
using MagneticFields.Geometry;
using static System.Math;

namespace MagneticFields.Reading
{
    public class Heading : MonoBehaviour
    {
        private float m_radians;
        private LineRenderer m_headingRenderer;
        private Circle m_circle;
        private Axis m_axis;

        public Heading()
        {
            m_headingRenderer = Utils.InitializeLineRenderer(new GameObject(), Color.white);
            m_headingRenderer.gameObject.transform.parent = gameObject.transform;

            m_circle = new GameObject().AddComponent<Circle>();
            m_circle.gameObject.transform.parent = gameObject.transform;

            m_axis = new GameObject().AddComponent<Axis>();
            m_axis.gameObject.transform.parent = gameObject.transform;

            gameObject.transform.localScale = new Vector3(.5f, .5f, .5f);
        }
      
        public float degrees
        {
            set
            {
                radians = (float)(value * PI / 180.0);
            }
            get
            {
                return (float)(radians * 180.0 / PI);
            }
        }

        public float widthMultiplier
        {
            set
            {
                m_headingRenderer.widthMultiplier = value;
                m_circle.widthMultiplier = value;
                m_axis.widthMultiplier = value;
            }
            get
            {
                return m_headingRenderer.widthMultiplier;
            }
        }

        public float radians
        {
            set
            {
                m_radians = value;
                m_headingRenderer.SetPosition(1, 
                    new Vector3((float)Sin(-m_radians), 0, (float)Cos(-m_radians)));
            }
            get
            {
                return m_radians;
            }
        }

        public void OnDestroy()
        {
            Destroy(m_headingRenderer.gameObject);
            Destroy(m_circle.gameObject);
            Destroy(m_axis.gameObject);
        }
    }
}
