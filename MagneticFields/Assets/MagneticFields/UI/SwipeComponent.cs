using UnityEngine;
using UnityEngine.UI;

namespace MagneticFields.UI
{
    public class SwipeComponent : MonoBehaviour
    {
        //private Text debug;
        public static float SWIPE_THRESHOLD = 20f;

        protected Vector2 fingerDown;
        protected Vector2 fingerUp;
        protected bool detectSwipeOnlyAfterRelease = false;

        public virtual void Awake()
        {
            //debug = GameObject.Find("Debug").GetComponent<Text>();
            //debug.text += "Awaking SwipeScene\n";
        }

        // Update is called once per frame
        public virtual void Update()
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    fingerUp = touch.position;
                    fingerDown = touch.position;
                }

                //Detects Swipe while finger is still moving
                if (touch.phase == TouchPhase.Moved)
                {
                    if (!detectSwipeOnlyAfterRelease)
                    {
                        fingerDown = touch.position;
                        checkSwipe();
                    }
                }

                //Detects swipe after finger is released
                if (touch.phase == TouchPhase.Ended)
                {
                    fingerDown = touch.position;
                    checkSwipe();
                }
            }
        }

        void checkSwipe()
        {
            //Check if Vertical swipe
            if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove())
            {
                //Debug.Log("Vertical");
                if (fingerDown.y - fingerUp.y > 0)//up swipe
                {
                    OnSwipeUp();
                }
                else if (fingerDown.y - fingerUp.y < 0)//Down swipe
                {
                    OnSwipeDown();
                }
                fingerUp = fingerDown;
            }

            //Check if Horizontal swipe
            else if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove())
            {
                //Debug.Log("Horizontal");
                if (fingerDown.x - fingerUp.x > 0)//Right swipe
                {
                    OnSwipeRight();
                }
                else if (fingerDown.x - fingerUp.x < 0)//Left swipe
                {
                    OnSwipeLeft();
                }
                fingerUp = fingerDown;
            }

            //No Movement at-all
            else
            {
                //Debug.Log("No Swipe!");
            }
        }

        float verticalMove()
        {
            return Mathf.Abs(fingerDown.y - fingerUp.y);
        }

        float horizontalValMove()
        {
            return Mathf.Abs(fingerDown.x - fingerUp.x);
        }

        public virtual void OnSwipeUp()
        {
            //var output = "OnSwipeUp";
            //debug.text = output;
        }

        public virtual void OnSwipeDown()
        {
            //var output = "OnSwipeDown";
            //debug.text = output;
        }

        public virtual void OnSwipeLeft()
        {
        //    var output = "OnSwipeLeft";
        //    debug.text = output;
        }

        public virtual void OnSwipeRight()
        {
            //var output = "OnSwipeRight";
            //debug.text = output;
        }
    }
}
