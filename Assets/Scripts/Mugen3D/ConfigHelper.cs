using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D.Core;

namespace Mugen3D
{
    public class ConfigHelper
    {
        public static CharacterConfig ReadCharacterConfig(string characterName)
        {
            CharacterConfig config = ConfigReader.Read<CharacterConfig>(ResourceLoader.LoadText("Config/Chars/" + characterName + "/" + characterName + ".def"));
            ActionsConfig actionsConfig = ConfigReader.Read<ActionsConfig>(ResourceLoader.LoadText(config.action));
            string commands = ResourceLoader.LoadText(config.command);
            config.SetActions(actionsConfig.actions.ToArray());
            config.SetCommand(commands);
            return config;
        }
        
    }
}
