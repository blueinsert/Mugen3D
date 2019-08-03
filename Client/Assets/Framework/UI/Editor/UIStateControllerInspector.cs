using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace bluebean.UGFramework
{
    [CustomEditor(typeof(UIStateController))]
    public class UIStateControllerInspector : Editor
    {
        private SerializedObject m_editTarget;
        private UIStateController m_uiStateController;

        public virtual void OnEnable()
        {
            m_uiStateController = target as UIStateController;
            m_editTarget = new SerializedObject(m_uiStateController);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Label(string.Format("Current State:{0}", m_uiStateController.CurState));
            if (GUILayout.Button("SwichToNextState"))
            {
                m_uiStateController.SwichToNextState();
            }
            if (GUILayout.Button("Save"))
            {
                m_uiStateController.CollectResources();
                m_editTarget.ApplyModifiedProperties();
            }
        }
    }
}
