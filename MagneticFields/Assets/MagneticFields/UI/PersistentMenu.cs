using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MagneticFields.UI
{
    public class PersistentMenu : MonoBehaviour
    {
        private Dropdown sceneDropdown;
        private Text debug;

        public enum Scene
        {
            Continuous,
            Place,
            Idle
        }

        void Awake()
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("PersistentMenu");

            if (objs.Length > 1)
            {
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(this.gameObject);

            Input.location.Start();
            Input.compass.enabled = true;

            debug = GameObject.Find("Debug").GetComponent<Text>();
            debug.text = "PersistentMenu";

            sceneDropdown = GameObject.Find("SceneDropdown").GetComponent<Dropdown>(); ;
            sceneDropdown.options.Clear();
            for (int i = 0; i < 3; i++)
            {
                sceneDropdown.options.Insert(i,
                    new Dropdown.OptionData(((Scene)i).ToString("g")));
            }
            sceneDropdown.onValueChanged.AddListener(
                delegate { OnSceneChanged(sceneDropdown); });
            sceneDropdown.value = 0;
            OnSceneChanged(sceneDropdown);
        }

        void OnSceneChanged(Dropdown dropdown)
        {
            var sceneName = string.Format("{0}Scene", ((Scene)dropdown.value).ToString("g"));
            SceneManager.LoadScene(sceneName);
        }
    }
}

