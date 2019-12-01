﻿using System;
using UnityEngine;
using UnityEngine.UI;
using MagneticFields.Reading;
using MagneticFields.Geometry;

namespace MagneticFields.Scenes
{
    public class IdleScene : MonoBehaviour
    {
        public const double IDLE_TIME = 4e7; // 1e7 Ticks a second

        private Text debug;

        private Heading heading;
        private ShapeReading shapeReading;
        private LineReading lineReading;
        private Light directionalLight;
        
        private Vector3 rawVector;
        private DateTime lastUpdated;

        void Awake()
        {
            directionalLight = new GameObject().AddComponent<Light>();
            directionalLight.type = LightType.Directional;

            shapeReading = new GameObject().AddComponent<ShapeReading>();
            lineReading = new GameObject().AddComponent<LineReading>();
            heading = new GameObject().AddComponent<Heading>();

            debug = GameObject.Find("Debug").GetComponent<Text>();
            debug.text += "Awaking Idle\n";

            lastUpdated = DateTime.Now;
        }

        void Update()
        {
            var transform = Camera.current.transform;

            // calculate location to place virtual objects directly in front of the camera
            var position = transform.position + transform.forward * 2.5f;

            // only change things if time has elapsed
            var elapsedTicks = (DateTime.UtcNow.Ticks - lastUpdated.Ticks);
            if (elapsedTicks > IDLE_TIME)
            {
                rawVector = Input.compass.rawVector;

                // the rawVector's coordinates are affected by deviceOrientation
                switch (Input.deviceOrientation)
                {
                    case DeviceOrientation.Portrait:
                    case DeviceOrientation.PortraitUpsideDown:
                    case DeviceOrientation.LandscapeLeft:
                    case DeviceOrientation.LandscapeRight:
                    case DeviceOrientation.FaceDown:
                    case DeviceOrientation.FaceUp:
                    case DeviceOrientation.Unknown:
                    default:
                        // upright (normal) portait mode, must correct for -z values           
                        rawVector.z = -rawVector.z; 
                        break;
                }
                
                heading.gameObject.transform.rotation = transform.rotation;
                heading.degrees = Input.compass.magneticHeading;

//                lineReading.gameObject.transform.rotation = transform.rotation;

                lineReading.Set(rawVector, transform.rotation, transform.position, DateTime.Now);

                shapeReading.Set(rawVector, transform.rotation, transform.position, DateTime.Now);

                lastUpdated = DateTime.UtcNow;
            }

            // always place virtual objects directly in front of the camera
            lineReading.gameObject.transform.position = position;
            heading.gameObject.transform.position = position;
            shapeReading.gameObject.transform.position = position;
            directionalLight.gameObject.transform.rotation = transform.rotation;

            var output = String.Empty;
            //output += String.Format("{0}\n", Utils.DebugVector("rawVector", rawVector));
            //output += String.Format("heading.degrees: {0,10:00.00}\n", heading.degrees);
            output += shapeReading.ToString();
            debug.text = output;
        }
    }
}