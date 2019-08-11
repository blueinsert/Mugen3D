using System.Collections;
using System.Collections.Generic;
using FixPointMath;

namespace bluebean.Mugen3D.Core
{
    public class ProjectileDef
    {
        /*
         Specifies the projectile priority. If the projectile collides with another projectile of equal priority, they will cancel. 
         If it collides with another of lower priority, it will cancel the lower- priority projectile, and the higher-priority one will have its priority decreased by 1. 
         Defaults to 1. 
         */
        public int projPriority;
        /*
         * Specifies the distance off the edge of the screen before the projectile is automatically removed.
         */
        public Number projEdgeBound;
        /*
         * Specifies the greatest distance the projectile can travel off the edge of the stage before being it is automatically removed. 
         */
        public Number projStageBound;
        /*
         Specifies the x and y offsets at which the projectile should be created. Both parameters default to 0 if omitted. 
         Projectiles are always created facing the same direction as the player. off_x is in relation to the direction the projectile is facing. 
         The exact behavior of the offset parameters is dependent on the postype. 
*/
        public Number[] offset;
        public string posType;
        public Number[] vel;
        public int facing;
        public int id;
    }

    public class Projectile : Helper
    {
        public ProjectileDef projDef { get; private set; }

        public Projectile(ProjectileDef def, ProjectileConfig config, Character owner):base(config, owner)
        {
            this.projDef = def;
            Init();
        }

        private void Init()
        {
            Vector pos = owner.position;
            switch (projDef.posType)
            {
                case "p1":
                    pos = owner.position + new Vector(projDef.offset.X()*owner.GetFacing(), projDef.offset.Y());
                    break;
                case "p2":
                    break;
                case "front":
                    break;
                case "back":
                    break;
                case "left":
                    break;
                case "right":
                    break;
            }
            this.moveCtr.PosSet(pos);
            this.ChangeFacing(projDef.facing);
            this.moveCtr.VelSet(projDef.vel.X(), projDef.vel.Y());
        }

        private bool AutoDestroyCheck() {
            var backEdgeDist = GetBackEdgeDist();
            var frontEdgeDist = GetFrontEdgeDist();
            if((backEdgeDist < 0 && Math.Abs(backEdgeDist) > projDef.projEdgeBound) ||
                (frontEdgeDist < 0 && Math.Abs(frontEdgeDist) > projDef.projEdgeBound))
            {
                return true;
            }
            var backStageDist = GetBackStageDist();
            var frontStageDist = GetFrontStageDist();
            if ((backStageDist < 0 && Math.Abs(backStageDist) > projDef.projStageBound) ||
                (frontStageDist < 0 && Math.Abs(frontStageDist) > projDef.projStageBound)) {
                return true;
            }
            return false;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (AutoDestroyCheck())
            {
                Destroy();
            }
        }

        public override void OnMoveGuarded(Unit target)
        {
            base.OnMoveGuarded(target);
        }

        public override void OnMoveHit(Unit target)
        {
            base.OnMoveHit(target);
        }
    }
}
