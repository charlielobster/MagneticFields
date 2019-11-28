using UnityEngine;
using MagneticFields.Geometry;

namespace MagneticFields.Reading
{
    public class LineReading : MonoBehaviour
    {
        private Vector3 m_reading;
        private LineRenderer readingRenderer;
        private LineRenderer yRenderer;
        private LineRenderer xzRenderer;

        public LineReading()
        {
            readingRenderer = Utils.InitializeLineRenderer(this.gameObject, Color.magenta);

            var yObject = new GameObject();
            yObject.transform.parent = this.gameObject.transform;
            yRenderer = Utils.InitializeLineRenderer(yObject, Color.magenta);
            var xzObject = new GameObject();
            xzObject.transform.parent = this.gameObject.transform;
            xzRenderer = Utils.InitializeLineRenderer(xzObject, Color.magenta);
        }

        public Vector3 reading
        {
            set
            {
                m_reading = value;
                m_reading.Normalize();
                readingRenderer.SetPosition(1, m_reading);
                yRenderer.SetPosition(1, new Vector3(0, m_reading.y, 0));
                xzRenderer.SetPosition(1, new Vector3(m_reading.x, 0, m_reading.z));
            }
            get { return readingRenderer.GetPosition(1); }
        }
                
        public void OnDestroy()
        {
            Destroy(readingRenderer.gameObject);
            Destroy(yRenderer.gameObject);
            Destroy(xzRenderer.gameObject);
        }
    }
}
