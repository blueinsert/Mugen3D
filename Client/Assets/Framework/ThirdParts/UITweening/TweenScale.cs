using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("UI/Tweening/Scale")]
    public class TweenScale : TweenMain
    {
        public Vector3 from = Vector3.one;
        public Vector3 to = Vector3.one;

        private Vector3 _from = Vector3.one;
        private Vector3 _to = Vector3.one;

        public Vector3 value
        {
            get { return rect.localScale; }
            set { rect.localScale = value; }
        }

        protected override void Start()
        {
            if (fromOffset) _from = value + from;
            else _from = from;
            if (toOffset) _to = value + to;
            else _to = to;
            base.Start();
        }

        protected override void OnUpdate(float factor, bool isFinished)
        {
            value = Vector3.Lerp(_from, _to, factor);
        }

        public override void FromCurrentValue() { from = value; }
        public override void ToCurrentValue() { to = value; }

        /// <summary>
        /// Create a TweenScale Component, and starts a tween.
        /// </summary>
        /// <param name="go">GameObject to Apply the tween too</param>
        /// <param name="duration">How long the tween will take</param>
        /// <param name="scale">The final Value at the end of the tween</param>
        /// <param name="method">The tweening method</parm>
        /// <param name="finished">The method execute at the end of the tween</param>
        /// <returns>Reference to the TweenScale component</returns>
        public static TweenScale Tween(GameObject go, float duration, Vector3 scale,
            Style style = Style.Once, Method method = Method.Linear, UnityAction finished = null)
        {
            TweenScale cls = TweenMain.Tween<TweenScale>(go, duration, style, method, finished);
            cls.from = cls.value;
            cls.to = scale;
            cls.Start();
            return cls;
        }
    }
}
