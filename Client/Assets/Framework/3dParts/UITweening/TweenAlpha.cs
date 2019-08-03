using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Graphic))]
    [AddComponentMenu("UI/Tweening/Alpha")]
    public class TweenAlpha : TweenMain
    {
        [Range(0f, 1f)]
        public float
            from,
            to;

        private float
            _from,
            _to;

        public float value
        {
            get { return gfx.color.a; }
            set { gfx.color = new Color(gfx.color.r, gfx.color.g, gfx.color.b, value); }
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
            value = Mathf.Lerp(_from, _to, factor);
        }

        public override void ToCurrentValue() { to = value; }
        public override void FromCurrentValue() { from = value; }

        /// <summary>
        /// Create a TweenAlpha Component, and starts a tween.
        /// </summary>
        /// <param name="go">GameObject to Apply the tween too</param>
        /// <param name="duration">How long the tween will take</param>
        /// <param name="alpha">The final Value at the end of the tween</param>
        /// <param name="method">The tweening method</parm>
        /// <param name="finished">The method execute at the end of the tween</param>
        /// <returns>Reference to the TweenAlpha component</returns>
        public static TweenAlpha Tween(GameObject go, float duration, float alpha,
            Style style = Style.Once, Method method = Method.Linear, UnityAction finished = null)
        {
            TweenAlpha cls = TweenMain.Tween<TweenAlpha>(go, duration, style, method, finished);
            cls.from = cls.value;
            cls.to = alpha;
            cls.Start();
            return cls;
        }
    }
}
