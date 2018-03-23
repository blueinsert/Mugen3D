using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mugen3D
{
    public class Config
    {
        public Dictionary<string, string> rawData = new Dictionary<string,string>();
        public Dictionary<int, float> data = new Dictionary<int,float>();

        public void AddConfig(string key, string value)
        {
            rawData[key] = value;
            float result;
            if (float.TryParse(value, out result))
            {
                data[key.GetHashCode()] = float.Parse(value);
            }
            else
            {
                data[key.GetHashCode()] = value.GetHashCode();
            }
        }

        public float GetConfig(int key)
        {
            if (data.ContainsKey(key))
            {
                return data[key];
            }
            else
            {
                Log.Error("can't get playerConfit, key:" + key);
                return 0;
            }
        }

        public float GetConfig(string key)
        {
            return GetConfig(key.GetHashCode());
        }

    }
}
