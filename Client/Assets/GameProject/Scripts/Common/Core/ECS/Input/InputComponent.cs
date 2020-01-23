using System.Collections;
using System.Collections.Generic;

namespace bluebean.Mugen3D.Core
{
    public class InputComponent: ComponentBase
    {
        public InputComponent()
        {

        }

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
