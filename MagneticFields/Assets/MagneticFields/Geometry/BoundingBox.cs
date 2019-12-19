using UnityEngine;
using MagneticFields.Reading;

namespace MagneticFields.Geometry
{ 

    public class BoundingBox : MonoBehaviour
    {
        private Vector3 m_center;
        private float m_unitLength;
        private Color m_color;
        private LineRenderer m_lineRenderer;
        private GameObject rendererObject;

        public LineReading lineReading;
        public ShapeReading shapeReading;
        public Heading heading;

        public BoundingBox()
        {
            m_unitLength = 1f;
            m_color = Color.white;
            rendererObject = new GameObject();
            m_lineRenderer = rendererObject.AddComponent<LineRenderer>();
            m_lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            m_lineRenderer.material.SetColor("_Color", color);
            m_lineRenderer.widthMultiplier = 0.0025f;
            m_lineRenderer.useWorldSpace = false;
            m_lineRenderer.positionCount = 16;
            m_lineRenderer.SetPosition(0, new Vector3(-.5f, -.5f, -.5f));
            m_lineRenderer.SetPosition(1, new Vector3(.5f, -.5f, -.5f));
            m_lineRenderer.SetPosition(2, new Vector3(.5f, -.5f, .5f));
            m_lineRenderer.SetPosition(3, new Vector3(-.5f, -.5f, .5f));
            m_lineRenderer.SetPosition(4, new Vector3(-.5f, -.5f, -.5f));
            m_lineRenderer.SetPosition(5, new Vector3(-.5f, .5f, -.5f));
            m_lineRenderer.SetPosition(6, new Vector3(.5f, .5f, -.5f));
            m_lineRenderer.SetPosition(7, new Vector3(.5f, .5f, .5f));
            m_lineRenderer.SetPosition(8, new Vector3(-.5f, .5f, .5f));
            m_lineRenderer.SetPosition(9, new Vector3(-.5f, .5f, -.5f));
            m_lineRenderer.SetPosition(10, new Vector3(.5f, .5f, -.5f));
            m_lineRenderer.SetPosition(11, new Vector3(.5f, -.5f, -.5f));
            m_lineRenderer.SetPosition(12, new Vector3(.5f, -.5f, .5f));
            m_lineRenderer.SetPosition(13, new Vector3(.5f, .5f, .5f));
            m_lineRenderer.SetPosition(14, new Vector3(-.5f, .5f, .5f));
            m_lineRenderer.SetPosition(15, new Vector3(-.5f, -.5f, .5f));
        }

        public Vector3 center
        {
            get
            {
                return m_center;
            }
            set
            {
                m_center = value;
                rendererObject.transform.position = value;
            }
        }

        public float unitLength
        {
            get
            {
                return m_unitLength;
            }
            set
            {
                rendererObject.transform.localScale = Vector3.one * value;
                m_lineRenderer.widthMultiplier = 0.005f * value;
                m_unitLength = value;
            }
        }
      
        public float minX
        {
            get
            {
                return m_center.x - .5f * m_unitLength;
            }
        }

        public float maxX
        {
            get
            {
                return m_center.x + .5f * m_unitLength;
            }
        }

        public float minY
        {
            get
            {
                return m_center.y - .5f * m_unitLength;
            }
        }

        public float maxY
        {
            get
            {
                return m_center.y + .5f * m_unitLength;
            }
        }

        public float minZ
        {
            get
            {
                return m_center.z - .5f * m_unitLength;
            }
        }

        public float maxZ
        {
            get
            {
                return m_center.z + .5f * m_unitLength;
            }
        }

        public Color color
        {
            get
            {
                return m_color;
            }
            set
            {
                m_color = value;
                m_lineRenderer.material.SetColor("_Color", m_color);
            }
        }
        
        public void OnDestroy()
        {
            Destroy(m_lineRenderer.gameObject);
        }

        public void OnEnable()
        {
            rendererObject.SetActive(true);
        }

        public void OnDisable()
        {
            rendererObject.SetActive(false);
        }

        public bool Surrounds(Vector3 pt)
        {
            return (minX <= pt.x && pt.x <= maxX && 
                minY <= pt.y && pt.y <= maxY && 
                minZ <= pt.z && pt.z <= maxZ);
        }
    }
}