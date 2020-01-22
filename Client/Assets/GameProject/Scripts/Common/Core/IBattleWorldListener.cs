using System.Collections;
using System.Collections.Generic;
using bluebean.UGFramework.ConfigData;

namespace bluebean.Mugen3D.Core
{
    public interface IBattleWorldListener
    {
        /// <summary>
        /// 比赛开始
        /// </summary>
        /// <param name="matchNo"></param>
        void OnMatchStart(int matchNo);

        /// <summary>
        /// 比赛结束
        /// </summary>
        /// <param name="matchNo"></param>
        void OnMatchEnd(int matchNo);

        /// <summary>
        /// 回合开始
        /// </summary>
        /// <param name="roundNo"></param>
        void OnRoundStart(int roundNo);

        /// <summary>
        /// 回合结束
        /// </summary>
        /// <param name="roundNo"></param>
        void OnRoundEnd(int roundNo);

        /// <summary>
        /// 创建人物
        /// </summary>
        /// <param name="character"></param>
        void OnCreateCharacter(Entity character);

        /// <summary>
        /// 销毁人物
        /// </summary>
        /// <param name="character"></param>
        void OnDestroyCharacter(Entity character);

        /// <summary>
        /// 创建摄像机
        /// </summary>
        /// <param name="cameraComponent"></param>
        void OnCameraCreate(CameraComponent cameraComponent);

        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="soundName"></param>
        void OnPlaySound(string soundName);

        void OnCreateStage(string prefab);
        //void OnUpdateAnim()
    }
}
