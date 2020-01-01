using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

namespace MagneticFields.UI.Swipe
{
    public enum SwipeState
    {
        NO_SWIPE,
        SWIPE_LEFT,
        SWIPE_RIGHT,
        SWIPE_UP,
        SWIPE_DOWN
    }

    public abstract class SwipeBehavior : DebugBehaviour
    {
        protected Vector2 delta;
        protected Vector2 lastDelta;
        protected Vector2 acceleration;
        protected SwipeState state;

        private static bool TouchedUi(Vector2 vector)
        {
            var pointer = new PointerEventData(EventSystem.current)
                {
                    position = vector
                };
            var raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);
            return raycastResults.Any((obj) => 
                obj.gameObject.layer == 5 && 
                obj.gameObject.name.IndexOf("Panel") < 0);                
        }

        public virtual void Update()
        {
            foreach (Touch touch in Input.touches)
            {
                if (!TouchedUi(touch.position))
                {
                    switch (touch.phase)
                    {
                        case TouchPhase.Moved:
                        case TouchPhase.Ended:

                            var swipeState = TestForSwipe(touch.deltaPosition);
                            switch (swipeState)
                            {
                                case SwipeState.SWIPE_RIGHT:
                                case SwipeState.SWIPE_LEFT:
                                    SetSwipeState(touch, swipeState);
                                    break;
                                default:
                                    break;
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        private void SetSwipeState(Touch touch, SwipeState swipeState)
        {
            state = swipeState;
            delta = touch.deltaPosition;
            if (lastDelta != null)
            {
                acceleration = delta - lastDelta;
            }
            lastDelta = delta;
        }
        
        private static SwipeState TestForSwipe(Vector2 delta)
        {
            float threshold = 20.0f;
            Vector2 deltaAbs = new Vector2(Math.Abs(delta.x), Math.Abs(delta.y));
            SwipeState state = SwipeState.NO_SWIPE;

            if (deltaAbs.y > threshold && deltaAbs.y > deltaAbs.x)
            {
                state = (delta.y > 0 ? SwipeState.SWIPE_UP : SwipeState.SWIPE_DOWN);
            }
            else if (deltaAbs.x > threshold && deltaAbs.x > deltaAbs.y)
            {
                state = (delta.x > 0 ? SwipeState.SWIPE_RIGHT : SwipeState.SWIPE_LEFT);
            }
            return state;
        }
    }
}
