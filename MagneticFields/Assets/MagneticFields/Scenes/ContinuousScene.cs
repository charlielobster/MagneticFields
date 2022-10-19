using System;
using UnityEngine;
using UnityEngine.UI;
using MagneticFields.Reading;
using KdTree;
using KdTree.Math;
using MagneticFields.UI;
using System.IO;
using UnityEngine.XR.ARFoundation;

namespace MagneticFields.Scenes
{
    public class ContinuousScene : TapBehavior
    {
        private float unitLength = 30f;         
        private KdTree<float, BoundingBox> kdTree;
        private bool reading;

        public override void OnTap()
        {
            reading = !reading;
            debug.text = (reading ? "Reading" : "Idle");
        }

        private GameObject continuousMenu
        {
            get => GameObject.Find("ContinuousPanel");
        }

        private Slider unitSlider
        {
            get => continuousMenu.transform.Find("FillPanel/UnitSlider")
                .transform.Find("BorderPanel/FillPanel/Slider").GetComponent<Slider>();
        }

        private Toggle headingsToggle
        {
            get => continuousMenu.transform.Find("FillPanel/HeadingsToggle").GetComponent<Toggle>();
        }

        private Toggle shapesToggle
        {
            get => continuousMenu.transform.Find("FillPanel/D3VectorsToggle").GetComponent<Toggle>();
        }

        private Toggle framesToggle
        {
            get => continuousMenu.transform.Find("FillPanel/FramesToggle").GetComponent<Toggle>();
        }

        private Toggle boundingBoxesToggle
        {
            get => continuousMenu.transform.Find("FillPanel/BoundingBoxesToggle").GetComponent<Toggle>();
        }

        private Button resetButton
        {
            get => continuousMenu.transform.Find("FillPanel/ResetButton").GetComponent<Button>();
        }

        void OnUnitSliderChanged()
        {
            unitLength = unitSlider.value * .001f;
            Reset();
        }

        void OnHeadingsToggleChanged()
        {
            foreach (var node in kdTree)
            {
                var b = node.Value;
                b.heading.gameObject.SetActive(headingsToggle.isOn);
            }
        }

        void OnShapesToggleChanged()
        {
            foreach (var node in kdTree)
            {
                var b = node.Value;
                b.shapeReading.SetActive(shapesToggle.isOn);
            }
        }

        void OnFramesToggleChanged()
        {
            foreach (var node in kdTree)
            {
                var b = node.Value;
                b.lineReading.showFrame = framesToggle.isOn;
            }
        }

        void OnBoundingBoxesChanged()
        {
            foreach (var node in kdTree)
            {
                var b = node.Value;
                b.enabled = boundingBoxesToggle.isOn;
            }
        }

        void OnResetButtonClicked()
        {
            Reset();
        }
     
        private void SetGridActive(bool active)
        {
            foreach (var i in kdTree)
            {
                i.Value.gameObject.SetActive(active);
            }
        }

        private void Reset()
        {
            float[] zeros = { 0f, 0f, 0f };
            while(kdTree.Count > 0)
            {
                var i = kdTree.GetNearestNeighbours(zeros, 1)[0];
                var parent = i.Value.gameObject;

                kdTree.RemoveAt(i.Point);

                i.Value.OnDestroy();
                Destroy(i.Value);
                Destroy(parent);
            }
        }

        private Button readButton
        {
            get
            {
                return GameObject.Find("ReadButton").GetComponent<Button>();
            }
        }

        void OnReadButtonClicked()
        {
            reading = !reading;
            if (reading)
            {
                readButton.GetComponentInChildren<Text>().text = "Stop";
            }
            else
            {
                readButton.GetComponentInChildren<Text>().text = "Read";
            }
        }

        void Awake()
        {
            reading = false;
            unitSlider.onValueChanged.AddListener(delegate { OnUnitSliderChanged(); });
            headingsToggle.onValueChanged.AddListener(delegate { OnHeadingsToggleChanged(); });
            shapesToggle.onValueChanged.AddListener(delegate { OnShapesToggleChanged(); });
            framesToggle.onValueChanged.AddListener(delegate { OnFramesToggleChanged(); });
            boundingBoxesToggle.onValueChanged.AddListener(delegate { OnBoundingBoxesChanged(); });
            resetButton.onClick.AddListener(OnResetButtonClicked);
            kdTree = new KdTree<float, BoundingBox>(3, new FloatMath());
            OnUnitSliderChanged();
        }

        public override void Update()
        {
            base.Update();

            if (reading)
            {
                if (Camera.current != null)
                {
                    var position = Camera.current.transform.position;

                    if (kdTree.Count == 0)
                    {
                        var nextPosition = GetNextBoxPosition(position);
                        AddBoundingBox(nextPosition);
                    }
                    else
                    {
                        float[] fPosition = { position.x, position.y, position.z };
                        var nearest = kdTree.GetNearestNeighbours(fPosition, 1)[0].Value;

                        if (!nearest.Surrounds(position))
                        {
                            var nextPosition = GetNextBoxPosition(position);
                            AddBoundingBox(nextPosition);
                        }
                    }
                }
            }
        }

        private void AddBoundingBox(Vector3 center)
        {
            float[] point = { center.x, center.y, center.z };
            var box = new GameObject().AddComponent<BoundingBox>();
            box.center = center;
            box.unitLength = unitLength;

            kdTree.Add(point, box);
            box.gameObject.SetActive(boundingBoxesToggle.isOn);

            box.heading = new GameObject().AddComponent<Heading>();
            box.heading.widthMultiplier = 0.01f * unitLength;
            box.heading.degrees = Input.compass.magneticHeading;
            box.heading.gameObject.transform.position = center;
            box.heading.gameObject.transform.localScale *= unitLength;
            box.heading.gameObject.SetActive(headingsToggle.isOn);

            box.lineReading = new GameObject().AddComponent<LineReading>();
            box.lineReading.Set(Input.compass, Input.deviceOrientation);
            box.lineReading.widthMultiplier = 0.01f * unitLength;
            box.lineReading.gameObject.transform.position = center;
            box.lineReading.gameObject.transform.localScale *= unitLength;
            box.lineReading.showFrame = framesToggle.isOn;

            box.shapeReading = new GameObject().AddComponent<ShapeReading>();
            box.shapeReading.Set(Input.compass, Input.deviceOrientation);
            box.shapeReading.gameObject.transform.position = center;
            box.shapeReading.gameObject.transform.localScale *= unitLength;
            box.shapeReading.SetActive(shapesToggle.isOn);

            box.color = box.lineReading.color;
        }

        private Vector3 GetNextBoxPosition(Vector3 position)
        {
            var nextPosition = position;
            for (var i = 0; i < 3; i++)
            {
                nextPosition[i] += .5f * unitLength;
            }
            nextPosition = nextPosition / unitLength;
            nextPosition = new Vector3(
                (float)Math.Floor(nextPosition.x),
                (float)Math.Floor(nextPosition.y),
                (float)Math.Floor(nextPosition.z));
            for (var i = 0; i < 3; i++)
            {
                nextPosition[i] *= unitLength;
            }
            return nextPosition;
        }        
    }
}

