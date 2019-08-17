using System.Collections;
using System.Collections.Generic;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    //collsion
    public class Clsn
    {
        public int type { get; set; }
        public Number x1 { get; set; }
        public Number y1 { get; set; }
        public Number x2 { get; set; }
        public Number y2 { get; set; }

        public Clsn(int type, Number x1, Number y1, Number x2, Number y2)
        {
            this.type = type;
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }

        public Clsn(Clsn clsn)
        {
            this.type = clsn.type;
            this.x1 = clsn.x1;
            this.y1 = clsn.y1;
            this.x2 = clsn.x2;
            this.y2 = clsn.y2;
        }

        public Clsn()
        {

        }
    }

    public class ActionsConfig
    {
        public List<Core.ActionDef> actions { get; set; }
    }

    public class EntityConfig
    {
        public string name { get; set; }
    }

    public class UnitConfig : EntityConfig
    {
        public string prefab { get; set; }
        public string action { get; set; }
        public string fsm { get; set; }

        public ActionDef[] actions { get; private set; }

        public UnitConfig()
        {
        }

        public void SetActions(ActionDef[] actions)
        {
            this.actions = actions;
        }
    }

    public class HelperConfig : UnitConfig
    {

    }

    public class ProjectileConfig : HelperConfig {
    }

    public class CharacterConfig: UnitConfig
    {
        public string characterName { get; set; }
        public string command { get; set; }

        public string commandContent { get; private set; }

        public void SetCommand(string content)
        {
            this.commandContent = content;
        }
    }

    public class PlayerConfig : CharacterConfig
    {
        public string playerName { get; set; }
    }
}
