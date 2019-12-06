using System;
using UnityEngine;
using static MagneticFields.Geometry.Utils;

namespace MagneticFields.Reading
{
    public abstract class Reading : MonoBehaviour
    {
        private GameObject m_root;
        private float m_heading;
        private Vector3 m_rawVector;
        private Quaternion m_rotation;
        private Vector3 m_position;
        private DeviceOrientation m_orientation;
        private Color m_color;
        private DateTime m_dateTime;

        public float heading
        {
            get
            {
                return m_heading;
            }
        }

        public Vector3 rawVector
        {
            get
            {
                return m_rawVector;
            }
        }

        public Quaternion rotation
        {
            get
            {
                return m_rotation;
            }
        }

        public Vector3 position
        {
            get
            {
                return m_position;
            }
        }

        public DeviceOrientation orientation
        {
            get
            {
                return m_orientation;
            }
        }

        public Color color
        {
            get
            {
                return m_color;
            }
        }

        public DateTime dateTime
        {
            get
            {
                return m_dateTime;
            }
        }

        protected GameObject root
        {
            get
            {
                return m_root;
            }
        }

        public static Color GetColorForVector(Vector3 vector)
        {
            const float MAX = 500f;
            float r = vector.magnitude / MAX;
            float g = 1f - vector.magnitude / MAX;
            return new Color(r, g, 0.34f);
        }
        
        public virtual void Set(Compass compass, Transform transform, DeviceOrientation orientation)
        {
            Set(compass.magneticHeading, compass.rawVector, transform.rotation, 
                transform.position, orientation, DateTime.UtcNow);
        }

        private void Set(float heading, Vector3 rawVector, Quaternion rotation, 
            Vector3 position, DeviceOrientation orientation, DateTime dateTime)
        {
            m_heading = heading;

            // the rawVector's coordinates are affected by deviceOrientation
            m_orientation = orientation;
            switch (m_orientation)
            {
                case DeviceOrientation.LandscapeLeft:
                    m_rawVector = new Vector3(-rawVector.y, rawVector.x, -rawVector.z);
                    break;
                case DeviceOrientation.LandscapeRight:
                    m_rawVector = new Vector3(rawVector.y, -rawVector.x, -rawVector.z);
                    break;
                case DeviceOrientation.FaceUp:
                    m_rawVector = new Vector3(rawVector.x, rawVector.z, rawVector.y);
                    break;
                case DeviceOrientation.PortraitUpsideDown:
                    m_rawVector = new Vector3(-rawVector.x, -rawVector.y, -rawVector.z);
                    break;
                case DeviceOrientation.Portrait:
                    // portait mode, correct for -z values?         
                    m_rawVector = new Vector3(rawVector.x, rawVector.y, -rawVector.z);
                    break;
                case DeviceOrientation.FaceDown:
                case DeviceOrientation.Unknown:
                default:
                    break;
            }

            m_color = GetColorForVector(m_rawVector);
            m_rotation = rotation;
            m_position = position;
            m_root.transform.rotation = m_rotation;
            // m_root.transform.position = m_position;
            m_dateTime = dateTime;
        }

        public virtual void Start()
        {
            m_root = new GameObject();
            m_root.transform.parent = gameObject.transform;
        }

        public virtual void OnDestroy()
        {
            Destroy(m_root);
        }

        public override String ToString()
        {
            return String.Format(
                "Reading:\n{0}\n{1}\n{2}\n{3}\n{4}\nUpdated:{5}\n",
                DebugFloat("heading", heading),
                DebugVector("rawVector", rawVector),
                DebugQuaternion("rotation",rotation),
                DebugVector("position", position),
                orientation.ToString(),
                dateTime.ToLongTimeString());
        }
    }
}
