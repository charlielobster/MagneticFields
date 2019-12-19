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

        public LineReading()
        {
            m_rawVectorRenderer = Utils.InitializeLineRenderer(root, color);

            m_yRenderer = Utils.InitializeLineRenderer(new GameObject(), Color.magenta);
            m_yRenderer.gameObject.transform.parent = root.transform;

            m_xzRenderer = Utils.InitializeLineRenderer(new GameObject(), Color.magenta);
            m_xzRenderer.gameObject.transform.parent = root.transform;

            m_xzRenderer.gameObject.transform.localScale = new Vector3(.5f, .5f, .5f);
        }

        public bool ShowFrame
        {
            set
            {
                m_xzRenderer.gameObject.SetActive(value);
                m_yRenderer.gameObject.SetActive(value);
            }
            get
            {
                return m_xzRenderer.gameObject.activeInHierarchy;
            }
        }

        public override void Set(Compass compass, Transform transform, DeviceOrientation orientation)
        {
            base.Set(compass, transform, orientation);

            var normalized = rawVector.normalized;
            m_rawVectorRenderer.SetPosition(1, normalized);
            m_rawVectorRenderer.material.SetColor("_Color", color);

            var xz = new Vector2(rawVector.x, rawVector.z);
            xz.Normalize();
            m_xzRenderer.SetPosition(1, new Vector3(xz.x, 0, xz.y));

            m_yRenderer.SetPosition(1, new Vector3(0, normalized.y, 0));
        }

        public float widthMultiplier
        {
            set
            {
                m_rawVectorRenderer.widthMultiplier = 10.0f * value;
                m_yRenderer.widthMultiplier = value;
                m_xzRenderer.widthMultiplier = value;
                m_rawVectorRenderer.widthMultiplier = value;
            }
            get
            {
                return m_rawVectorRenderer.widthMultiplier;
            }
        }

        public override void OnDestroy()
        {
            Destroy(m_rawVectorRenderer.gameObject);
            Destroy(m_yRenderer.gameObject);
            Destroy(m_xzRenderer.gameObject);
            base.OnDestroy();
        }
    }
}


