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
            m_rawVectorRenderer = Utils.InitializeLineRenderer(m_root, Color.magenta);

            m_yRenderer = Utils.InitializeLineRenderer(new GameObject(), Color.magenta);
            m_yRenderer.gameObject.transform.parent = m_root.transform;

            m_xzRenderer = Utils.InitializeLineRenderer(new GameObject(), Color.magenta);
            m_xzRenderer.gameObject.transform.parent = m_root.transform;

            m_xzRenderer.gameObject.transform.localScale = new Vector3(.5f, .5f, .5f);
        }

        public new void Set(Vector3 rawVector, Quaternion rotation, Vector3 position, DateTime dateTime)
        {
            base.Set(rawVector, rotation, position, dateTime);
            var normalized = m_rawVector.normalized;
            m_rawVectorRenderer.SetPosition(1, normalized);

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


