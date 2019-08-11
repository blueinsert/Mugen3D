using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UI
{
    public abstract class TweenMain : MonoBehaviour
    {
        static public TweenMain self;

        #region Enums
        public enum Method
        {
            Linear,
            EaseIn,
            EaseOut,
            EaseInOut,
            BounceIn,
            BounceOut
        }

        public enum Style
        {
            Once,
            Loop,
            PingPong
        }
        #endregion

        #region publicVars
        public UnityEvent OnFinished;

        public bool fromOffset = false;
        public bool toOffset = false;

        public Method method = Method.Linear;
        public AnimationCurve functionCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
        public Style style = Style.Once;
        public bool ignoreTimeScale = true;
        public float delay = 0f;
        public float duration = 1f;

        public float amountPerDelta
        {
            get
            {
                if (_Duration != duration)
                {
                    _Duration = duration;
                    _AmountPerDelta = Mathf.Abs((duration > 0f) ? 1f / duration : 1000f);
                }
                return _AmountPerDelta;
            }
        }

        public float tweenFactor { get { return _Factor; } set { _Factor = Mathf.Clamp01(value); } }
        #endregion

        #region components
        private Graphic _gfx;
        protected Graphic gfx
        {
            get
            {
                if (_gfx == null)
                    _gfx = GetComponent<Graphic>();
                return _gfx;
            }
        }

        private RectTransform _rect;
        protected RectTransform rect
        {
            get
            {
                if (_rect == null)
                    _rect = GetComponent<RectTransform>();
                return _rect;
            }
        }
        #endregion

        #region privateVars
        private bool _Started = false;
        private float
            _StartTime = 0f,
            _Duration = 0f,
            _AmountPerDelta = 1000f,
            _Factor = 0f;
        #endregion

        void Reset()
        {
            if (!_Started)
            {
                ToCurrentValue();
                FromCurrentValue();
            }
            Start();
        }

        void Awake()
        {
            if (OnFinished == null)
                OnFinished = new UnityEvent();
        }

        protected virtual void Start() { Update(); }
        void Update()
        {
            float delta = ignoreTimeScale ? UnScaledTime.deltaTime : Time.deltaTime;
            float time = ignoreTimeScale ? UnScaledTime.time : Time.time;
            if (!_Started)
            {
                _Started = true;
                _StartTime = time + delay;
            }

            if (time < _StartTime)
                return;

            _Factor += amountPerDelta * delta;

            if (style == Style.Loop)
            {
                if (_Factor > 1f)
                    _Factor -= Mathf.Floor(_Factor);
            }
            else if (style == Style.PingPong)
            {
                if (_Factor > 1f)
                {
                    _Factor = 1f - (_Factor - Mathf.Floor(_Factor));
                    _AmountPerDelta = -_AmountPerDelta;
                }
                else if (_Factor < 0f)
                {
                    _Factor = -_Factor;
                    _Factor -= Mathf.Floor(_Factor);
                    _AmountPerDelta = -_AmountPerDelta;
                }
            }

            if ((style == Style.Once) && (duration == 0f || _Factor > 1f || _Factor < 0f))
            {
                _Factor = Mathf.Clamp01(_Factor);
                Sample(_Factor, true);

                if (duration == 0f || (_Factor == 1f && _AmountPerDelta > 0f || _Factor == 0f && amountPerDelta < 0f))
                    enabled = false;

                //Event Callback stuff
                if (self == null)
                {
                    self = this;
                    if (OnFinished != null)
                        OnFinished.Invoke();
                }
                self = null;
            }
            else
                Sample(_Factor, false);
        }

        void OnDisable() { _Started = false; }

        protected void Sample(float factor, bool isFinished)
        {
            float val = Mathf.Clamp01(factor);

            switch (method)
            {
                case Method.Linear:
                    break;

                case Method.EaseIn:
                    val = 1f - Mathf.Sin(0.5f * Mathf.PI * (1f - val));
                    break;

                case Method.EaseOut:
                    val = 1f - Mathf.Sin(0.5f * Mathf.PI * val);
                    break;

                case Method.EaseInOut:
                    const float pi2 = Mathf.PI * 2;
                    val = val - Mathf.Sin(val * pi2) / pi2;
                    break;

                case Method.BounceIn:
                    val = Bounce(val);
                    break;

                case Method.BounceOut:
                    val = 1f - Bounce(1f - val);
                    break;
            }
            OnUpdate((functionCurve != null) ? functionCurve.Evaluate(val) : val, isFinished);
        }

        float Bounce(float val)
        {
            if (val < 0.363636f)
                val = 7.5685f * val * val;
            else if (val < 0.727272f)
                val = 7.5625f * (val -= 0.545454f) * val + 0.75f;
            else if (val < 0.909090f)
                val = 7.5625f * (val -= 0.818181f) * val + 0.9375f;
            else
                val = 7.5625f * (val -= 0.9545454f) * val + 0.984375f;

            return val;
        }

        #region PlayMethods
        /// <summary>
        /// Reset Tween to begining.
        /// </summary>
        public void ResetToBeginning()
        {
            _Started = false;
            _Factor = (_AmountPerDelta < 0f) ? 1f : 0f;
            Sample(_Factor, false);
            Start();
        }

        /// <summary>
        /// Plays Tween.
        /// </summary>
        public void Play() { PlayForward(); }

        /// <summary>
        /// Plays Tween.
        /// </summary>
        public void PlayForward()
        {
            _AmountPerDelta = Mathf.Abs(_AmountPerDelta);
            enabled = true;
            Start();
            Update();
        }

        /// <summary>
        /// Plays Tween in reverse.
        /// </summary>
        public void PlayReverse()
        {
            _AmountPerDelta = -Mathf.Abs(_AmountPerDelta);
            enabled = true;
            Start();
            Update();
        }

        /// <summary>
        /// Toogle play direction and play tween.
        /// </summary>
        public void Toggle()
        {
            if (_Factor > 0f)
                _AmountPerDelta = -amountPerDelta;
            else
                _AmountPerDelta = Mathf.Abs(amountPerDelta);
            enabled = true;
        }
        #endregion

        /// <summary>
        /// Interface for new tweeners to implemeant.
        /// </summary>
        /// <param name="factor"></param>
        /// <param name="isFinished"></param>
        abstract protected void OnUpdate(float factor, bool isFinished);
        
        /// <summary>
        /// Set "To" the current Value.
        /// </summary>
        public virtual void ToCurrentValue() { }
        /// <summary>
        /// Set "From" the current Value.
        /// </summary>
        public virtual void FromCurrentValue() { }

        protected static T Tween<T>(GameObject go, float duration, Style style,
            Method method, UnityAction finished = null)  where T : TweenMain
        {
            T cls = go.GetComponent<T>();
            if (cls == null)
                cls = go.AddComponent<T>();

            if (finished != null)
                cls.OnFinished.AddListener(finished);

            cls._Started = false;
            cls.duration = duration;
            cls.method = method;
            cls.style = style;
            cls._Factor = 0f;
            cls._AmountPerDelta = Mathf.Abs(cls._AmountPerDelta);
            cls.functionCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
            cls.enabled = true;

            if (duration <= 0f)
            {
                cls.Sample(1f, true);
                cls.enabled = false;
            }

            return cls;
        }
    }
}
