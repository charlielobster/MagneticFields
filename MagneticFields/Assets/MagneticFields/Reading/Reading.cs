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
        private Boolean m_isValid;

        public Boolean isValid
        {
            get
            {
                return m_isValid;
            }
        }

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

        public static float interpolate(float val, float y0, float x0, float y1, float x1)
        {
            return (val - x0) * (y1 - y0) / (x1 - x0) + y0;
        }

        public static float blue(float grayscale)
        {
            if (grayscale < -0.33)
                return 1.0f;
            else if (grayscale < 0.33)
                return interpolate(grayscale, 1.0f, -0.33f, 0.0f, 0.33f);
            else
                return 0.0f;
        }

        public static float green(float grayscale)
        {
            if (grayscale < -1.0)
                return 0.0f; // unexpected grayscale value
            if (grayscale < -0.33)
                return interpolate(grayscale, 0.0f, -1.0f, 1.0f, -0.33f);
            else if (grayscale < 0.33)
                return 1.0f;
            else if (grayscale <= 1.0)
                return interpolate(grayscale, 1.0f, 0.33f, 0.0f, 1.0f);
            else
                return 1.0f; // unexpected grayscale value
        }

        public static float red(float grayscale)
        {
            if (grayscale < -0.33)
                return 0.0f;
            else if (grayscale < 0.33)
                return interpolate(grayscale, 0.0f, -0.33f, 1.0f, 0.33f);
            else
                return 1.0f;
        }

        public static Color GetColorForVector(Vector3 vector)
        {
            const float MAX = 500f;
            float r = vector.magnitude / MAX;
            float g = 1f - vector.magnitude / MAX;
            return new Color(r, g, 0.34f);

            /*
            const float MAX = 3000f;
            float mag = (2.0f * vector.magnitude / MAX) - 1.0f;
            float r = red(mag);
            float g = green(mag);
            float b = blue(mag);
            return new Color(r, g, b);
            */
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
                    m_isValid = true;
                    break;
                case DeviceOrientation.LandscapeRight:
                    m_rawVector = new Vector3(rawVector.y, -rawVector.x, -rawVector.z);
                    m_isValid = true;
                    break;
                case DeviceOrientation.PortraitUpsideDown:
                    m_rawVector = new Vector3(-rawVector.x, -rawVector.y, -rawVector.z);
                    m_isValid = true;
                    break;
                case DeviceOrientation.Portrait:
                    // portait mode, correct for -z values?         
                    m_rawVector = new Vector3(rawVector.x, rawVector.y, -rawVector.z);
                    m_isValid = true;
                    break;
                case DeviceOrientation.FaceUp:
                    // FaceUp appears to be off by a PI rotation about some axis
                case DeviceOrientation.FaceDown:
                case DeviceOrientation.Unknown:
                default:
                    // these have issues
                    m_rawVector = rawVector;
                    m_isValid = false;
                    break;
            }

            m_color = GetColorForVector(m_rawVector);
            m_rotation = rotation;
            m_position = position;
            m_root.transform.rotation = m_rotation;
            //m_root.transform.position = m_position;
            m_dateTime = dateTime;

            //Activate only if valid
            gameObject.SetActive(m_isValid);
        }

        public Reading()
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
                "Reading:\n{0}\n{1}\n{2}\n{3}\n{4}\nIsValid: {5}\nUpdated:{6}\n",
                DebugFloat("heading", heading),
                DebugVector("rawVector", rawVector),
                DebugQuaternion("rotation",rotation),
                DebugVector("position", position),
                orientation.ToString(),
                isValid,
                dateTime.ToLongTimeString());
        }
    }
}
