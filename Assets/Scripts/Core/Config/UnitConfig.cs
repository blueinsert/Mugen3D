using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{
    public class UnitConfig : EntityConfig
    {
        public string modelFile { get; set; }
        public string actionConfigFile { get; set; }
        public string cmdConfigFile { get; set; }
        public string fsmConfigFile { get; set; }
    }
}
