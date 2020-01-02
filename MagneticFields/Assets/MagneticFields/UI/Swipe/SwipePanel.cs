using UnityEngine;

namespace MagneticFields.UI.Swipe
{
    public class SwipePanel : SwipeBehavior
    {
        protected RectTransform swipeContainer
        {
            get => GameObject.Find("SwipePanel").GetComponent<RectTransform>();
        }

        public virtual void Awake()
        {
            //debug.text = "SwipePanel";
            SetDimension();
        }

        public void SetDimension()
        {
            swipeContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width + 2.0f);
            swipeContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height + 2.0f);
            swipeContainer.transform.position = new Vector3(-Screen.width / 2.0f, Screen.height / 2.0f, 0.0f);
            swipeContainer.ForceUpdateRectTransforms();
        }

        public override void Update()
        {
            base.Update();

            switch (state)
            {
                case SwipeState.SWIPE_LEFT:
                case SwipeState.SWIPE_RIGHT:
                    //debug.text = "Swipe";
                    var position = swipeContainer.position;
                    position.x += delta.x;

                    if (Mathf.Abs(position.x) > (Screen.width / 2.0f))
                    {
                        position.x = Screen.width / 2.0f;
                        position.x *= (state == SwipeState.SWIPE_LEFT ? -1.0f : 1.0f);
                        state = SwipeState.NO_SWIPE;
                    }
                    swipeContainer.position = position;
                    break;

                default:
                    break;
            }                   
        }
    }
}
