using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    //collsion
    public class Clsn {
        public bool useLast { get; set; }
        public int type { get; set; }
        public float x1 { get; set; }
        public float y1 { get; set; }
        public float x2 { get; set; }
        public float y2 { get; set; }

        public Clsn(int type, float x1, float y1, float x2, float y2)
        {
            this.type = type;
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }

        public Clsn()
        {

        }
    }

    public class ActionFrame
    {
        public float normalizeTime { get; set; }
        public int duration { get; set; }
        public List<Clsn> clsns1 { get; set; }
        public List<Clsn> clsns2 { get; set; }
        public List<Clsn> clsns3 { get; set; }
        public float xOffset { get; set; }
        public float yOffset { get; set; }

        public ActionFrame()
        {
            clsns1 = new List<Clsn>();
            clsns2 = new List<Clsn>();
            clsns3 = new List<Clsn>();
        }
    }

    public class Action
    {
        public int animNo { get; set; }
        public string animName { get; set; }
        public int animLength { get; set; }
        public List<ActionFrame> frames { get; set; }
        public int loopStartIndex { get; set; }

        public Action()
        {
            frames = new List<ActionFrame>();
        }

        public Action(int actionNo)
        {
            this.animNo = actionNo;
            frames = new List<ActionFrame>();
        }
    }
}
