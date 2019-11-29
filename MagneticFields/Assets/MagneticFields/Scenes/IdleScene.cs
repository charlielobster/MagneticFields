using System;
using UnityEngine;
using UnityEngine.UI;
using MagneticFields.Reading;
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
        
        private Vector3 rawVector;

        public const double IDLE_TIME = 4e7; // 1e7 Ticks a second
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
                
                heading.degrees = Input.compass.magneticHeading;
                heading.gameObject.transform.rotation = transform.rotation;

                lineReading.rawVector = rawVector;
                lineReading.gameObject.transform.rotation = transform.rotation;

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
            output += String.Format("magneticHeading: {0,10:00.00}\n", heading.degrees);
            debug.text = output;
        }
    }
}