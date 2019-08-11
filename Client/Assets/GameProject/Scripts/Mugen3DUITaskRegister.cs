using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using bluebean.UGFramework;
using bluebean.UGFramework.UI;
using Debug = bluebean.UGFramework.Log.Debug;

namespace bluebean.Mugen3D
{
    public class Mugen3DUITaskRegister : MonoBehaviour
    {
        public static bool RegisterUITasks()
        {
            var gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                Debug.LogError("Mugen3DUITaskRegister:RegisterUITasks gameManager == null");
                return false;
            }
            var uiManager = gameManager.UIManager;
            uiManager.RegisterUITask(new UITaskRegisterItem() { Name = "MainMenuUITask", TypeFullName = "bluebean.Mugen3D.UI.MainMenuUITask" });
            uiManager.RegisterUITask(new UITaskRegisterItem() { Name = "CharacterSelectUITask", TypeFullName = "bluebean.Mugen3D.UI.CharacterSelectUITask" });
            uiManager.RegisterUITask(new UITaskRegisterItem() { Name = "BattleUITask", TypeFullName = "bluebean.Mugen3D.UI.BattleUITask" });
            return true;
        }
    }
}
