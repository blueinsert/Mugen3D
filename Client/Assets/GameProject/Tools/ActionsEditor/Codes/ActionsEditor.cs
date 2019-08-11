using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using YamlDotNet.Serialization;
using bluebean.Mugen3D.Core;

namespace Mugen3D.Tools
{
    [RequireComponent(typeof(ActionsEditorView))]
    public class ActionsEditor : MonoBehaviour
    {
        public ActionsEditorModule module;
        public ActionsEditorView view;
        public ActionsEditorController controller;
        public static ActionsEditor Instance;

        public void Awake()
        {
            module = new ActionsEditorModule();
            controller = this.GetComponent<ActionsEditorController>();
            controller.SetModule(module);
            view = this.GetComponent<ActionsEditorView>();
            view.SetController(controller);
            Instance = this;
        }
        
    }
}
