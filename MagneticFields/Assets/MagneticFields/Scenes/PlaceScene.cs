using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using MagneticFields.Reading;
using MagneticFields.UI;

namespace MagneticFields.Scenes
{
    public class PlaceScene : DebugBehaviour
    {
        public GameObject gridElement;
        
        private ARRaycastManager raycastManager;
        private ARPlaneManager planeManager;
        private ARAnchorManager referencePointManager;
        private ARAnchor referencePoint = null;
        private List<ARRaycastHit> hits = new List<ARRaycastHit>();
        private int samples = 1;
        private int xDimension = 4;
        private int yDimension = 3;
        private int zDimension = 4;
        private float unitLength = 1f / 8f;
        private bool calibrate = false;
        private bool calibrationComplete = false;

        private GameObject[] boundaryBoxes;
        private GameObject[] fieldObjects;
        private FieldReading[] readings;
        private Vector3[] rawVectors;
        private Vector3[] calibrations;
        private int[] counts;

        private Pose pose;
        private Vector3 center;
        private bool stopUpdate = false;
        
        private Slider xSlider
        {
            get
            {
                return GameObject.Find("XSlider").GetComponent<Slider>();
            }
        }

        void OnXSliderChanged(Slider slider)
        {
            xDimension = (int)slider.value;
        }

        private Slider ySlider
        {
            get
            {
                return GameObject.Find("YSlider").GetComponent<Slider>();
            }
        }

        void OnYSliderChanged(Slider slider)
        {
            yDimension = (int)slider.value;
        }

        private Slider zSlider
        {
            get
            {
                return GameObject.Find("ZSlider").GetComponent<Slider>();
            }
        }

        void OnZSliderChanged(Slider slider)
        {
            zDimension = (int)slider.value;
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
        }

        private Slider samplesSlider
        {
            get
            {
                return GameObject.Find("SamplesSlider").GetComponent<Slider>();
            }
        }

        void OnSamplesSliderChanged(Slider slider)
        {
            samples = (int)slider.value;
        }

        private Toggle calibrationToggle
        {
            get
            {
                return GameObject.Find("CalibrationToggle").GetComponent<Toggle>();
            }
        }

        void OnCalibrationToggleChanged(Toggle toggle)
        {
     //       debug.text += ("\ntoggle.value: " + toggle.isOn + "\n");
            calibrate = toggle.isOn;
        }

        private Button resetButton
        {
            get
            {
                return GameObject.Find("ResetButton").GetComponent<Button>();
            }
        }

        private Button hideButton
        {
            get
            {
                return GameObject.Find("HideButton").GetComponent<Button>();
            }
        }

        void OnResetButtonClicked()
        {
            //debug.text += ("Resetting with values\nxDimension: " + 
            //    xDimension + "\nyDimension: " + yDimension + "\nzDimension: " + 
            //    zDimension + "\nunitLength: " + unitLength + "\nsamples: " + samples + "\n");
            Reset();
        }

        private void Reset()
        {
            EnableTracking();
            if (boundaryBoxes != null && boundaryBoxes.Length > 0)
            {
                foreach (var b in boundaryBoxes)
                {
                    Destroy(b);
                }
            }
            boundaryBoxes = new GameObject[xDimension * yDimension * zDimension];
            if (fieldObjects != null && fieldObjects.Length > 0)
            {
                foreach (var f in fieldObjects)
                {
                    Destroy(f);
                }
            }
            fieldObjects = new GameObject[xDimension * yDimension * zDimension];
            readings = new FieldReading[xDimension * yDimension * zDimension];
            rawVectors = new Vector3[xDimension * yDimension * zDimension];
            if (calibrate)
            {
                calibrations = new Vector3[xDimension * yDimension * zDimension];
            }
            counts = new int[xDimension * yDimension * zDimension];
            for (var i = 0; i < xDimension * yDimension * zDimension; i++)
            {
                rawVectors[i] = new Vector3(0, 0, 0);
                if (calibrate)
                {
                    calibrations[i] = new Vector3(0, 0, 0);
                }
                counts[i] = 0;
            }
            calibrationComplete = false;
            stopUpdate = false;
        }

