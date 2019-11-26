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

        private Heading heading;
        private CameraReading cameraReading;
        private LineReading lineReading;

        private Light directionalLight;
        
        private float magneticHeading;
        private Vector3 rawVector;

        public const double IDLE_TIME = 2e7; // 10,000,000 Ticks a second
        private DateTime lastUpdated;

        void Awake()
        {
            directionalLight = new GameObject().AddComponent<Light>();
            directionalLight.type = LightType.Directional;

            cameraReading = new GameObject().AddComponent<CameraReading>();
            lineReading = new GameObject().AddComponent<LineReading>();
            heading = new GameObject().AddComponent<Heading>();

            debug = GameObject.Find("Debug").GetComponent<Text>();
            debug.text += "Awaking Idle\n";

            magneticHeading = Input.compass.magneticHeading;
            lastUpdated = DateTime.Now;
        }

        void Update()
        {
            var transform = Camera.current.transform;
            var position = transform.position + transform.forward * 2.5f;
            var elapsedTicks = (DateTime.UtcNow.Ticks - lastUpdated.Ticks);

            if (elapsedTicks > IDLE_TIME)
            {
                rawVector = Input.compass.rawVector;
                rawVector.z = -rawVector.z;              
                magneticHeading = Input.compass.magneticHeading;
                
                heading.Angle = magneticHeading;
                heading.gameObject.transform.rotation = transform.rotation;

                var t = new Vector2((float)(Math.Sqrt(rawVector.x * rawVector.x + rawVector.z * rawVector.z)), rawVector.y);
                t.Normalize();
                var ang = -(float)(Math.Asin(t.y) * (180.0 / Math.PI));
                if (t.y > 0) ang = -ang;
                var s = new Vector2(rawVector.z, rawVector.x);
                s.Normalize();                
                var angle = (float)(Math.Acos(s.x) * (180.0 / Math.PI));
                if (s.y > 0) angle = -angle;
                var q = Quaternion.Euler(90f + ang, 0, 0);
                q = Quaternion.Euler(0, -angle, 0) * q;
                cameraReading.gameObject.transform.rotation = transform.rotation * q;
                cameraReading.Reading = rawVector;

                lineReading.Reading = rawVector;
                lineReading.gameObject.transform.rotation = transform.rotation;

                //  switch (Input.deviceOrientation)
                //  {
                //      case DeviceOrientation.Portrait:
                ////          lineReading.transform.rotation = transform.rotation;
                //          break;
                //      case DeviceOrientation.PortraitUpsideDown:
                //      case DeviceOrientation.LandscapeLeft:
                //      case DeviceOrientation.LandscapeRight:
                //      case DeviceOrientation.FaceDown:
                //      case DeviceOrientation.FaceUp:
                //      case DeviceOrientation.Unknown:
                //      default:
                //  //        lineReading.transform.rotation = transform.rotation * Quaternion.Euler(0, 0, 90);
                //          break;
                //  }

                lastUpdated = DateTime.UtcNow;
            }
            
            lineReading.gameObject.transform.position = position;

            heading.gameObject.transform.position = position;

            cameraReading.gameObject.transform.position = position;

            directionalLight.gameObject.transform.rotation = transform.rotation;

            var output = String.Empty;
            output += String.Format("{0}\n", Utils.DebugVector("compass", rawVector));
            output += String.Format("magneticHeading: {0,10:00.00}\n", magneticHeading);
            debug.text = output;
        }
    }
}