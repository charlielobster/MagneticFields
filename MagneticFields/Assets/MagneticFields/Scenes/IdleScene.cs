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
        private GameObject lightObj;
        private ARPlaneManager planeManager;

        void Awake()
        {
            // Make a game object
            lightObj = new GameObject();

            // Add the light component
            var light = lightObj.AddComponent<Light>();
            light.type = LightType.Directional;

            var reading = new GameObject();
            cameraReading = reading.AddComponent<CameraReading>();

            debug = GameObject.Find("Debug").GetComponent<Text>();
            debug.text += "Awaking Idle\n";
        }

        void Update()
        {
            var transform = Camera.current.transform;
            var position = transform.position;

            var output = Utils.DebugVector("camera", position);
            output += "\n";

            var reading = Input.compass.rawVector;
            output += Utils.DebugVector("compass", reading);

            debug.text = output;

            var displacement = position + transform.forward * 3f;

            cameraReading.Reading = reading;
            cameraReading.Position = displacement;
            
            lightObj.transform.rotation = transform.rotation;
        }
    }
}