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
        private List<string> animNames;
        private ActionsEditorModule module;
        private ActionsEditorAnimController animCtl;
        public ActionsEditorCameraController cameraController;
        private bool isResponseToUIEvent = true;

        public Camera sceneCamera;
        public Camera uiCamera;
        public Canvas cancas;
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
        public Button btnDeleteClsn;
        public Button btnUseLastClsn;
        public Clsn curSelectedClsn;

        // Use this for initialization
        void Start()
        {

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
            if (animCtl != null)
            {
                animCtl.Update();
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

        private void RegisterEventInternal()
        {
            btnSave.onClick.AddListener(() =>
            {
                this.module.Save();
            });
            btnPause.onClick.AddListener(() => {
                animCtl.Stop();
            });
            btnPlay.onClick.AddListener(() => {
                animCtl.Play(this.module.actions[this.module.curActionIndex]);
            });

            btnCreateAction.onClick.AddListener(() =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.module.AddAction();
                UpdateUI();
            });
            btnDeleAction.onClick.AddListener(() =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.module.DeleteAction();
                UpdateUI();
            });
            btnGoLeftAction.onClick.AddListener(() =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.module.GoPrevAction();
                UpdateUI();
            });
            btnGoRightAction.onClick.AddListener(() =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.module.GoNextAction();
                UpdateUI();
            });
         
            
            scrollActionList.onValueChanged.AddListener((value) => {
                if (!isResponseToUIEvent)
                    return;
                var step = scrollActionList.numberOfSteps;
                int index = (int)(value * (step - 1));
                this.module.curActionIndex = index;
                UpdateUI();
            });  
            dropdownAnimName.onValueChanged.AddListener((animNameIndex) =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.module.actions[this.module.curActionIndex].animName = animNames[animNameIndex];
                UpdateUI();
            });
            labelAnimNo.onValueChanged.AddListener((animNo) =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.module.actions[this.module.curActionIndex].animNo = int.Parse(animNo);
            });

            btnGoLeftActionElem.onClick.AddListener(() =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.module.GoPrevActionElem();
                UpdateUI();
                
            });
            btnGoRightActionElem.onClick.AddListener(() =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.module.GoNextActionElem();
                UpdateUI();
            });
            
            scrollActionElemList.onValueChanged.AddListener((value) => {
                if (!isResponseToUIEvent)
                    return;
                var step = scrollActionElemList.numberOfSteps;
                int index = (int)(value * (step - 1));
                this.module.curActionElemIndex = index;
                UpdateUI();
            });
            
            btnCreateActionElem.onClick.AddListener(() =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.module.CreateActionElem();
                UpdateUI();
            });
            btnDeleActionElem.onClick.AddListener(() =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.module.DeleteActionElem();
                UpdateUI();
            });

            labelNormalizedTime.onEndEdit.AddListener((normalizedTime) =>
            {
                if (!isResponseToUIEvent)
                    return;
                //UnityEngine.Debug.Log(normalizedTime);
                this.module.actions[this.module.curActionIndex].frames[this.module.curActionElemIndex].normalizeTime = float.Parse(normalizedTime).ToNumber();
                UpdateUI();
            });
            sliderNormalizedTime.onValueChanged.AddListener((normalizedTime) => {
                if (!isResponseToUIEvent)
                    return;
                this.module.actions[this.module.curActionIndex].frames[this.module.curActionElemIndex].normalizeTime = normalizedTime.ToNumber();
                UpdateUI();
            });
            labelDurationTime.onValueChanged.AddListener((duration) =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.module.actions[this.module.curActionIndex].frames[this.module.curActionElemIndex].duration = int.Parse(duration);
            });
            labelXOffset.onValueChanged.AddListener((xoffset) =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.module.actions[this.module.curActionIndex].frames[this.module.curActionElemIndex].xOffset = float.Parse(xoffset).ToNumber();
                UpdateUI();
            });
            labelYOffset.onValueChanged.AddListener((yoffset) =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.module.actions[this.module.curActionIndex].frames[this.module.curActionElemIndex].yOffset = float.Parse(yoffset).ToNumber();
                UpdateUI();
            });

            toggleLoopStart.onValueChanged.AddListener((value) =>
            {
                if (!isResponseToUIEvent)
                    return;
                if (value == true)
                {
                    this.module.actions[this.module.curActionIndex].loopStartIndex = this.module.curActionElemIndex;
                }
            });

            btnAddClsn1.onClick.AddListener(() => {
                print("click addclsn1");
                if (!isResponseToUIEvent)
                    return;
                this.module.CreateClsn(1);
                UpdateUI();
            });
            btnAddClsn2.onClick.AddListener(() =>
            {
                print("click addclsn1");
                if (!isResponseToUIEvent)
                    return;
                this.module.CreateClsn(2);
                UpdateUI();
            });
            btnDeleteClsn.onClick.AddListener(() =>
            {
                if (!isResponseToUIEvent)
                    return;
                if (curSelectedClsn == null)
                    return;
                this.module.DeleteClsn(curSelectedClsn);
                UpdateUI();
            });
            btnUseLastClsn.onClick.AddListener(() => {
                if (!isResponseToUIEvent)
                    return;
                this.module.UseLastClsns();
                UpdateUI();
            });
        }

        public void OnDrawGizmos()
        {
            Vector3 v00 = new Vector3(-1,-1,0);
            Vector3 v01 = new Vector3(-1,1,0);
            Vector3 v02 = new Vector3(1, 1, 0);
            Vector3 v03 = new Vector3(1,-1,0);
            Gizmos.DrawLine(v00, v01);
            Gizmos.DrawLine(v01, v02);
            Gizmos.DrawLine(v02, v03);
            Gizmos.DrawLine(v03, v00);
        }

        

        public void Init(ActionsEditorModule module, ActionsEditorAnimController animCtl)
        {
            this.animCtl = animCtl;
            animNames = new List<string>();
            foreach (AnimationState state in animCtl.m_anim)
            {
                animNames.Add(state.name);
            }
            dropdownAnimName.AddOptions(animNames);
            this.module = module;
            UpdateUI();
            RegisterEventInternal();   
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
            if (this.module.actions != null && this.module.actions.Count != 0)
            {
                var curAction = this.module.actions[this.module.curActionIndex];
                this.scrollActionList.numberOfSteps = this.module.actions.Count;
                this.scrollActionList.size = 1 / (float)(this.module.actions.Count);
                if (this.module.actions.Count > 1)
                    this.scrollActionList.value = this.module.curActionIndex / (float)(this.module.actions.Count - 1);
                else
                    this.scrollActionList.value = 0;
                this.dropdownAnimName.value = animNames.IndexOf(curAction.animName);
                this.labelAnimNo.text = curAction.animNo.ToString();
                this.labelCurActionNo.text = this.module.curActionIndex + "/" + this.module.actions.Count;
                if (curAction.frames != null && curAction.frames.Count != 0)
                {
                    var curActionElem = curAction.frames[this.module.curActionElemIndex];
                    this.scrollActionElemList.numberOfSteps = curAction.frames.Count;
                    this.scrollActionElemList.size = 1 / (float)(curAction.frames.Count);
                    if (curAction.frames.Count > 1)
                        this.scrollActionElemList.value = this.module.curActionElemIndex / (float)(curAction.frames.Count - 1);
                    else
                        this.scrollActionElemList.value = 0;
                    this.labelCurActionElemNo.text = this.module.curActionElemIndex + "/" + curAction.frames.Count + "-" + curActionElem.duration + "ticks";
                    this.labelNormalizedTime.text = curActionElem.normalizeTime.ToString();
                    this.sliderNormalizedTime.value = curActionElem.normalizeTime.AsFloat();
                    this.labelDurationTime.text = curActionElem.duration.ToString();
                    this.labelXOffset.text = curActionElem.xOffset.ToString();
                    this.labelYOffset.text = curActionElem.yOffset.ToString();
                    this.toggleLoopStart.isOn = this.module.curActionElemIndex == curAction.loopStartIndex;
                    animCtl.Sample(curAction.animName, curActionElem.normalizeTime.AsFloat());
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
