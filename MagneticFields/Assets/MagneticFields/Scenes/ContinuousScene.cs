using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MagneticFields.Geometry;
using MagneticFields.Reading;
using KdTree;
using KdTree.Math;

namespace MagneticFields.Scenes
{
    public class ContinuousScene : MonoBehaviour
    {
        public GameObject gridElement;

        private float unitLength = .0375f;   //1f / 25f;

        private List<LineReading> readings;
        private KdTree<float, BoundingBox> kdTree;
        private bool reading;
        private bool gridHidden;

        private Text debug
        {
            get
            {
                return GameObject.Find("Debug").GetComponent<Text>();
            }
        }
       
        private Slider unitSlider
        {
            get
            {
                return GameObject.Find("UnitSlider").GetComponent<Slider>();
            }
        }

        void OnUnitSliderChanged(Slider slider)
        {
            unitLength = (float)slider.value;
            Reset();
        }        

        private Button resetButton
        {
            get
            {
                return GameObject.Find("ResetButton").GetComponent<Button>();
            }
        }

        void OnResetButtonClicked()
        {
            Reset();
        }

        private Button hideGridButton
        {
            get
            {
                return GameObject.Find("HideGridButton").GetComponent<Button>();
            }
        }

        void OnHideGridButtonClicked()
        {
            gridHidden = !gridHidden;
            SetGridActive(!gridHidden);
            if (gridHidden)
            {
                hideGridButton.GetComponentInChildren<Text>().text = "Show Grid";
            }
            else
            {
                hideGridButton.GetComponentInChildren<Text>().text = "Hide Grid";
            }
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

            foreach (var r in readings)
            {
                var parent = r.gameObject;
                r.OnDestroy();
                Destroy(r);
                Destroy(parent);
            }
            readings.Clear();
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
            gridHidden = true;
            unitSlider.value = unitLength;
            debug.text = "Awaking Continuous Scene...";
            unitSlider.onValueChanged.AddListener(delegate { OnUnitSliderChanged(unitSlider); });
            resetButton.onClick.AddListener(OnResetButtonClicked);
            readButton.onClick.AddListener(OnReadButtonClicked);
            hideGridButton.onClick.AddListener(OnHideGridButtonClicked);
            kdTree = new KdTree<float, BoundingBox>(3, new FloatMath());
            readings = new List<LineReading>();
        }

        void Update()
        {
            //debug.text = "Update\n";
            if (reading)
            {
                if (Camera.current != null)
                {
                    var position = Camera.current.transform.position;
                    //debug.text += ("\n" + Utils.DebugVector("position", position) + "\n");

                    if (kdTree.Count == 0)
                    {
                        var nextPosition = GetNextBoxPosition(position);
                        AddBoundingBox(nextPosition);
                    }
                    else
                    {
                        float[] fPosition = { position.x, position.y, position.z };
                        var nearest = kdTree.GetNearestNeighbours(fPosition, 1)[0].Value;
                        //debug.text += ("\n" + Utils.DebugVector("nearest", nearest.center) + "\n");

                        if (!nearest.Surrounds(position))
                        {
                            var nextPosition = GetNextBoxPosition(position);
                            //debug.text += ("\nadding BoundingBox\n" + Utils.DebugVector("nextPosition", nextPosition) + "\n");
                            AddBoundingBox(nextPosition);
                        }
                        else
                        {
                           // debug.text += ("\nnearest surrounds position\n");
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

            box.gameObject.SetActive(!gridHidden);
            kdTree.Add(point, box);

            //box.heading = new GameObject().AddComponent<Heading>();
            //box.heading.widthMultiplier = 0.01f * unitLength;
            //box.heading.gameObject.transform.rotation = transform.rotation;
            //box.heading.degrees = Input.compass.magneticHeading;
            //box.heading.gameObject.transform.position = center;
            //box.heading.gameObject.transform.localScale *= unitLength;

            box.lineReading = new GameObject().AddComponent<LineReading>();
            readings.Add(box.lineReading);

            box.lineReading.ShowFrame = false;
            box.lineReading.Set(Input.compass, Camera.current.transform, Input.deviceOrientation);
            box.lineReading.widthMultiplier = 0.01f * unitLength;
            box.lineReading.gameObject.transform.position = center;
            box.lineReading.gameObject.transform.localScale *= unitLength;

            box.shapeReading = new GameObject().AddComponent<ShapeReading>();

            box.shapeReading.Set(Input.compass, Camera.current.transform, Input.deviceOrientation);
            box.shapeReading.gameObject.transform.position = center;
            box.shapeReading.gameObject.transform.localScale *= unitLength;


         //   debug.text += reading.ToString();

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

