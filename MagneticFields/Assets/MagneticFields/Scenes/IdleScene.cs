﻿using System;
using UnityEngine;
using MagneticFields.Reading;
using MagneticFields.UI;
using UnityEngine.UI;
using static MagneticFields.Geometry.Utils;

namespace MagneticFields.Scenes
{
    public class IdleScene : DebugBehaviour
    {
        public const double TICKS_PER_SECOND = 1e7; // 1e7 Ticks a second

        private Heading heading;
        private ShapeReading shapeReading;
        private LineReading lineReading;
        private Light directionalLight;
        
        private Vector3 rawVector;
        private DateTime lastUpdated;

        private GameObject idleMenu
        {
            get => GameObject.Find("IdlePanel");
        }

        private Slider rateSlider
        {
            get => idleMenu.transform.Find("FillPanel/RateSlider")
                .transform.Find("BorderPanel/FillPanel/Slider").GetComponent<Slider>();
        }

        private Toggle headingToggle
        {
            get => idleMenu.transform.Find("FillPanel/HeadingToggle").GetComponent<Toggle>();
        }

        private Toggle shapeToggle
        {
            get => idleMenu.transform.Find("FillPanel/D3VectorToggle").GetComponent<Toggle>();
        }

        private Toggle frameToggle
        {
            get => idleMenu.transform.Find("FillPanel/FrameToggle").GetComponent<Toggle>();
        }

        void OnHeadingToggleChanged()
        {
            heading.gameObject.SetActive(headingToggle.isOn);
        }

        void OnShapeToggleChanged()
        {
            shapeReading.SetActive(shapeToggle.isOn);
        }

        void OnFrameToggleChanged()
        {
            lineReading.showFrame = frameToggle.isOn;
        }

        void Awake()
        {
            directionalLight = new GameObject().AddComponent<Light>();
            directionalLight.type = LightType.Directional;

            shapeReading = new GameObject().AddComponent<ShapeReading>();
            lineReading = new GameObject().AddComponent<LineReading>();
            heading = new GameObject().AddComponent<Heading>();

            headingToggle.onValueChanged.AddListener(delegate { OnHeadingToggleChanged(); });
            shapeToggle.onValueChanged.AddListener(delegate { OnShapeToggleChanged(); });
            frameToggle.onValueChanged.AddListener(delegate { OnFrameToggleChanged(); });

            lastUpdated = DateTime.Now;
        }

        void Update()
        {
            // calculate location to place virtual objects directly in front of the camera
            var transform = Camera.current.transform;
            var position = transform.position + transform.forward * 2.5f;

            // only change things if time has elapsed
            var elapsedTicks = (DateTime.UtcNow.Ticks - lastUpdated.Ticks);
            if (elapsedTicks > (rateSlider.value * TICKS_PER_SECOND))
            {
                var compass = Input.compass;
                var orientation = Input.deviceOrientation;

                heading.degrees = compass.magneticHeading;
                lineReading.Set(compass, orientation);
                shapeReading.Set(compass, orientation);

                lastUpdated = DateTime.UtcNow;
                debug.text = "heading:" + heading.degrees + " " + DebugVector("rawVector", compass.rawVector);
            }

            // place virtual objects directly in front of the camera
            lineReading.gameObject.transform.position = position;
            heading.gameObject.transform.position = position;
            shapeReading.gameObject.transform.position = position;

            directionalLight.gameObject.transform.rotation = transform.rotation;            
        }

        int quadrant(float heading)
        {
            return (int)heading / 90;
        }

        int quadrant(float x, float z)
        {
            if (z > 0)
                return x > 0 ? 3 : 0;
            else
                return x > 0 ? 2 : 1;
        }
    }
}