        void OnHideClicked()
        {
            GameObject.Find("HideMePanel").SetActive(false);
            GameObject.Find("DebugPanel").SetActive(false);
        }

        void Awake()
        {
     //       debug.text = "Awaking Place Scene...";
            try
            {
                planeManager = GetComponent<ARPlaneManager>();
                raycastManager = GetComponent<ARRaycastManager>();
                referencePointManager = GetComponent<ARAnchorManager>();
                xSlider.onValueChanged.AddListener(delegate { OnXSliderChanged(xSlider); });
                ySlider.onValueChanged.AddListener(delegate { OnYSliderChanged(ySlider); });
                zSlider.onValueChanged.AddListener(delegate { OnZSliderChanged(zSlider); });
                unitSlider.onValueChanged.AddListener(delegate { OnUnitSliderChanged(unitSlider); });
                samplesSlider.onValueChanged.AddListener(delegate { OnSamplesSliderChanged(samplesSlider); });
                calibrationToggle.onValueChanged.AddListener(delegate { OnCalibrationToggleChanged(calibrationToggle); });
                resetButton.onClick.AddListener(OnResetButtonClicked);
                hideButton.onClick.AddListener(OnHideClicked);
                Reset();
            }
            catch (Exception e)
            {
      //          debug.text += ("\n" + e.ToString() + "\n");
            }
        }

