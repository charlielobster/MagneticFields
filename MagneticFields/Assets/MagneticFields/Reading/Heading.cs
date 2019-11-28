using UnityEngine;
using MagneticFields.Geometry;
using static System.Math;

namespace MagneticFields.Reading
{
    public class Heading : MonoBehaviour
    {
        private float radians;
        private LineRenderer headingRenderer;
        private Circle circle;
        private Axis axis;

        public Heading()
        {
            headingRenderer = Utils.InitializeLineRenderer(new GameObject(), Color.white);
            headingRenderer.gameObject.transform.parent = this.gameObject.transform;

            circle = new GameObject().AddComponent<Circle>();
            circle.gameObject.transform.parent = this.gameObject.transform;

            axis = new GameObject().AddComponent<Axis>();
            axis.gameObject.transform.parent = this.gameObject.transform;

            this.gameObject.transform.localScale = new Vector3(.5f, .5f, .5f);
        }
      
        public float Degrees
        {
            set
            {
                Radians = (float)(value * PI / 180.0);
            }
            get
            {
                return (float)(Radians * 180.0 / PI);
            }
        }

        public float Radians
        {
            set
            {
                radians = value;
                headingRenderer.SetPosition(1, new Vector3((float)Sin(-radians), 0, (float)Cos(-radians)));
            }
            get
            {
                return radians;
            }
        }

        public void OnDestroy()
        {
            Destroy(headingRenderer.gameObject);
            Destroy(circle.gameObject);
            Destroy(axis.gameObject);
        }
    }
}
