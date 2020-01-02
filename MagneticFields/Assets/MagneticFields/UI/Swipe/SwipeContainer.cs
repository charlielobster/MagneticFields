using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MagneticFields.UI.Swipe
{
    public class SwipeContainer : SwipePanel
    {
        private CanvasGroup currentCanvas;
        private Image currentButton;

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

        void HideTab(Image button, CanvasGroup canvas)
        {
            button.color = Palette.Purple;
            canvas.alpha = 0f; 
            canvas.blocksRaycasts = false;
        }

        void ShowTab(Image button, CanvasGroup canvas)
        {
            button.color = Palette.LightGray;
            canvas.alpha = 1f; 
            canvas.blocksRaycasts = true;
            currentButton = button;
            currentCanvas = canvas;
        }

        void OnIdleButtonClicked()
        {
            OnButtonClicked(idleButtonPanel, idleCanvas);
            SceneManager.LoadScene("IdleScene");
        }

        void OnContinuousButtonClicked()
        {
            OnButtonClicked(continuousButtonPanel, continuousCanvas);
            SceneManager.LoadScene("ContinuousScene");
        }

        void OnPlaceButtonClicked()
        {
            OnButtonClicked(placeButtonPanel, placeCanvas);
            SceneManager.LoadScene("PlaceScene");
        }

        void OnOptionsButtonClicked()
        {
            OnButtonClicked(optionsButtonPanel, optionsCanvas);
        }

        private void OnButtonClicked(Image nextButton, CanvasGroup nextCanvas)
        {
            HideTab(currentButton, currentCanvas);
            ShowTab(nextButton, nextCanvas);
        }

        public override void Awake()
        {
            base.Awake();

            Input.location.Start();
            Input.compass.enabled = true;

            PersistSingletonContainer();

            HideTab(continuousButtonPanel, continuousCanvas);
            HideTab(placeButtonPanel, placeCanvas);
            HideTab(optionsButtonPanel, optionsCanvas);
            ShowTab(idleButtonPanel, idleCanvas);

            SceneManager.LoadScene("IdleScene");

            idleButton.onClick.AddListener(OnIdleButtonClicked);
            continuousButton.onClick.AddListener(OnContinuousButtonClicked);
            placeButton.onClick.AddListener(OnPlaceButtonClicked);
            optionsButton.onClick.AddListener(OnOptionsButtonClicked);
        }

        private void PersistSingletonContainer()
        {
            // ensure only one persistent instance of SwipeContainerObject 
            GameObject[] objs = GameObject.FindGameObjectsWithTag("SwipeContainerObject");
            if (objs.Length > 1)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
    }
}

