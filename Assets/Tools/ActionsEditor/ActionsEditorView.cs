﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mugen3D.Tools
{
    public class ActionsEditorView : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private ActionsEditorModule module;
        private ActionsEditorAnimController animCtl;
        public ActionsEditorCameraController cameraController;
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
        public InputField labelDurationTime;
        public InputField labelXOffset;
        public InputField labelYOffset;
        public Toggle toggleLoopStart;
        private List<string> animNames;

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
                this.module.AddAction();
                UpdateUI();
            });
            btnDeleAction.onClick.AddListener(() =>
            {
                this.module.DeleteAction();
                UpdateUI();
            });
            btnGoLeftAction.onClick.AddListener(() =>
            {
                this.module.GoPrevAction();
                UpdateUI();
            });
            btnGoRightAction.onClick.AddListener(() =>
            {
                this.module.GoNextAction();
                UpdateUI();
            });
  
            dropdownAnimName.onValueChanged.AddListener((animNameIndex) =>
            {
                this.module.curAction.animName = animNames[animNameIndex];
                UpdateUI();
            });
            labelAnimNo.onValueChanged.AddListener((animNo) =>
            {
                this.module.curAction.animNo = int.Parse(animNo);
            });

            btnGoLeftActionElem.onClick.AddListener(() =>
            {
                this.module.GoPrevActionElem();
                UpdateUI();
            });
            btnGoRightActionElem.onClick.AddListener(() =>
            {
                this.module.GoNextActionElem();
                UpdateUI();
            });
            btnCreateActionElem.onClick.AddListener(() =>
            {
                this.module.CreateActionElem();
                UpdateUI();
            });
            btnDeleActionElem.onClick.AddListener(() =>
            {
                this.module.DeleteActionElem();
                UpdateUI();
            });

            labelNormalizedTime.onValueChanged.AddListener((normalizedTime) =>
            {
                this.module.curActionElem.normalizeTime = float.Parse(normalizedTime);
                UpdateUI();
            });
            labelDurationTime.onValueChanged.AddListener((duration) =>
            {
                this.module.curActionElem.duration = int.Parse(duration);
                UpdateUI();
            });
            labelXOffset.onValueChanged.AddListener((xoffset) =>
            {
                this.module.curActionElem.xOffset = float.Parse(xoffset);
                UpdateUI();
            });
            labelYOffset.onValueChanged.AddListener((yoffset) =>
            {
                this.module.curActionElem.yOffset = float.Parse(yoffset);
                UpdateUI();
            });

            toggleLoopStart.onValueChanged.AddListener((value) =>
            {
                if (value == true)
                {
                    this.module.curAction.loopStartIndex = this.module.curActionElemIndex - 1;
                }
            });
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
            RegisterEventInternal();
            this.module = module;
            UpdateUI();
        }

        public void UpdateUI()
        {
            if (this.module.actionLength > 0)
            {
                this.scrollActionList.numberOfSteps = this.module.actionLength;
                this.scrollActionList.size = 1 / (float)(this.module.actionLength);
                if (this.module.actionLength > 1)
                    this.scrollActionList.value = (this.module.curActionIndex - 1) / (float)(this.module.actionLength - 1);
                else
                    this.scrollActionList.value = 0;
                //this.scrollActionList
                this.dropdownAnimName.value = animNames.IndexOf(this.module.curAction.animName);
                this.labelAnimNo.text = this.module.curAction.animNo.ToString();
                this.labelCurActionNo.text = this.module.curActionIndex + "/" + this.module.actionLength;
                if (this.module.curAction.frames.Count > 0)
                {
                    this.scrollActionElemList.numberOfSteps = this.module.curAction.frames.Count;
                    this.scrollActionElemList.size = 1 / (float)(this.module.curAction.frames.Count);
                    if (this.module.curAction.frames.Count > 1)
                        this.scrollActionElemList.value = (this.module.curActionElemIndex - 1) / (float)(this.module.curAction.frames.Count - 1);
                    else
                        this.scrollActionElemList.value = 0;

                    this.labelCurActionElemNo.text = this.module.curActionElemIndex + "/" + this.module.curAction.frames.Count + "-" + this.module.curActionElem.duration + "ticks";
                    this.labelNormalizedTime.text = this.module.curActionElem.normalizeTime.ToString();
                    this.labelDurationTime.text = this.module.curActionElem.duration.ToString();
                    this.labelXOffset.text = this.module.curActionElem.xOffset.ToString();
                    this.labelYOffset.text = this.module.curActionElem.yOffset.ToString();
                    this.toggleLoopStart.isOn = (this.module.curActionElemIndex - 1) == this.module.curAction.loopStartIndex;
                    animCtl.Sample(this.module.curAction.animName, this.module.curActionElem.normalizeTime);
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

        }
    }
}
