using System;
using UnityEngine;
using static MagneticFields.Geometry.Utils;

namespace MagneticFields.Reading
{
    public abstract class Reading : MonoBehaviour
    {
        public static Color GetColorForVector(Vector3 vector)
        {
            const float MAX = 500f;
            float r = vector.magnitude / MAX;
            float g = 1f - vector.magnitude / MAX;
            return new Color(r, g, 0.34f);
        }

        public virtual void Set(Vector3 rawVector, Quaternion rotation, Vector3 position, DateTime dateTime)
        {
            m_rawVector = rawVector;
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
            return String.Format("{0}\n{1}\n{2}\n{3}\n",
                DebugVector("rawVector", m_rawVector),
                DebugQuaternion("rotation", m_rotation),
                DebugVector("position", m_position),
                m_dateTime.ToShortTimeString());
        }

        protected GameObject m_root;
        protected Vector3 m_rawVector;
        protected Quaternion m_rotation;
        protected Vector3 m_position;
        protected Color m_color;
        protected DateTime m_dateTime;
    }
}
