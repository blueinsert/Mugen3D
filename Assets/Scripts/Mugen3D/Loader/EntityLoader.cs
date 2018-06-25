﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D
{
    public class EntityLoader
    {
        public static Unit curUnit;
        private static int maxId = 0;

        public static Character LoadPlayer(int slot, string characterName, Transform parent)
        {
            string prefix = "Chars/" + characterName;
            CharacterConfig config = ConfigReader.Read<CharacterConfig>(ResourceLoader.LoadText(prefix + "/" + characterName + ".def"));

            UnityEngine.Object prefab = ResourceLoader.Load(prefix + config.modelFile);

            GameObject go = GameObject.Instantiate(prefab, parent) as GameObject;

            Character p = go.AddComponent<Character>();
            curUnit = p;
            p.slot = slot;
            p.id = maxId++;
            p.Init(characterName, config);
            curUnit = null;
        
            World.Instance.AddEntity(p);
            return p;
        }

        /*
        public static Helper LoadHelper(string helperName, Character master, Transform parent)
        {
            UnityEngine.Object prefab = Resources.Load<UnityEngine.Object>("Helpers/" + helperName + "/" + helperName);
            GameObject go = GameObject.Instantiate(prefab, parent) as GameObject;
            Helper helper = go.GetComponentInChildren<Helper>();
            helper.master = master;
            helper.Init();
            World.Instance.AddEntity(helper);
            return helper;
        }
         */
    }
}
