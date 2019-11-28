using UnityEngine;
using static System.Math;

namespace MagneticFields.Geometry
{
    public class Circle : MonoBehaviour
    {
        public static readonly int VERTEX_COUNT = 32;

        // draw a unit-length yellow circle in the xz-plane
        public Circle()
        {
            var renderer = gameObject.AddComponent<LineRenderer>();
            renderer.gameObject.transform.parent = this.gameObject.transform;
            renderer.material = new Material(Shader.Find("Sprites/Default"));
            renderer.material.SetColor("_Color", Color.yellow);
            renderer.widthMultiplier = 0.025f;
            renderer.loop = true;
            renderer.useWorldSpace = false;
            renderer.positionCount = VERTEX_COUNT;

            double theta = 0;
            for (int i = 0; i < VERTEX_COUNT; i++)
            {
                theta = i * (2.0 * PI) / VERTEX_COUNT;
                renderer.SetPosition(i, new Vector3((float)Cos(theta), 0, (float)Sin(theta)));
            }
        }
    }
}
