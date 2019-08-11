using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    [AddComponentMenu("UI/Tweening/Canvas Group Alpha")]
    public class TweenCGAlpha : TweenMain
    {
        private CanvasGroup cg;

        [Range(0f, 1f)]
        public float
            from,
            to;

        private float
            _from,
            _to;

        void Awake()
        {
            cg = GetComponent<CanvasGroup>();
        }

        public float value
        {
            get { return cg.alpha; }
            set { cg.alpha = value; }
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
        /// Create a TweenCGAlpha Componet and start the tween.
        /// </summary>
        /// <param name="go">GameObject to Apply the tween too</param>
        /// <param name="duration">How long the tween will take</param>
        /// <param name="alpha">The final Value at the end of the tween</param>
        /// <param name="method">The tweening method</parm>
        /// <param name="finished">The method execute at the end of the tween</param>
        /// <returns>Reference to the TweenCGAlpha component</returns>
        public static TweenCGAlpha Tween(GameObject go, float duration, float alpha,
            Style style = Style.Once, Method method = Method.Linear, UnityAction finished = null)
        {
            TweenCGAlpha cls = TweenMain.Tween<TweenCGAlpha>(go, duration, style, method, finished);
            cls.from = cls.value;
            cls.to = alpha;
            cls.Start();
            return cls;
        }
    }
}
