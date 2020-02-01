using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MagneticFields.UI
{
    public abstract class TapBehavior : DebugBehaviour
    {
        protected float[] timeTouchBegan;
        protected bool[] touchDidMove;
        protected float tapTimeThreshold = 0.2f;

        public virtual void Start()
        {
            timeTouchBegan = new float[10];
            touchDidMove = new bool[10];
        }

        public abstract void OnTap();

        public virtual void Update()
        {
            foreach (Touch touch in Input.touches)
            {
                var pointer = new PointerEventData(EventSystem.current);
                pointer.position = touch.position;
                var results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointer, results);
                if (results.Count == 0)
                {
                    int fingerIndex = touch.fingerId;

                    if (touch.phase == TouchPhase.Began)
                    {
                        //debug.text = string.Format("Finger #{0} entered", fingerIndex);
                        timeTouchBegan[fingerIndex] = Time.time;
                        touchDidMove[fingerIndex] = false;
                    }
                    if (touch.phase == TouchPhase.Moved)
                    {
                        //debug.text += string.Format("\nFinger #{0} moved!" + fingerIndex);
                        touchDidMove[fingerIndex] = true;
                    }
                    if (touch.phase == TouchPhase.Ended)
                    {
                        float tapTime = Time.time - timeTouchBegan[fingerIndex];
                        //debug.text += "\nFinger #" + fingerIndex.ToString() + " left. Tap time: " + tapTime.ToString();
                        if (tapTime <= tapTimeThreshold && touchDidMove[fingerIndex] == false)
                        {
                            OnTap();
                            debug.text += "\nFinger #" + fingerIndex.ToString() + " TAP DETECTED at: " + touch.position.ToString();
                        }
                    }
                }
            }
        }
    }
}