        void Update()
        {
            if (!stopUpdate)
            {
                try
                {
                    if (Camera.current != null)
                    {
        //                debug.text = Utils.DebugVector("camera.position", Camera.current.transform.position);
                    }

                    if (referencePoint == null)
                    {
                        if (Input.touchCount == 0)
                            return;

                        var touch = Input.GetTouch(0);
                        if (touch.phase != TouchPhase.Began)
                            return;

                        if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                        {
                            if (hits.Count > 0)
                            {                               
                                var hit = hits[0];
                                pose = hit.pose;
                                pose.rotation = Quaternion.Euler(0, 0, 0);
                                center = pose.position;

                                var t = planeManager.trackables[hit.trackableId];
                                referencePoint = referencePointManager.AttachAnchor(t, pose);
                             //   DisableTracking(t);
                                PlaceGrid();
                            }
                        }
                    }

                    if (referencePoint != null)
                    {
                        var currentPosition = Camera.current.transform.position;
                        var xd = unitLength * .5f * xDimension;
                        var zd = unitLength * .5f * zDimension;
                        var yd = unitLength * yDimension;

                        if (((center.x - xd) <= currentPosition.x) &&
                            (currentPosition.x <= (center.x + xd)) &&
                            (center.y <= currentPosition.y) &&
                            (currentPosition.y <= (center.y + yd)) &&
                            ((center.z - zd) <= currentPosition.z) &&
                            (currentPosition.z <= (center.z + zd)))
                        {
                //            debug.text += "\nInside reading space...\n";
                            var dc = currentPosition - center;
                            var indices = dc / unitLength;
                            indices = new Vector3(
                                (float)Math.Floor(indices.x + xDimension / 2f),
                                (float)Math.Floor(indices.y), 
                                (float)Math.Floor(indices.z + zDimension / 2f));
  
                            int index = (int)(indices.x * yDimension * zDimension
                                + indices.y * zDimension + indices.z);

                            if (0 <= index &&
                                index < xDimension * yDimension * zDimension && 
                                counts[index] < samples)
                            {
                                if (calibrate && !calibrationComplete)
                                {
                                    calibrations[index] += Input.compass.rawVector;
                                }
                                else
                                {
                                    rawVectors[index] += Input.compass.rawVector;
                                }
                                counts[index]++;

                                if (!calibrate || calibrationComplete)
                                {
                                    var lr = boundaryBoxes[index].GetComponentInChildren<LineRenderer>();
                                    var color = new Color((float)counts[index] / (float)samples, 0, 1f);
                                    lr.material.SetColor("_Color", color);
                                    lr.widthMultiplier = 0.005f * color.r;
                                }

                                if (counts[index] == samples)
                                {
                                    if (calibrate && !calibrationComplete)
                                    {
                                        calibrations[index] /= (float)samples;
                                        var lr = boundaryBoxes[index].GetComponentInChildren<LineRenderer>();
                                        lr.material.SetColor("_Color", Color.white);
                                    }
                                    else if (calibrate)
                                    {
                                        rawVectors[index] /= (float)samples;
                                        rawVectors[index] -= calibrations[index];
                                        readings[index].Reading = rawVectors[index];
                                    }
                                    else
                                    {
                                        rawVectors[index] /= (float)samples;
                                        readings[index].Reading = rawVectors[index];
                                    }


                                    if (!calibrate || calibrationComplete)
                                    {
                                        var lr = boundaryBoxes[index].GetComponentInChildren<LineRenderer>();
                                        lr.material.SetColor("_Color", readings[index].Color);
                                        lr.widthMultiplier = 0.0005f;
                                        fieldObjects[index].transform.localScale *= unitLength;
                                        fieldObjects[index].SetActive(true);
                                    }

                                    if (AllDone())
                                    {
                                        if (calibrate && !calibrationComplete)
                                        {
                              //              debug.text += "\ncalibration complete, taking readings...\n";
                                            calibrationComplete = true;
                                            for (var i = 0; i < counts.Length; i++)
                                            {
                                                counts[i] = 0;
                                            }
                                          //  Thread.Sleep(1000);
                                        } else
                                        {
                                            stopUpdate = true;
                      //                      debug.text += "\nComplete\n";
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                catch (Exception e)
                {
               //     debug.text += e.ToString();
                    stopUpdate = true;
                }
            }
        }

        public void OnDisable()
        {
            GameObject.Find("HideMePanel").SetActive(true);
            GameObject.Find("DebugPanel").SetActive(true);
        }

        private bool AllDone()
        {
            foreach (var c in counts)
                if (c < samples)
                    return false;
            return true;
        }

        private void PlaceGrid()
        {
            var r = Quaternion.Euler(0, 0, 0);
            var xd = .5f * xDimension;
            var zd = .5f * zDimension;

            for (float i = -xd; i < xd; i++)
            {
                for (float j = 0; j < yDimension; j++)
                {
                    for (float k = -zd; k < zd; k++)
                    {
                        var d = unitLength * new Vector3(i, j, k);
                        var position = pose.position + d;

                        int index = (int)((i + xd) * yDimension * zDimension + j * zDimension + (k + zd));
                        boundaryBoxes[index] = Instantiate(gridElement, position, r);

                        var lr = boundaryBoxes[index].GetComponentInChildren<LineRenderer>();
                        lr.material.SetColor("_Color", Color.blue);
                        lr.widthMultiplier = 0.0025f;
                
                        fieldObjects[index] = new GameObject();
                        readings[index] = fieldObjects[index].AddComponent<FieldReading>();

                        fieldObjects[index].SetActive(false);

                        readings[index].Reading = Input.compass.rawVector;

                        d = .5f * unitLength * Vector3.one;
                        readings[index].Position = position + d;

                        fieldObjects[index].transform.localScale *= unitLength;
                        boundaryBoxes[index].transform.localScale *= unitLength;
                    }
                }
            }
        }

        private void EnableTracking()
        {          
            planeManager.enabled = true;
            foreach (var plane in planeManager.trackables)
                plane.gameObject.SetActive(true);

            referencePointManager.enabled = true;
            if (referencePoint != null)
            {
                referencePointManager.RemoveAnchor(referencePoint);
                referencePoint = null;
            }
        }

        private void DisableTracking(ARPlane exclude)
        {
            planeManager.enabled = false;
            foreach (var plane in planeManager.trackables)
                if (plane != exclude)
                    plane.gameObject.SetActive(false);

            referencePointManager.enabled = false;
        }
    }
}

