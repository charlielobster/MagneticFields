using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

namespace MagneticFields.UI
{
    public class PersistentMenu : SwipePanel
    {
        //private Dropdown sceneDropdown;

        protected int swipeOffset = 0;
 
        //public enum Scene
        //{
        //    Idle,
        //    Continuous,
        //    Place
        //}

        public override void Awake()
        {
            base.Awake();

            GameObject[] objs = GameObject.FindGameObjectsWithTag("PersistentMenu");

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
            //foreach (var t in tabs)
            //{
            //    var btn = this.gameObject.AddComponent<Button>();
            //}

            //sceneDropdown = GameObject.Find("SceneDropdown").GetComponent<Dropdown>(); ;
            //sceneDropdown.options.Clear();
            //for (int i = 0; i < 3; i++)
            //{
            //    sceneDropdown.options.Insert(i,
            //        new Dropdown.OptionData(((Scene)i).ToString("g")));
            //}
            //sceneDropdown.onValueChanged.AddListener(
            //    delegate { OnSceneChanged(sceneDropdown); });
            //sceneDropdown.value = 0;
            //OnSceneChanged(sceneDropdown);

        }

        //void OnSceneChanged(Dropdown dropdown)
        //{
        //    var sceneName = string.Format("{0}Scene", ((Scene)dropdown.value).ToString("g"));
        //    SceneManager.LoadScene(sceneName);
        //}
    }
}

