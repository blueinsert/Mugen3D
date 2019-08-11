using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    public class CommandEngine : BaseEngine
    {
        private List<CmdManager> cmdMgrs = new List<CmdManager>();

       public CommandEngine(BattleWorld world):base(world)
       {

       }

        protected override void OnAddEntity(Entity e)
        {
            if(e is Character)
            {
                var c = e as Character;
                this.cmdMgrs.Add(c.cmdMgr);
            }
        }

        protected override void OnRemoveEntity(Entity e)
        {
            if (e is Character)
            {
                var c = e as Character;
                this.cmdMgrs.Remove(c.cmdMgr);
            }
        }

        public override void Update()
        {
            foreach(var cmdMgr in this.cmdMgrs)
            {
                cmdMgr.Update(cmdMgr.owner.input);
            }
        }

    }
}
