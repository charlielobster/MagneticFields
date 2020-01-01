using System;
using UnityEngine;
using UnityEngine.UI;

namespace MagneticFields.UI
{
    public class SwipeContainerScript : DebugBehaviour
    {
        private Canvas currentCanvas;

        private Button optionsButton
        {
            get => GameObject.Find("OptionsButton").GetComponent<Button>();
        }

        private Button idleButton
        {
            get => GameObject.Find("IdleButton").GetComponent<Button>();
        }

        private Button placeButton
        {
            get => GameObject.Find("PlaceButton").GetComponent<Button>();
        }

        private Button continuousButton
        {
            get => GameObject.Find("ContinuousButton").GetComponent<Button>();
        }

        private Canvas optionsCanvas
        {
            get => GameObject.Find("OptionsCanvas").GetComponent<Canvas>();
        }

        private Canvas idleCanvas
        {
            get => GameObject.Find("IdleCanvas").GetComponent<Canvas>();
        }

        private Canvas continuousCanvas
        {
            get => GameObject.Find("ContinuousCanvas").GetComponent<Canvas>();
        }

        private Canvas placeCanvas
        {
            get => GameObject.Find("PlaceCanvas").GetComponent<Canvas>();
        }

        //void Hide()
        //{
            //canvasGroup.alpha = 0f; //this makes everything transparent
            //canvasGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
        //}

        void OnIdleButtonClicked()
        {


            //canvasGroup.alpha = 0f; //this makes everything transparent
            //canvasGroup.blocksRaycasts = false;

            debug.text = "OnIdleButtonClicked";
            OnButtonClicked(idleCanvas);
        }

        void OnContinuousButtonClicked()
        {
            debug.text = "OnContinuousButtonClicked";
            OnButtonClicked(continuousCanvas);
        }

        void OnPlaceButtonClicked()
        {
            debug.text = "OnPlaceButtonClicked";
            OnButtonClicked(placeCanvas);
        }

        void OnOptionsButtonClicked()
        {
            debug.text = "OnOptionsButtonClicked";
            OnButtonClicked(optionsCanvas);
        }

        private void OnButtonClicked(Canvas nextCanvas)
        {
            try
            {
                debug.text = "setting: " + nextCanvas + "\n";
                currentCanvas.gameObject.SetActive(false);
                currentCanvas = nextCanvas;
                currentCanvas.gameObject.SetActive(true);
//                debug.text = "OnButtonClicked";
            }
            catch (Exception e)
            {
                debug.text = e.ToString();
            }
        }

        void Awake()
        {
            //try
            //{
            //    continuousCanvas.gameObject.SetActive(false);
            //    placeCanvas.gameObject.SetActive(false);
            //    optionsCanvas.gameObject.SetActive(false);
            //    idleCanvas.gameObject.SetActive(true);
                
            //    currentCanvas = idleCanvas;

            //    idleButton.onClick.AddListener(OnIdleButtonClicked);
            //    continuousButton.onClick.AddListener(OnContinuousButtonClicked);
            //    placeButton.onClick.AddListener(OnPlaceButtonClicked);
            //    optionsButton.onClick.AddListener(OnOptionsButtonClicked);
            //   // debug.text = "SwipeContainerScript.Awake";
            //}
            //catch (Exception e)
            //{
            //    debug.text = e.ToString();
            //}


        }
    }
}

