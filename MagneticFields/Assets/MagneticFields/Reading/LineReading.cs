﻿using UnityEngine;
using MagneticFields.Geometry;

namespace MagneticFields.Reading
{
    public class LineReading : MonoBehaviour
    {
        private Vector3 m_reading;
        private Vector2 m_xz;
        private LineRenderer m_readingRenderer;
        private LineRenderer m_yRenderer;
        private LineRenderer m_xzRenderer;

        public LineReading()
        {
            m_readingRenderer = Utils.InitializeLineRenderer(this.gameObject, Color.magenta);

            m_yRenderer = Utils.InitializeLineRenderer(new GameObject(), Color.magenta);
            m_yRenderer.gameObject.transform.parent = this.gameObject.transform;

            m_xzRenderer = Utils.InitializeLineRenderer(new GameObject(), Color.magenta);
            m_xzRenderer.gameObject.transform.parent = this.gameObject.transform;

            m_xzRenderer.gameObject.transform.localScale = new Vector3(.5f, .5f, .5f);
        }

        public Vector3 rawVector
        {
            set
            {
                m_reading = value;
                m_reading.Normalize();
                m_readingRenderer.SetPosition(1, m_reading);

                m_xz = new Vector2(m_reading.x, m_reading.z);
                m_xz.Normalize();
                m_xzRenderer.SetPosition(1, new Vector3(m_xz.x, 0, m_xz.y));

                m_yRenderer.SetPosition(1, new Vector3(0, m_reading.y, 0));
            }
            get
            {
                return m_readingRenderer.GetPosition(1);
            }
        }
                
        public void OnDestroy()
        {
            Destroy(m_readingRenderer.gameObject);
            Destroy(m_yRenderer.gameObject);
            Destroy(m_xzRenderer.gameObject);
        }
    }
}
