using System;
using UnityEngine;
using UnityEngine.UI;
using MagneticFields.Reading;

namespace MagneticFields.Scenes
{
    public class IdleScene : MonoBehaviour
    {
        public const double IDLE_TIME = 1e5; // 1e7 Ticks a second

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
            debug.text += "\nAwaking Idle Scene...\n";

            lastUpdated = DateTime.Now;
        }

        void Update()
        {
            // calculate location to place virtual objects directly in front of the camera
            var transform = Camera.current.transform;
            var position = transform.position + transform.forward * 2.5f;

            // only change things if time has elapsed
            var elapsedTicks = (DateTime.UtcNow.Ticks - lastUpdated.Ticks);
            if (elapsedTicks > IDLE_TIME)
            {
                var compass = Input.compass;
                var orientation = Input.deviceOrientation;

                heading.gameObject.transform.rotation = transform.rotation;
                heading.degrees = compass.magneticHeading;

                lineReading.Set(compass, transform, orientation);
                shapeReading.Set(compass, transform, orientation);
                
                lastUpdated = DateTime.UtcNow;
            }

            // always place virtual objects directly in front of the camera
            lineReading.gameObject.transform.position = position;
            heading.gameObject.transform.position = position;
            shapeReading.gameObject.transform.position = position;
            directionalLight.gameObject.transform.rotation = transform.rotation;

            var output = String.Empty;
            output += shapeReading.ToString();
            debug.text = output;
        }
    }
}