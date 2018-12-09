using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{
    public class VectorSerialize{
        public Number x {get; set;}
        public Number y {get; set;}
        public Number z { get; set; }
    }

    public class CameraConfig
    {  
        public Number depth { get; set; } //the z coordinate value
        public Number aspect { get; set; } //width divided by height
        public Number yOffset { get; set; }
        public Number dumpRatio { get; set; }
        public Number minFiledOfView { get; set; }
        public Number maxFiledOfView { get; set; }
    }

    public class StageConfig
    {
        public string stage { get; set; }
        public int borderXMin { get; set; }
        public int borderXMax { get; set; }
        public int borderYMin { get; set; }
        public int borderYMax { get; set; }
        public VectorSerialize[] initPos { get; set; }
        public CameraConfig cameraConfig { get; set; }
    }

    public class WorldConfig
    {
        public StageConfig stageConfig { get; private set; }

        public void SetStageConfig(StageConfig stageConfig)
        {
            this.stageConfig = stageConfig;
        }

    }
}