using UnityEngine.UI;

namespace MagneticFields.UI
{
    public class ComboSlider : DebugBehaviour
    {
        private string path
        {
            get => "BorderPanel/FillPanel/";
        }

        private Text minLabel
        {
            get => gameObject.transform.Find(path + "MinLabel").GetComponent<Text>();
        }

        private Text maxLabel
        {
            get => gameObject.transform.Find(path + "MaxLabel").GetComponent<Text>();
        }

        private InputField currentInput
        {
            get => gameObject.transform.Find(path + "CurrentInput").GetComponent<InputField>();
        }

        private Slider slider
        {
            get => gameObject.transform.Find(path + "Slider").GetComponent<Slider>();
        }

        void OnSliderChanged()
        {
            string asString = string.Format("{0,0:0.0#}", slider.value);
            currentInput.SetTextWithoutNotify(asString);
        }

        void OnCurrentValueChanged()
        {
            var result = default(float);
            if (float.TryParse(currentInput.text, out result))
            {
                if (slider.minValue > result)
                {
                    slider.minValue = result;
                    minLabel.text = string.Format("{0,0:0.0#}", result);
                }
                if (slider.maxValue < result)
                {
                    slider.maxValue = result;
                    maxLabel.text = string.Format("{0,0:0.0#}", result);
                }
                slider.SetValueWithoutNotify(result);
            }
        }

        public virtual void Awake()
        {
            slider.onValueChanged.AddListener(delegate { OnSliderChanged(); });
            currentInput.onValueChanged.AddListener(delegate { OnCurrentValueChanged(); });
        }
    }
}
