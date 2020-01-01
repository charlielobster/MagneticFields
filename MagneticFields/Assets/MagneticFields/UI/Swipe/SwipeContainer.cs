using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MagneticFields.UI.Swipe
{
    public class SwipeContainer : SwipePanel
    {
        private CanvasGroup currentCanvas;
        private Image currentButtonPanel;

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

        private Image optionsButtonPanel
        {
            get => GameObject.Find("OptionsButtonPanel").GetComponent<Image>();
        }

        private Image idleButtonPanel
        {
            get => GameObject.Find("IdleButtonPanel").GetComponent<Image>();
        }

        private Image placeButtonPanel
        {
            get => GameObject.Find("PlaceButtonPanel").GetComponent<Image>();
        }

        private Image continuousButtonPanel
        {
            get => GameObject.Find("ContinuousButtonPanel").GetComponent<Image>();
        }

        private CanvasGroup optionsCanvas
        {
            get => GameObject.Find("OptionsPanel").GetComponent<CanvasGroup>();
        }

        private CanvasGroup idleCanvas
        {
            get => GameObject.Find("IdlePanel").GetComponent<CanvasGroup>();
        }

        private CanvasGroup continuousCanvas
        {
            get => GameObject.Find("ContinuousPanel").GetComponent<CanvasGroup>();
        }

        private CanvasGroup placeCanvas
        {
            get => GameObject.Find("PlacePanel").GetComponent<CanvasGroup>();
        }

        void HideTab(Image buttonPanel, CanvasGroup canvasGroup)
        {
            buttonPanel.color = new Color32(120, 49, 152, 255);
            canvasGroup.alpha = 0f; 
            canvasGroup.blocksRaycasts = false;
        }

        void ShowTab(Image buttonPanel, CanvasGroup canvasGroup)
        {
            buttonPanel.color = new Color32(94, 94, 94, 255);
            canvasGroup.alpha = 1f; 
            canvasGroup.blocksRaycasts = true; 
        }

        void OnIdleButtonClicked()
        {


            //canvasGroup.alpha = 0f; //this makes everything transparent
            //canvasGroup.blocksRaycasts = false;



            debug.text = "OnIdleButtonClicked";
            OnButtonClicked(idleButtonPanel, idleCanvas);
            SceneManager.LoadScene("IdleScene");
        }

        void OnContinuousButtonClicked()
        {
            debug.text = "OnContinuousButtonClicked";

            OnButtonClicked(continuousButtonPanel, continuousCanvas);
            SceneManager.LoadScene("ContinuousScene");
        }

        void OnPlaceButtonClicked()
        {
            debug.text = "OnPlaceButtonClicked";
            OnButtonClicked(placeButtonPanel, placeCanvas);
            SceneManager.LoadScene("PlaceScene");
        }

        void OnOptionsButtonClicked()
        {
            debug.text = "OnOptionsButtonClicked";
            OnButtonClicked(optionsButtonPanel, optionsCanvas);
        }

        private void OnButtonClicked(Image nextButtonPanel, CanvasGroup nextCanvas)
        {
            try
            {
                debug.text += "\nsetting: " + nextCanvas + "\n";
                HideTab(currentButtonPanel, currentCanvas);
                currentButtonPanel = nextButtonPanel;
                currentCanvas = nextCanvas;
                ShowTab(currentButtonPanel, currentCanvas);
                debug.text += "\nOnButtonClicked";
            }
            catch (Exception e)
            {
                debug.text = e.ToString();
            }
        }

        public override void Awake()
        {

            base.Awake();

            GameObject[] objs = GameObject.FindGameObjectsWithTag("SwipeContainerObject");

            if (objs.Length > 1)
            {
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(this.gameObject);

            Input.location.Start();
            Input.compass.enabled = true;


            try
            {
                SceneManager.LoadScene("IdleScene");
            }
            catch (Exception e)
            {
                debug.text = e.ToString();
            }






            try
            {
                HideTab(continuousButtonPanel, continuousCanvas);
                HideTab(placeButtonPanel, placeCanvas);
                HideTab(optionsButtonPanel, optionsCanvas);
                ShowTab(idleButtonPanel, idleCanvas);

                currentButtonPanel = idleButtonPanel;
                currentCanvas = idleCanvas;

                idleButton.onClick.AddListener(OnIdleButtonClicked);
                continuousButton.onClick.AddListener(OnContinuousButtonClicked);
                placeButton.onClick.AddListener(OnPlaceButtonClicked);
                optionsButton.onClick.AddListener(OnOptionsButtonClicked);
                // debug.text = "SwipeContainerScript.Awake";
            }
            catch (Exception e)
            {
                debug.text = e.ToString();
            }


        }
    }
}

