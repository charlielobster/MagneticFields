using UnityEngine;
using MagneticFields.Geometry;

namespace MagneticFields.Reading
{
    public class LineReading : MonoBehaviour
    {
        private Vector3 reading;
        private Material vectorMaterial;
        private LineRenderer readingRenderer;
        private GameObject yObject;
        private LineRenderer yRenderer;
        private GameObject xzObject;
        private LineRenderer xzRenderer;

        public LineReading()
        {
            readingRenderer = Utils.InitializeLineRenderer(this.gameObject, Color.magenta);

            yObject = new GameObject();
            yObject.transform.parent = this.gameObject.transform;
            yRenderer = Utils.InitializeLineRenderer(yObject, Color.magenta);
            xzObject = new GameObject();
            xzObject.transform.parent = this.gameObject.transform;
            xzRenderer = Utils.InitializeLineRenderer(xzObject, Color.magenta);
        }

        public Vector3 Reading
        {
            set
            {
                reading = value;
                reading.Normalize();
                readingRenderer.SetPosition(1, reading);
                yRenderer.SetPosition(1, new Vector3(0, reading.y, 0));
                xzRenderer.SetPosition(1, new Vector3(reading.x, 0, reading.z));
            }
            get { return readingRenderer.GetPosition(1); }
        }
                
        public void OnDestroy()
        {
            Destroy(readingRenderer);
            Destroy(yRenderer);
            Destroy(yObject);
            Destroy(xzRenderer);
            Destroy(xzObject);
        }
    }
}
