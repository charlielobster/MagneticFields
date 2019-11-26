using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagneticFields.Geometry
{
    public static class Utils
    {
        public static String DebugVector(string name, Vector3 v)
        {
            return (name + ".x:" + String.Format("{0,10:00.00}", v.x) +
                "\n" + name + ".y:" + String.Format("{0,10:00.00}", v.y) +
                "\n" + name + ".z:" + String.Format("{0,10:00.00}", v.z) +
                "\nmagnitude: " + String.Format("{0,10:00.00}", v.magnitude) + "\n");
        }

        public static Quaternion RotateA2B(Vector3 a, Vector3 b)
        {
            Vector3 c = Vector3.Cross(a, b);
            float w = (float)Math.Sqrt((a.magnitude * a.magnitude) * (b.magnitude * b.magnitude)) + Vector3.Dot(a, b);
            return new Quaternion(c.x, c.y, c.z, w);
        }

        public static LineRenderer InitializeLineRenderer(GameObject gameObject, Color color)
        {
            var renderer = gameObject.AddComponent<LineRenderer>();
            renderer.material = new Material(Shader.Find("Sprites/Default"));
            renderer.material.SetColor("_Color", color);
            renderer.widthMultiplier = 0.025f;
            renderer.useWorldSpace = false;
            renderer.positionCount = 2;
            renderer.SetPosition(0, Vector3.zero);
            return renderer;
        }
    }
}
