using System.Collections;
using System.Collections.Generic;

namespace bluebean.UGFramework.ConfigData
{
    public interface IGridDataReader
    {
        int Row { get; }
        int Column { get; }
        void ParseFromString(string content);
        void ParseFromFile(string filePath);
        string[] ReadLine(int n);
        string ReadCell(int x, int y);
    }
}
