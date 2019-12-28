using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using YamlDotNet.Serialization;
using bluebean.Mugen3D.Core;

namespace Mugen3D.Tools
{
    public class ActionsEditorController : MonoBehaviour
    {
        private ActionsEditorModule m_module;
        public ActionsEditorModule module { get { return m_module; } }

        public void SetModule(ActionsEditorModule module) {
            this.m_module = module;
        }

        private CharacterConfig m_characterConfig;

        public bool Load()
        {
            bool res = false;
            string filePath = EditorUtility.OpenFilePanel("Choose Character Def", "Assets/Resources/Chars", "def.txt");
            if (string.IsNullOrEmpty(filePath) == false)
            {
                TextReader reader = File.OpenText(filePath);
                var deserializer = new Deserializer();
                this.m_characterConfig = deserializer.Deserialize<CharacterConfig>(reader.ReadToEnd());
                if (this.m_characterConfig != null)
                {
                    UnityEngine.Object prefab = null;//todo bluebean.Mugen3D.ClientGame.ResourceLoader.Load(m_characterConfig.prefab);
                    GameObject go = GameObject.Instantiate(prefab, this.transform.Find("Scene/Player")) as GameObject;
                    go.AddComponent<AnimationController>();
                    go.transform.position = Vector3.zero;
                    ActionsConfig actionsConfig = null;//todo ConfigReader.Parse<ActionsConfig>(bluebean.Mugen3D.ClientGame.ResourceLoader.LoadText(m_characterConfig.action));
                    if (actionsConfig == null)
                        actionsConfig = new ActionsConfig();
                    this.module.Init(actionsConfig.actions);
                    res = true;
                }
            }
            return res;
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
            using (TextWriter writer = File.CreateText("Assets/Resources/" + m_characterConfig.action + ".txt"))
            {
                writer.Write(strWriter.ToString());
            }
            print("save success");
        }
        
    }
}
