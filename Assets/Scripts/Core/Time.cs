using System.Collections;
using System.Collections.Generic;

namespace Mugen3D.Core
{
    public class Time
    {
        public static Number deltaTime;
        public static Number time;
        public static int frameCount;

        public static void Clear()
        {
            deltaTime = 0;
            time = 0;
            frameCount = 0;
        }

        public static void Update(Number _deltaTime)
        {
            time += _deltaTime;
            deltaTime = _deltaTime;
            frameCount++;
        }
    }
}