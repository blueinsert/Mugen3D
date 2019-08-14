using bluebean.Mugen3D.UI;
using bluebean.UGFramework.ConfigData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class Test
{
    [MenuItem("Test/StartBattleUITask")]
    public static void StartBattleUITask()
    {
        var configLoader = ConfigDataLoader.Instance;
        BattleUITask.StartUITask(null, configLoader.GetConfigDataCharacter(1), configLoader.GetConfigDataCharacter(1), ConfigDataLoader.Instance.GetConfigDataStage(1));
    }
}
