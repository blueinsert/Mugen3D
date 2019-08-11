﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using bluebean.Mugen3D.Core;

namespace bluebean.Mugen3D.ClientGame
{
    public class ConfigHelper
    {
        public static CharacterConfig ReadCharacterConfig(string characterName)
        {
            CharacterConfig config = ConfigReader.Parse<CharacterConfig>(ResourceLoader.LoadText("Chars/" + characterName + "/" + characterName + ".def"));
            ActionsConfig actionsConfig = ConfigReader.Parse<ActionsConfig>(ResourceLoader.LoadText(config.action));
            string commands = ResourceLoader.LoadText(config.command);
            config.SetActions(actionsConfig.actions.ToArray());
            config.SetCommand(commands);
            return config;
        }
        
    }
}