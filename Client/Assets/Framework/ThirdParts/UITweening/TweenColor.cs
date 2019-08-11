using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

namespace UnityEngine.UI
{
    [RequireComponent(typeof(Graphic))]
    [AddComponentMenu("UI/Tweening/Color")]
    public class TweenColor : TweenMain
    {
        public Color from = Color.black;
        public Color to = Color.black;

        public Color value
        {
            get { return gfx.color; }
            set { gfx.color = value; }
        }

        protected override void OnUpdate(float factor, bool isFinished)
        {
            value = Color.Lerp(from, to, factor);
        }

        public override void ToCurrentValue() { to = value; }
        public override void FromCurrentValue() { from = value; }

        /// <summary>
        /// Create a TweenColor Component, and starts a tween.
        /// </summary>
        /// <param name="go">GameObject to Apply the tween too</param>
        /// <param name="duration">How long the tween will take</param>
        /// <param name="color">The final Value at the end of the tween</param>
        /// <param name="method">The tweening method</parm>
        /// <param name="finished">The method execute at the end of the tween</param>
        /// <returns>Reference to the TweenColor component</returns>
        public static TweenColor Tween(GameObject go, float duration, Color color,
            Style style = Style.Once, Method method = Method.Linear, UnityAction finished = null)
        {
            TweenColor cls = TweenMain.Tween<TweenColor>(go, duration, style, method, finished);

            cls.from = cls.value;
            cls.to = color;
            cls.Start();
            return cls;
        }
    }
}
