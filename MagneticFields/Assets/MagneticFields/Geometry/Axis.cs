using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagneticFields.Geometry
{
    public class Axis : MonoBehaviour
    {
        private List<LineRenderer> axes;

        public Axis()
        {
            axes = new List<LineRenderer>();

            var xAxisObject = new GameObject();
            xAxisObject.transform.parent = this.gameObject.transform;
            axes.Add(Utils.InitializeLineRenderer(xAxisObject, Color.red));
            axes[0].SetPosition(1, new Vector3(1f, 0, 0));

            var yAxisObject = new GameObject();
            yAxisObject.transform.parent = this.gameObject.transform;
            axes.Add(Utils.InitializeLineRenderer(yAxisObject, Color.green));
            axes[1].SetPosition(1, new Vector3(0, 1f, 0));

            var zAxisObject = new GameObject();
            zAxisObject.transform.parent = this.gameObject.transform;
            axes.Add(Utils.InitializeLineRenderer(zAxisObject, Color.blue));
            axes[2].SetPosition(2, new Vector3(0, 0, 1f));
        }

        public void OnDestroy()
        {
            foreach (var a in axes)
            {
                Destroy(a);
            }
        }
    }
}

