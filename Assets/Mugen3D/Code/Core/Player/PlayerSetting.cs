using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    [System.Serializable]
    public class PlayerSetting
    {
        public TextAsset commandFile;
        public List<TextAsset> stateFiles;
        public TextAsset animDef;
    }

}
