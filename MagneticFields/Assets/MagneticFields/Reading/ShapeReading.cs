using UnityEngine;
using MagneticFields.Geometry;
using System;
using static System.Math;

namespace MagneticFields.Reading
{
    public class ShapeReading : Reading
    {
        private Material m_vectorMaterial;
        private GameObject m_axis;
        private GameObject m_cone;
        private GameObject m_stem;
        private GameObject m_arrowHeadBase;

        protected override void Set(float heading, Vector3 rawVector, Quaternion rotation, 
            Vector3 position, DeviceOrientation orientation, DateTime dateTime)
        {
            base.Set(heading, rawVector, rotation, position, orientation, dateTime);

            m_vectorMaterial.SetColor("_Color", m_color);

            var xz_y = new Vector2(
                (float)(Sqrt(m_rawVector.x * m_rawVector.x + m_rawVector.z * m_rawVector.z)),
                m_rawVector.y);
            xz_y.Normalize();
            var phi = -(float)(Asin(xz_y.y) * 180.0 / PI);
            if (xz_y.y > 0) phi = -phi;

            var zx = new Vector2(m_rawVector.z, m_rawVector.x);
            zx.Normalize();
            var theta = (float)(Acos(zx.x) * 180.0 / PI);
            if (zx.y > 0) theta = -theta;

            var q = Quaternion.Euler(90f + phi, 0, 0);
            q = Quaternion.Euler(0, -theta, 0) * q;
            m_root.transform.rotation = m_rotation * q;
        }
        
        public new void Start()
        {
            base.Start();

            m_vectorMaterial = new Material(Shader.Find("Diffuse"));
            m_vectorMaterial.SetColor("_Color", m_color);

            m_stem = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            m_stem.GetComponent<Renderer>().material = m_vectorMaterial;
            m_stem.transform.localScale = new Vector3(.25f, 1f, .25f);
            m_stem.transform.parent = m_root.transform;

            m_arrowHeadBase = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            m_arrowHeadBase.GetComponent<Renderer>().material = m_vectorMaterial;
            m_arrowHeadBase.transform.localScale = new Vector3(.5f, .01f, .5f);
            m_arrowHeadBase.transform.position = new Vector3(0, 1f, 0);
            m_arrowHeadBase.transform.parent = m_root.transform;

            m_cone = Cone.CreateCone();
            m_cone.transform.rotation = Quaternion.Euler(90f, 0, 0);
            m_cone.transform.position = new Vector3(0, 1.5f, 0);
            m_cone.transform.localScale = new Vector3(.25f, .25f, .5f);
            m_cone.transform.parent = m_root.transform;
            m_cone.GetComponent<MeshRenderer>().material = m_vectorMaterial;

            gameObject.transform.localScale = new Vector3(.5f, .5f, .5f);
        }

        public new void OnDestroy()
        {
            Destroy(m_cone);
            Destroy(m_stem);
            Destroy(m_arrowHeadBase);
            base.OnDestroy();
        }
    }
}

