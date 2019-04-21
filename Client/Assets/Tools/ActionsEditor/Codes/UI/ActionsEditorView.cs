using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Mugen3D.Core;

namespace Mugen3D.Tools
{
    public class ActionsEditorView : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private List<string> animNames = new List<string>();
        private bool isResponseToUIEvent = true;
        private ActionsEditorController controller;
        private AnimController m_animController;
        public AnimController animController
        {
            get
            {
                if (m_animController == null)
                {
                    m_animController = this.GetComponentInChildren<AnimController>();
                }
                return m_animController;
            }
        }
        public CameraController cameraController;
        public Camera sceneCamera;
        public Camera uiCamera;
        public Canvas canvas;
        public Button btnLoad;
        public Button btnSave;
        public Button btnPlay;
        public Button btnPause;
        public Button btnDeleAction;
        public Button btnCreateAction;
        public Button btnGoLeftAction;
        public Button btnGoRightAction;
        public Scrollbar scrollActionList;
        public Text labelCurActionNo;
        public Dropdown dropdownAnimName;
        public InputField labelAnimNo;
        public Button btnDeleActionElem;
        public Button btnCreateActionElem;
        public Button btnGoLeftActionElem;
        public Button btnGoRightActionElem;
        public Scrollbar scrollActionElemList;
        public Text labelCurActionElemNo;
        public InputField labelNormalizedTime;
        public Slider sliderNormalizedTime;
        public InputField labelDurationTime;
        public InputField labelXOffset;
        public InputField labelYOffset;
        public Toggle toggleLoopStart;
        public Transform rootClsns;
        public GameObject prefabClsn;
        public Button btnAddClsn1;
        public Button btnAddClsn2;
        public Button btnAddClsn3;
        public Button btnDeleteClsn;
        public Button btnUseLastClsn;
        public Clsn curSelectedClsn;

        void Awake()
        {
            RegisterEventInternal();
        }

        public Vector3 ScenePosToUIPos(Vector3 wPos)
        {
            Vector3 screenPos = sceneCamera.WorldToScreenPoint(wPos);
            screenPos.z =  canvas.planeDistance;
            var uiPos = canvas.worldCamera.ScreenToWorldPoint(screenPos);
            return uiPos;
        }

        public Vector3 UIPosToScenePos(Vector3 uiPos)
        {
            var screenPos = canvas.worldCamera.WorldToScreenPoint(uiPos);
            var wPos =sceneCamera.ScreenToWorldPoint(screenPos);
            return wPos;
        }

