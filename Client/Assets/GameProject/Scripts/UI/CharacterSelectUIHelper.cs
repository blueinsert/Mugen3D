using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.Mugen3D.UI
{
    public class CharacterSelectUIHelper
    {
        public static CharacterGridPos GetMaxGridPos(int count, int rowSize)
        {
            CharacterGridPos gridPos = new CharacterGridPos();
            gridPos.Row = (count - 1) / rowSize;
            if(count > rowSize)
            {
                gridPos.Col = rowSize - 1;
            }
            else
            {
                gridPos.Col = count - 1;
            }
            return gridPos;
        }

        public static CharacterGridPos GetGridPos(int index, int rowSize)
        {
            CharacterGridPos gridPos = new CharacterGridPos();
            gridPos.Row = index / rowSize;
            gridPos.Col = index % rowSize;
            return gridPos;
        }

        public static int GetIndexFromGridPos(CharacterGridPos gridPos, int rowSize)
        {
            return gridPos.Row * rowSize + gridPos.Col;
        }

    }
}
