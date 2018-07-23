using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{
    public class CameraConfig : EntityConfig
    {
        public Number depth { get; set; }
        public Number fieldOfView { get; set; }
        public Number yOffset { get; set; }
        public Number aspect { get; set; }
    }
}
