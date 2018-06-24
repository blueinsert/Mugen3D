using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    //collsion
    public class Clsn {
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

        public Clsn(Clsn clsn)
        {
            this.type = clsn.type;
            this.x1 = clsn.x1;
            this.y1 = clsn.y1;
            this.x2 = clsn.x2;
            this.y2 = clsn.y2;
        }

        public Clsn()
        {

        }
    }

    public class ActionFrame
    {
        public float normalizeTime { get; set; }
        public int duration { get; set; }
        public List<Clsn> clsns { get; set; }
       
        public float xOffset { get; set; }
        public float yOffset { get; set; }

        public ActionFrame()
        {
            clsns = new List<Clsn>();
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

        public void CalculateAnimLength()
        {
            int length = 0;
            foreach (var frame in frames)
            {
                length += frame.duration;
            }
            this.animLength = length;
        }
    }
}
