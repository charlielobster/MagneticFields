using System;
using UnityEngine;
using UnityEngine.UI;

namespace MagneticFields.UI
{
    public class WorldCanvas : MonoBehaviour
    {
        private Text text;

        public String Text
        {
            get
            {
                Init();
                return text.text;
            }
            set
            {
                Init();
                text.text = value;
            }
        }

        private bool inited = false;

        private void Init()
        {
            if (!inited)
            {
                var canvas = this.gameObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.WorldSpace;
                CanvasScaler cs = this.gameObject.AddComponent<CanvasScaler>();
                cs.scaleFactor = 10f;
                cs.dynamicPixelsPerUnit = 10f;
                this.gameObject.AddComponent<GraphicRaycaster>();

                GameObject panelObject = new GameObject();
                panelObject.transform.parent = this.gameObject.transform;

                var pr = panelObject.AddComponent<CanvasRenderer>();
                pr.transform.localScale = new Vector3(.32f, .32f, 0); // (.32f, .16f, 0); // for 4 lines
                pr.transform.position = new Vector3(20f, -19f, 0);
                pr.SetAlpha(.4f);

                var i = panelObject.AddComponent<Image>();
                i.color = Color.black;

                GameObject textObject = new GameObject();
                textObject.transform.position = new Vector3(56f, -54f, 0);  // for 9 lines

                //textObject.transform.position = new Vector3(57f, -64f, 0); // for 4 lines
                textObject.transform.parent = this.gameObject.transform;

                text = textObject.AddComponent<Text>();
                text.alignment = TextAnchor.UpperLeft;
                text.horizontalOverflow = HorizontalWrapMode.Overflow;
                text.verticalOverflow = VerticalWrapMode.Overflow;
                text.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
                text.fontSize = 3;
                text.enabled = true;
                text.color = Color.white;

                this.gameObject.transform.localScale = new Vector3(0.025f, 0.025f, 0);
                inited = true;
            }
        }
        
        void Start()
        {
            Init();
        }

        void Update()
        {
            this.gameObject.transform.rotation = Camera.current.transform.rotation;
        }
    }
}

