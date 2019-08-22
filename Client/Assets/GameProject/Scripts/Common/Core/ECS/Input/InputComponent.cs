﻿using System.Collections;
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
        #region 单例模式
        public static InputComponent Instance { get { return m_instance; } }
        private static InputComponent m_instance;
        public static InputComponent CreateInstance() { m_instance = new InputComponent(); return m_instance; }
        #endregion

        //public Dictionary<int, List<InputRecord>> InputRecords = new Dictionary<int, List<InputRecord>>();

        private int[] m_inputCodes = new int[2];

        public void Update(int p1InputCode, int p2InputCode)
        {
            m_inputCodes[0] = p1InputCode;
            m_inputCodes[1] = p2InputCode;
        }

        public int GetInputCode(int playerSlot)
        {
            return m_inputCodes[playerSlot];
        }
    }
}