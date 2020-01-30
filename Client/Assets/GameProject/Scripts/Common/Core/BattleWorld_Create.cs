using bluebean.UGFramework.ConfigData;
using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    public partial class BattleWorld : WorldBase
    {
        /// <summary>
        /// 创建一个角色
        /// </summary>
        /// <param name="configDataCharacter"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private Entity AddCharacter(ConfigDataCharacter configDataCharacter, int index)
        {
            var entity = AddEntity();
            entity.AddComponent<BasicInfoComponent>().Init(index, configDataCharacter);
            entity.AddComponent<TransformComponent>();
            entity.AddComponent<MoveComponent>();
            entity.AddComponent<CommandComponent>().Init();
            var actionDefStr = sFileReader(configDataCharacter.ActionDef);
            //todo 反序列化速度有点慢；和configDataLoader统一
            var acttionDefs = ConfigReader.Parse<List<ActionDef>>(actionDefStr);
            entity.AddComponent<AnimationComponent>().Init(acttionDefs.ToArray());
            entity.AddComponent<FSMComponent>().Initialize(entity);
            entity.AddComponent<PhysicsComponent>();
            entity.AddComponent<CollideComponent>();
            entity.AddComponent<HitComponent>();
            m_characterArray[index] = entity;
            //entity.AddComponent<DelayImpactComponent>();
            m_listener.OnCreateCharacter(entity);
            return entity;
        }
    }
}
