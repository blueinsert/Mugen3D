using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
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
        public Vector offset;
        public string posType;
    }

    public class Projectile : Helper
    {
        public ProjectileDef projDef { get; private set; }
        public Projectile(ProjectileDef def, ProjectileConfig config, Character owner):base(config, owner)
        {
            this.projDef = def;
        }

        private bool AutoDestroyCheck() {
            return false;
        }

        public override void OnUpdate(Number deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (AutoDestroyCheck())
            {
                Destroy();
            }
        }

        public override void OnMoveGuarded(Unit target)
        {
            Destroy();
        }

        public override void OnMoveHit(Unit target)
        {
            Destroy();
        }
    }
}
