﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YamlDotNet.Serialization;
namespace Mugen3D.Tools
{
    public class ActionsEditor : MonoBehaviour
    {
        public string characterName;
        public Transform playerRoot;
        public ActionsEditorView view;
        public ActionsEditorModule module;
        public ActionsEditorController controller;

        private string prefix;
        private CharacterConfig config;
        const string resourePath = "Assets/Resources/";

        public void Awake()
        {
            prefix = "Chars/" + characterName;
            config = ConfigReader.Read<CharacterConfig>(ResourceLoader.LoadText(prefix + "/" + characterName + ".def"));

            UnityEngine.Object prefab = ResourceLoader.Load(prefix + config.modelFile);

            GameObject go = GameObject.Instantiate(prefab, playerRoot) as GameObject;
            go.transform.position = Vector3.zero;

            ActionsConfig actionsConfig = ConfigReader.Read<ActionsConfig>(ResourceLoader.LoadText(prefix + config.actionConfigFile));
            if (actionsConfig == null)
                actionsConfig = new ActionsConfig();
            module = new ActionsEditorModule(actionsConfig.actions);
            module.doSave = Save;
            controller = ActionsEditorController.Instance;
            controller.module = module;
            controller.view = view;
            var anim = go.GetComponent<Animation>();
            ActionsEditorAnimController animCtl = new ActionsEditorAnimController(anim);
            view.Init(module, animCtl);
            
        }

        public void Save()
        {
            YamlDotNet.Serialization.Serializer serializer = new Serializer();
            StringWriter strWriter = new StringWriter();
            ActionsConfig actionConfig = new ActionsConfig();
            actionConfig.actions = module.actions;
            foreach (var action in actionConfig.actions)
            {
                action.CalculateAnimLength();
            }
            serializer.Serialize(strWriter, actionConfig);

            using (TextWriter writer = File.CreateText(resourePath + prefix + config.actionConfigFile + ".txt"))
            {
                writer.Write(strWriter.ToString());
            }
            print("save success");
        }
        
    }
}
