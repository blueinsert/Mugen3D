using System.Collections;
using System.Collections.Generic;
namespace Mugen3D.Core
{
    public class EntityFactory
    {
        public static Character CreateCharacter(string name, int slot, bool isLocal)
        {
            CharacterConfig config = ConfigReader.Parse<CharacterConfig>(FileReader.Read("Chars/" + name + "/" + name + ".def"));
            ActionsConfig actionsConfig = ConfigReader.Parse<ActionsConfig>(FileReader.Read(config.action));
            string commands = FileReader.Read(config.command);
            config.SetActions(actionsConfig.actions.ToArray());
            config.SetCommand(commands);
            Character p = new Character(name, config, slot, isLocal);
            return p;
        }

        public static Helper CreateHelper(string name, Character owner)
        {
            HelperConfig config = ConfigReader.Parse<HelperConfig>(FileReader.Read("Helpers/" + name + "/" + name + ".def"));
            ActionsConfig actionsConfig = ConfigReader.Parse<ActionsConfig>(FileReader.Read(config.action));
            config.SetActions(actionsConfig.actions.ToArray());
            Helper helper = new Helper(config, owner);
            return helper;
        }
    }
}
