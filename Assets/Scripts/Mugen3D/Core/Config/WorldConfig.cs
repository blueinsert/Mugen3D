using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mugen3D.Core
{
    public class VectorInt{
        public int x {get; set;}
        public int y {get; set;}
    }

    public class CameraConfig
    {  
        public Number depth { get; set; }
        public Number yOffset { get; set; }
        public Number maxPlayerDist { get; set; }
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
        public VectorInt[] initPos { get; set; }
        public CameraConfig cameraConfig { get; set; }
    }

    public class PlayerInputConfig
    {
        public int slot { get; set; }
        public int up { get; set; }
        public int down { get; set; }
        public int left { get; set; }
        public int right { get; set; }
        public int a { get; set; }
        public int b { get; set; }
        public int c { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
    }

    public class InputConfig
    {
        public PlayerInputConfig[] playerInputs { get; set; }
    }

    public class WorldConfig
    {
        public StageConfig stageConfig { get; private set; }
        public InputConfig inputConfig { get; private set; }

        public void SetStageConfig(StageConfig stageConfig)
        {
            this.stageConfig = stageConfig;
        }

        public void SetInputConfig(InputConfig inputConfig)
        {
            this.inputConfig = inputConfig;
        }
    }
}