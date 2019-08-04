using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.UGFramework.ConfigData
{
    public partial class ConfigDataLoader
    {
	    #region 类内部变量
	    private Dictionary<int,ConfigDataCharacter> m_configDataCharacter = new Dictionary<int,ConfigDataCharacter>();
		#endregion
		
		 partial void InitAllConfigDataTableName(){
			m_allConfigTableNames.Clear();
			m_allConfigTableNames.Add("Character");
		}
		
		#region 访问函数
	    public Dictionary<int,ConfigDataCharacter> GetAllConfigDataCharacter () {
			return m_configDataCharacter;
		}
        
	    public ConfigDataCharacter GetConfigDataCharacter (int id) {
			ConfigDataCharacter data;
			(m_configDataCharacter).TryGetValue(id, out data);
			return data;
		}
        #endregion	
	
	    #region 反序列化
	    private void DeserializeConfigDataCharacter (AssetObject scriptableObj) {
		    var data = scriptableObj.m_bytes;
		    MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            object obj = bf.Deserialize(stream);
			List<ConfigDataCharacter> list = (List<ConfigDataCharacter>) obj;
			foreach(var configData in list){
				(m_configDataCharacter).Add(configData.ID, configData);
			}
	    }
		
	    partial void InitAllDeserializeFuncs(){
			m_deserializeFuncDics.Clear();
		    m_deserializeFuncDics.Add("ConfigDataCharacter", DeserializeConfigDataCharacter);
	    }
		
		#endregion
	}
}