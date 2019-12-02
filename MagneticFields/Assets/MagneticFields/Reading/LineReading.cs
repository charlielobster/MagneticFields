using UnityEngine;
using MagneticFields.Geometry;
using System;

namespace MagneticFields.Reading
{
    public class LineReading : Reading
    {
        private LineRenderer m_rawVectorRenderer;
        private LineRenderer m_yRenderer;
        private LineRenderer m_xzRenderer;

        public new void Start()
        {
            base.Start();
            m_rawVectorRenderer = Utils.InitializeLineRenderer(m_root, m_color);

            m_yRenderer = Utils.InitializeLineRenderer(new GameObject(), Color.magenta);
            m_yRenderer.gameObject.transform.parent = m_root.transform;

            m_xzRenderer = Utils.InitializeLineRenderer(new GameObject(), Color.magenta);
            m_xzRenderer.gameObject.transform.parent = m_root.transform;

            m_xzRenderer.gameObject.transform.localScale = new Vector3(.5f, .5f, .5f);
        }

        protected override void Set(float heading, Vector3 rawVector, Quaternion rotation, Vector3 position, DeviceOrientation orientation, DateTime dateTime)
        {
            base.Set(heading, rawVector, rotation, position, orientation, dateTime);
            var normalized = m_rawVector.normalized;
            m_rawVectorRenderer.SetPosition(1, normalized);
            m_rawVectorRenderer.material.SetColor("_Color", m_color);

            var xz = new Vector2(m_rawVector.x, m_rawVector.z);
            xz.Normalize();
            m_xzRenderer.SetPosition(1, new Vector3(xz.x, 0, xz.y));

            m_yRenderer.SetPosition(1, new Vector3(0, normalized.y, 0));
        }
                
        public new void OnDestroy()
        {
            Destroy(m_rawVectorRenderer.gameObject);
            Destroy(m_yRenderer.gameObject);
            Destroy(m_xzRenderer.gameObject);
            base.OnDestroy();
        }
    }
}


