using System;
using UnityEngine;
using static MagneticFields.Geometry.Utils;

namespace MagneticFields.Reading
{
    public abstract class Reading : MonoBehaviour
    {
        protected GameObject m_root;
        protected float m_heading;
        protected Vector3 m_rawVector;
        protected Quaternion m_rotation;
        protected Vector3 m_position;
        protected DeviceOrientation m_orientation;
        protected Color m_color;
        protected DateTime m_dateTime;

        public static Color GetColorForVector(Vector3 vector)
        {
            const float MAX = 500f;
            float r = vector.magnitude / MAX;
            float g = 1f - vector.magnitude / MAX;
            return new Color(r, g, 0.34f);
        }
        
        public void Set(Compass compass, Transform transform, DeviceOrientation orientation)
        {
            Set(compass.magneticHeading, compass.rawVector, transform.rotation, 
                transform.position, orientation, DateTime.UtcNow);
        }

        protected virtual void Set(float heading, Vector3 rawVector, Quaternion rotation, 
            Vector3 position, DeviceOrientation orientation, DateTime dateTime)
        {
            m_heading = heading;
            m_rawVector = rawVector;

            rawVector = Input.compass.rawVector;

            // the rawVector's coordinates are affected by deviceOrientation
            m_orientation = orientation;
            switch (m_orientation)
            {
                case DeviceOrientation.Portrait:
                case DeviceOrientation.PortraitUpsideDown:
                case DeviceOrientation.LandscapeLeft:
                case DeviceOrientation.LandscapeRight:
                case DeviceOrientation.FaceDown:
                case DeviceOrientation.FaceUp:
                case DeviceOrientation.Unknown:
                default:
                    // upright (normal) portait mode, must correct for -z values           
                    m_rawVector.z = -m_rawVector.z;
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
                "Reading:\n{0}\n{1}\n{2}\n{3}\n{4}\nTimestamp: {5}\n",
                DebugFloat("heading", m_heading),
                DebugVector("rawVector", m_rawVector),
                DebugQuaternion("rotation", m_rotation),
                DebugVector("position", m_position),
                m_orientation.ToString(),
                m_dateTime.ToLongTimeString());
        }
    }
}
