using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    /// <summary>
    /// 输入指令记录
    /// </summary>
    public class InputRecord
    {
        public int frameNum;
        public int entityID;
        public int inputCode;
    }

    public class InputComponent: ComponentBase
    {
        public Dictionary<int, List<InputRecord>> InputRecords = new Dictionary<int, List<InputRecord>>();
    }
}
