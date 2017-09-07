using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    [System.Serializable]
    public class PlayerSetting
    {
        public TextAsset commandFile;
        public TextAsset commonStateFile;
        public List<TextAsset> stateFiles;
        public TextAsset paramsSetFile;
    }

}
