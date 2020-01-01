using UnityEngine;
using UnityEngine.UI;

namespace MagneticFields.UI
{
    public class DebugBehaviour : MonoBehaviour
    {
        protected Text debug
        {
            get => GameObject.Find("Debug").GetComponent<Text>();
        }
    }
}
