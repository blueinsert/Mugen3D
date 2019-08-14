using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using bluebean.UGFramework;
using bluebean.UGFramework.ConfigData;

namespace bluebean.Mugen3D.ClientGame
{
    public class BattleSceneViewController : MonoViewController
    {
        protected override void OnBindFieldsComplete()
        {
            base.OnBindFieldsComplete();
        }

        public void CreateStage(ConfigDataStage configDataStage, IAssetProvider assetProvider)
        {
            var stagePrefab = assetProvider.GetAsset<GameObject>(configDataStage.Prefab);
            var go = GameObject.Instantiate(stagePrefab);
            go.transform.SetParent(StageRoot.transform, false);
        }

        #region AutoBind
        [AutoBind("./StageRoot")]
        public GameObject StageRoot;
        [AutoBind("./PlayerRoot")]
        public GameObject PlayerRoot;
        #endregion
    }
}
