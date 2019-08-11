using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("UI/Tweening/Rotation")]
    public class TweenRot : TweenMain
    {
        public Vector3
            from,
            to;

        private Vector3
            _from,
            _to;

        public Quaternion value
        {
            get { return rect.localRotation; }
            set { rect.localRotation = value; }
        }

        protected override void Start()
        {
            if (fromOffset) _from = value.eulerAngles + from;
            else _from = from;
            if (toOffset) _to = value.eulerAngles + to;
            else _to = to;
        }

        protected override void OnUpdate(float factor, bool isFinished)
        {
            value = Quaternion.Euler(Vector3.Lerp(_from, _to, factor));
        }

        public override void ToCurrentValue() { to = value.eulerAngles; }
        public override void FromCurrentValue() { from = value.eulerAngles; }

        /// <summary>
        /// Create a TweenRot Component, and starts a tween.
        /// </summary>
        /// <param name="go">GameObject to Apply the tween too</param>
        /// <param name="duration">How long the tween will take</param>
        /// <param name="rot">The Fianl Value at the end of the tween</param>
        /// <param name="method">The tweening method</parm>
        /// <param name="finished">The method execute at the end of the tween</param>
        /// <returns>Reference to the TweenRot component</returns>
        static public TweenRot Tween(GameObject go, float duration, Quaternion rot,
            Style style = Style.Once, Method method = Method.Linear, UnityAction finished = null)
        {
            TweenRot cls = TweenMain.Tween<TweenRot>(go, duration, style, method, finished);
            cls.from = cls.value.eulerAngles;
            cls.to = rot.eulerAngles;
            cls.Start();
            return cls;
        }
    }
}
