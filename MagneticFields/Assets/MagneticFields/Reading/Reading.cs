using System;
using UnityEngine;
using static MagneticFields.Geometry.Utils;

namespace MagneticFields.Reading
{
    public abstract class Reading : MonoBehaviour
    {
        private GameObject m_root;
        private Vector3 m_rawVector;
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

        public Vector3 rawVector
        {
            get
            {
                return m_rawVector;
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

            /*
            const float MAX = 3000f;
            float mag = (2.0f * vector.magnitude / MAX) - 1.0f;
            float r = red(mag);
            float g = green(mag);
            float b = blue(mag);
            return new Color(r, g, b);
            */
        }

        public virtual void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public virtual void Set(Compass compass, DeviceOrientation orientation)
        {
            Set(compass.rawVector, compass.magneticHeading, orientation, DateTime.UtcNow);
        }

        private void Set(Vector3 vector, float heading, DeviceOrientation orientation, DateTime dateTime)
        {
            // the rawVector's coordinates are affected by deviceOrientation
            switch (orientation)
            {
                case DeviceOrientation.LandscapeLeft:
                    m_rawVector = new Vector3(-vector.y, vector.x, -vector.z);
                    m_isValid = true;
                    break;

                case DeviceOrientation.LandscapeRight:
                    m_rawVector = new Vector3(vector.y, -vector.x, -vector.z);
                    m_isValid = true;
                    break;

                case DeviceOrientation.PortraitUpsideDown:
                    m_rawVector = new Vector3(-vector.x, -vector.y, -vector.z);
                    m_isValid = true;
                    break;

                case DeviceOrientation.Portrait:
                    m_rawVector = new Vector3(vector.x, vector.y, vector.z);
                    m_isValid = true;
                    break;

                case DeviceOrientation.FaceUp:
                case DeviceOrientation.FaceDown:
                    m_rawVector = new Vector3(vector.x, vector.z, vector.y);
                    m_isValid = true;
                    break;

                case DeviceOrientation.Unknown:
                default:
                    // these have issues
                    m_rawVector = vector;
                    m_isValid = false;
                    break;
            }

            var hq = quadrant(heading);
            switch (hq)
            {
                case 0:
                    m_rawVector.x = -1.0f * Math.Abs(m_rawVector.x);
                    m_rawVector.z = Math.Abs(m_rawVector.z);
                    break;
                case 1:
                    m_rawVector.x = -1.0f * Math.Abs(m_rawVector.x);
                    m_rawVector.z = -1.0f * Math.Abs(m_rawVector.z);
                    break;
                case 2:
                    m_rawVector.x = Math.Abs(m_rawVector.x);
                    m_rawVector.z = -1.0f * Math.Abs(m_rawVector.z);
                    break;
                case 3:
                    m_rawVector.x = Math.Abs(m_rawVector.x);
                    m_rawVector.z = Math.Abs(m_rawVector.z);
                    break;
            }

            m_color = GetColorForVector(m_rawVector);
            m_dateTime = dateTime;
        }


        int quadrant(float heading)
        {
            return (int)heading / 90;
        }

        int quadrant(float x, float z)
        {
            if (z > 0)
                return x > 0 ? 3 : 0;
            else
                return x > 0 ? 2 : 1;
        }

        public virtual void Awake()
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
                "Reading:\n{0}\nIsValid: {1}\nUpdated:{2}\n",
                DebugVector("rawVector", rawVector),
                isValid,
                dateTime.ToLongTimeString());
        }
    }
}
