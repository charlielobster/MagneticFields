using System;
using UnityEngine;

namespace MagneticFields.Geometry
{
    public static class Utils
    {
        public static String DebugFloat(string name, float f)
        {
            return String.Format("{0}: {1,10:00.00}", name, f);
        }
        public static String DebugVector(string name, Vector3 vector)
        {
            return (name + ".x:" + String.Format("{0,10:00.00}", vector.x) +
                "\n" + name + ".y:" + String.Format("{0,10:00.00}", vector.y) +
                "\n" + name + ".z:" + String.Format("{0,10:00.00}", vector.z) +
                "\nmagnitude: " + String.Format("{0,10:00.00}", vector.magnitude) + "\n");
        }

        public static String DebugTransform(string name, Transform transform)
        {
            return String.Format("transform {0}\n{1}\n{2}\n{3}\n", name,
                DebugQuaternion("rotation", transform.rotation), 
                DebugVector("position", transform.position),
                DebugVector("scale", transform.localScale));
        }

        public static String DebugQuaternion(string name, Quaternion quaternion)
        {
            return (name + ".x:" + String.Format("{0,10:00.00}", quaternion.x) +
              "\n" + name + ".y:" + String.Format("{0,10:00.00}", quaternion.y) +
              "\n" + name + ".z:" + String.Format("{0,10:00.00}", quaternion.z) +
              "\n" + name + ".w:" + String.Format("{0,10:00.00}", quaternion.w) + "\n");
        }

        public static Quaternion RotateA2B(Vector3 a, Vector3 b)
        {
            Vector3 c = Vector3.Cross(a, b);
            float w = (float)Math.Sqrt((a.magnitude * a.magnitude) * (b.magnitude * b.magnitude)) + Vector3.Dot(a, b);
            return new Quaternion(c.x, c.y, c.z, w);
        }

        public static LineRenderer InitializeLineRenderer(GameObject gameObject, Color color, float widthMultiplier = 0.025f)
        {
            var renderer = gameObject.AddComponent<LineRenderer>();
            renderer.material = new Material(Shader.Find("Sprites/Default"));
            renderer.material.SetColor("_Color", color);
            renderer.widthMultiplier = widthMultiplier;
            renderer.useWorldSpace = false;
            renderer.positionCount = 2;
            renderer.SetPosition(0, Vector3.zero);
            return renderer;
        }
    }
}