        //return ui rectTransform size unit / world size unit
        public float GetUISceneLenRadio()
        {
            if (sceneCamera.orthographic)
            {
                return 768.0f / (sceneCamera.orthographicSize * 2);
            }
            else
            {
                return 1;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                cameraController.ZoomOut();
            }
            //Zoom in
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                cameraController.ZoomIn();
            }   
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            cameraController.BeginDrag(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            cameraController.OnDrag(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
        }

        private void Load()
        {
            if (controller.Load())
            {
                this.Init();
            }
        }

        private void RegisterEventInternal()
        {
            btnLoad.onClick.AddListener(() => {
                Load();
            });
            btnSave.onClick.AddListener(() =>
            {
                controller.Save();
            });
            btnPause.onClick.AddListener(() => {
                this.animController.Stop();
            });
            btnPlay.onClick.AddListener(() => {
                animController.Play(this.controller.module.curAction);
            });
            btnCreateAction.onClick.AddListener(() =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.controller.module.AddAction();
                UpdateUI();
            });
            btnDeleAction.onClick.AddListener(() =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.controller.module.DeleteAction();
                UpdateUI();
            });
            btnGoLeftAction.onClick.AddListener(() =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.controller.module.GoPrevAction();
                UpdateUI();
            });
            btnGoRightAction.onClick.AddListener(() =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.controller.module.GoNextAction();
                UpdateUI();
            });   
            scrollActionList.onValueChanged.AddListener((value) => {
                if (!isResponseToUIEvent)
                    return;
                var step = scrollActionList.numberOfSteps;
                int index = (int)(value * (step - 1));
                this.controller.module.SetActionIndex(index);
                UpdateUI();
            });  
            dropdownAnimName.onValueChanged.AddListener((animNameIndex) =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.controller.module.curAction.animName = animNames[animNameIndex];
                UpdateUI();
            });
            labelAnimNo.onValueChanged.AddListener((animNo) =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.controller.module.curAction.animNo = int.Parse(animNo);
            });
            btnGoLeftActionElem.onClick.AddListener(() =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.controller.module.GoPrevActionElem();
                UpdateUI();     
            });
            btnGoRightActionElem.onClick.AddListener(() =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.controller.module.GoNextActionElem();
                UpdateUI();
            });  
            scrollActionElemList.onValueChanged.AddListener((value) => {
                if (!isResponseToUIEvent)
                    return;
                var step = scrollActionElemList.numberOfSteps;
                int index = (int)(value * (step - 1));
                this.controller.module.SetActionElemIndex(index);
                UpdateUI();
            });           
            btnCreateActionElem.onClick.AddListener(() =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.controller.module.CreateActionElem();
                UpdateUI();
            });
            btnDeleActionElem.onClick.AddListener(() =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.controller.module.DeleteActionElem();
                UpdateUI();
            });
            labelNormalizedTime.onEndEdit.AddListener((normalizedTime) =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.controller.module.curActionFrame.normalizeTime = float.Parse(normalizedTime).ToNumber();
                UpdateUI();
            });
            sliderNormalizedTime.onValueChanged.AddListener((normalizedTime) => {
                if (!isResponseToUIEvent)
                    return;
                this.controller.module.curActionFrame.normalizeTime = normalizedTime.ToNumber();
                UpdateUI();
            });
            labelDurationTime.onValueChanged.AddListener((duration) =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.controller.module.curActionFrame.duration = int.Parse(duration);
            });
            labelXOffset.onValueChanged.AddListener((xoffset) =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.controller.module.curActionFrame.xOffset = float.Parse(xoffset).ToNumber();
                UpdateUI();
            });
            labelYOffset.onValueChanged.AddListener((yoffset) =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.controller.module.curActionFrame.yOffset = float.Parse(yoffset).ToNumber();
                UpdateUI();
            });
            toggleLoopStart.onValueChanged.AddListener((value) =>
            {
                if (!isResponseToUIEvent)
                    return;
                if (value == true)
                {
                    this.controller.module.curAction.loopStartIndex = this.controller.module.curActionElemIndex;
                }
            });
            btnAddClsn1.onClick.AddListener(() => {
                print("click addclsn1");
                if (!isResponseToUIEvent)
                    return;
                this.controller.module.CreateClsn(1);
                UpdateUI();
            });
            btnAddClsn2.onClick.AddListener(() =>
            {
                print("click addclsn2");
                if (!isResponseToUIEvent)
                    return;
                this.controller.module.CreateClsn(2);
                UpdateUI();
            });
            btnAddClsn3.onClick.AddListener(() =>
            {
                print("click addclsn3");
                if (!isResponseToUIEvent)
                    return;
                this.controller.module.CreateClsn(3);
                UpdateUI();
            });
            btnDeleteClsn.onClick.AddListener(() =>
            {
                if (!isResponseToUIEvent)
                    return;
                if (curSelectedClsn == null)
                    return;
                this.controller.module.DeleteClsn(curSelectedClsn);
                UpdateUI();
            });
            btnUseLastClsn.onClick.AddListener(() => {
                if (!isResponseToUIEvent)
                    return;
                this.controller.module.UseLastClsns();
                UpdateUI();
            });
        }

        public void SetController(ActionsEditorController controller)
        {
            this.controller = controller;
        }

        private void Init()
        {       
            animNames.Clear();
            foreach (AnimationState state in animController.anim)
            {
                animNames.Add(state.name);
            }
            dropdownAnimName.AddOptions(animNames);
            UpdateUI(); 
        }

        void UpdateClsns(ActionFrame frame)
        {
            if (rootClsns.childCount != 0)
            {
                List<GameObject> toDestroyList = new List<GameObject>();
                for (int i = 0; i < rootClsns.childCount; i++)
                {
                    var child = rootClsns.GetChild(i);
                    //GameObject.Destroy(child.gameObject);
                    toDestroyList.Add(child.gameObject);
                }
                foreach (var go in toDestroyList)
                {
                    GameObject.Destroy(go);
                }
                toDestroyList.Clear();
            }
            foreach (var clsn in frame.clsns)
            {
                var go = GameObject.Instantiate(prefabClsn, rootClsns);
                go.SetActive(true);
                WidgetCLSN wClsn = go.GetComponent<WidgetCLSN>();
                wClsn.toggle.onValueChanged.AddListener((v) => {
                    if (v)
                    {
                        this.curSelectedClsn = clsn;
                    }
                });
                wClsn.Init(clsn);
            }
        }

        public void UpdateUI()
        {
            isResponseToUIEvent = false;
            var module = this.controller.module;
            if (module.actions != null && module.actions.Count != 0)
            {
                var curAction = module.curAction;
                this.scrollActionList.numberOfSteps = module.actions.Count;
                this.scrollActionList.size = 1 / (float)(module.actions.Count);
                if (module.actions.Count > 1)
                    this.scrollActionList.value = module.curActionIndex / (float)(module.actions.Count - 1);
                else
                    this.scrollActionList.value = 0;
                this.dropdownAnimName.value = animNames.IndexOf(curAction.animName);
                this.labelAnimNo.text = curAction.animNo.ToString();
                this.labelCurActionNo.text = module.curActionIndex + "/" + module.actions.Count;
                if (curAction.frames != null && curAction.frames.Count != 0)
                {
                    var curActionElem = curAction.frames[module.curActionElemIndex];
                    this.scrollActionElemList.numberOfSteps = curAction.frames.Count;
                    this.scrollActionElemList.size = 1 / (float)(curAction.frames.Count);
                    if (curAction.frames.Count > 1)
                        this.scrollActionElemList.value = module.curActionElemIndex / (float)(curAction.frames.Count - 1);
                    else
                        this.scrollActionElemList.value = 0;
                    this.labelCurActionElemNo.text = module.curActionElemIndex + "/" + curAction.frames.Count + "-" + curActionElem.duration + "ticks";
                    this.labelNormalizedTime.text = curActionElem.normalizeTime.ToString();
                    this.sliderNormalizedTime.value = curActionElem.normalizeTime.AsFloat();
                    this.labelDurationTime.text = curActionElem.duration.ToString();
                    this.labelXOffset.text = curActionElem.xOffset.ToString();
                    this.labelYOffset.text = curActionElem.yOffset.ToString();
                    this.toggleLoopStart.isOn = module.curActionElemIndex == curAction.loopStartIndex;
                    this.animController.Sample(curAction.animName, curActionElem.normalizeTime.AsFloat());
                    UpdateClsns(curActionElem);
                }
                else
                {
                    this.labelCurActionElemNo.text = "0/0";
                    this.scrollActionElemList.numberOfSteps = 0;
                }
            }
            else
            {
                this.labelCurActionNo.text = "0/0";
                this.scrollActionList.numberOfSteps = 0;
            }
            isResponseToUIEvent = true;
        }
    }
}
