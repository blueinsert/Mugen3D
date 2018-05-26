using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
                animCtl.Play(this.module.curAction);
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
                int index = (int)(value * (step - 1)) + 1;
                this.module.curActionIndex = index;
                UpdateUI();
            });  
            dropdownAnimName.onValueChanged.AddListener((animNameIndex) =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.module.curAction.animName = animNames[animNameIndex];
                UpdateUI();
            });
            labelAnimNo.onValueChanged.AddListener((animNo) =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.module.curAction.animNo = int.Parse(animNo);
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
                int index = (int)(value * (step - 1)) + 1;
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

            labelNormalizedTime.onValueChanged.AddListener((normalizedTime) =>
            {
                if (!isResponseToUIEvent)
                    return;
                sliderNormalizedTime.value = float.Parse(normalizedTime);
                UpdateUI();
            });
            sliderNormalizedTime.onValueChanged.AddListener((normalizedTime) => {
                if (!isResponseToUIEvent)
                    return;
                this.module.curActionElem.normalizeTime = normalizedTime;   
                UpdateUI();
            });
            labelDurationTime.onValueChanged.AddListener((duration) =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.module.curActionElem.duration = int.Parse(duration);
            });
            labelXOffset.onValueChanged.AddListener((xoffset) =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.module.curActionElem.xOffset = float.Parse(xoffset);
                UpdateUI();
            });
            labelYOffset.onValueChanged.AddListener((yoffset) =>
            {
                if (!isResponseToUIEvent)
                    return;
                this.module.curActionElem.yOffset = float.Parse(yoffset);
                UpdateUI();
            });

            toggleLoopStart.onValueChanged.AddListener((value) =>
            {
                if (!isResponseToUIEvent)
                    return;
                if (value == true)
                {
                    this.module.curAction.loopStartIndex = this.module.curActionElemIndex - 1;
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

        void ResizeScrollAction()
        {
            this.scrollActionList.numberOfSteps = this.module.actionLength;
            this.scrollActionList.size = 1 / (float)(this.module.actionLength);
        }

        void SetScrollActionPos()
        {
            if (this.module.curAction.frames.Count > 1)
                this.scrollActionElemList.value = (this.module.curActionElemIndex - 1) / (float)(this.module.curAction.frames.Count - 1);
            else
                this.scrollActionElemList.value = 0;
        }

        void ResizeScrollActionElem()
        {
            this.scrollActionElemList.numberOfSteps = this.module.curAction.frames.Count;
            this.scrollActionElemList.size = 1 / (float)(this.module.curAction.frames.Count);
        }

        void SetAcrollActionElemPos()
        {
            if (this.module.actionLength > 1)
                this.scrollActionList.value = (this.module.curActionIndex - 1) / (float)(this.module.actionLength - 1);
            else
                this.scrollActionList.value = 0;
        }

        void UpdateClsns()
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
            foreach (var clsn in this.module.curActionElem.clsns)
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

        void UpdateActionElem()
        {
            if (this.module.actionLength > 0 && this.module.curAction.frames.Count > 0)
            {
                ResizeScrollActionElem();
                SetAcrollActionElemPos();
                this.labelCurActionElemNo.text = this.module.curActionElemIndex + "/" + this.module.curAction.frames.Count + "-" + this.module.curActionElem.duration + "ticks";
                this.labelNormalizedTime.text = this.module.curActionElem.normalizeTime.ToString();
                this.sliderNormalizedTime.value = this.module.curActionElem.normalizeTime;
                this.labelDurationTime.text = this.module.curActionElem.duration.ToString();
                this.labelXOffset.text = this.module.curActionElem.xOffset.ToString();
                this.labelYOffset.text = this.module.curActionElem.yOffset.ToString();
                this.toggleLoopStart.isOn = (this.module.curActionElemIndex - 1) == this.module.curAction.loopStartIndex;
                animCtl.Sample(this.module.curAction.animName, this.module.curActionElem.normalizeTime);
                UpdateClsns();        
            }
            else
            {
                this.labelCurActionElemNo.text = "0/0";
                this.scrollActionElemList.numberOfSteps = 0;
            }
        }

        void UpdateAction()
        {
            if (this.module.actionLength > 0)
            {
                ResizeScrollAction();
                SetScrollActionPos();
                this.dropdownAnimName.value = animNames.IndexOf(this.module.curAction.animName);
                this.labelAnimNo.text = this.module.curAction.animNo.ToString();
                this.labelCurActionNo.text = this.module.curActionIndex + "/" + this.module.actionLength;
            }
            else
            {
                this.labelCurActionNo.text = "0/0";
                this.scrollActionList.numberOfSteps = 0;
            }
        }

        public void UpdateUI()
        {
            isResponseToUIEvent = false;
            UpdateAction();
            UpdateActionElem();
            isResponseToUIEvent = true;
        }
    }
}
