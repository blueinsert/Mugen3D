using System.Collections;
using System.Collections.Generic;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D.Core
{
    public class WorldConfig
    {
        public int borderXMin { get; set; }
        public int borderXMax { get; set; }
        public int borderYMin { get; set; }
        public int borderYMax { get; set; }
        public Vector[] initPos { get; set; }
    }
}
