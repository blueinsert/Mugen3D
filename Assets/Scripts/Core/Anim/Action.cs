﻿using System.Collections;
using System.Collections.Generic;
using Vector = Mugen3D.Core.Vector;
using Number = Mugen3D.Core.Number;

namespace Mugen3D.Core
{
    //collsion
    public class Clsn {
        public int type { get; set; }
        public Number x1 { get; set; }
        public Number y1 { get; set; }
        public Number x2 { get; set; }
        public Number y2 { get; set; }

        public Clsn(int type, Number x1, Number y1, Number x2, Number y2)
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
        public Number normalizeTime { get; set; }
        public int duration { get; set; }
        public List<Clsn> clsns { get; set; }

        public Number xOffset { get; set; }
        public Number yOffset { get; set; }

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