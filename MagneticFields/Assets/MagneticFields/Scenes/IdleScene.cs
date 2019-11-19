using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;
using MagneticFields;
using MagneticFields.Reading;
using MagneticFields.UI;
using MagneticFields.Geometry;

namespace MagneticFields.Scenes
{
    public class IdleScene : MonoBehaviour
    {
        private Text debug;
        private CameraReading cameraReading;
        private SkinnyReading skinnyReading;
        private GameObject lightObj;
        private ARPlaneManager planeManager;
        private float currentMagneticHeading;
        private int count = 0;

        void Awake()
        {
            // Make a game object
            lightObj = new GameObject();

            // Add the light component
            var light = lightObj.AddComponent<Light>();
            light.type = LightType.Directional;

            var cameraReadingObject = new GameObject();
            cameraReading = cameraReadingObject.AddComponent<CameraReading>();

            var skinnyReadingObject = new GameObject();
            skinnyReading = skinnyReadingObject.AddComponent<SkinnyReading>();

            debug = GameObject.Find("Debug").GetComponent<Text>();
            debug.text += "Awaking Idle\n";

            currentMagneticHeading = Input.compass.magneticHeading;
        }

        void Update()
        {
            var transform = Camera.current.transform;
            var position = transform.position;

            var output = Utils.DebugVector("camera", position);
            output += "\n";

            var reading = Input.compass.rawVector;
            output += Utils.DebugVector("compass", reading);

            output += String.Format("magneticHeading: {0}\n", Input.compass.magneticHeading);
            var displacement = position + transform.forward * 3f;

            currentMagneticHeading += Input.compass.magneticHeading;
            count = (count + 1) % 8;

            if (count == 7)
            {
                currentMagneticHeading = currentMagneticHeading / 8;


                //  skinnyReading.Reading = .005f * reading;
                skinnyReading.Reading = transform.forward;
                skinnyReading.rotateAboutY(-currentMagneticHeading);
                cameraReading.Reading = reading;
            }

            skinnyReading.Position = displacement;

            cameraReading.Position = displacement;
            debug.text = output;


            
            
            lightObj.transform.rotation = transform.rotation;
        }
    }
}