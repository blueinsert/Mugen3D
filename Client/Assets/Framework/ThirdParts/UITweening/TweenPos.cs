using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("UI/Tweening/Position")]
    public class TweenPos : TweenMain 
    {
        public Vector3 from;
        public Vector3 to;
        private Vector3 _from;
        private Vector3 _to;

        public Vector3 value
        {
            get { return rect.localPosition; }
            set { rect.localPosition = value; }
        }

        protected override void Start()
        {
            if (fromOffset) _from = value + from;
            else _from = from;
            if (toOffset) _to = value + to;
            else _to = to;
        }

        protected override void OnUpdate(float factor, bool isFinished)
        {
            value = Vector3.Lerp(_from, _to, factor);
        }

        public override void ToCurrentValue() { to = value; }
        public override void FromCurrentValue() { from = value; }

        /// <summary>
        /// Create a TweenPos Component, and starts a tween.
        /// </summary>
        /// <param name="go">GameObject to Apply the tween too</param>
        /// <param name="duration">How long the tween will take</param>
        /// <param name="pos">The final Value at the end of the tween</param>
        /// <param name="Style">The tweening style</parm>
        /// <param name="method">The tweening method</parm>
        /// <param name="finished">The method execute at the end of the tween</param>
        /// <returns>Reference to the TweenPos component</returns>
        public static TweenPos Tween(GameObject go, float duration, Vector3 pos,
            Style style = Style.Once, Method method = Method.Linear, UnityAction finished = null)
        {
            TweenPos cls = TweenMain.Tween<TweenPos>(go, duration, style, method, finished);
            cls.from = cls.value;
            cls.to = pos;
            cls.Start();
            return cls;
        }
    }
}
