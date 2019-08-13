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
	    private Dictionary<int,ConfigDataCamera> m_configDataCamera = new Dictionary<int,ConfigDataCamera>();
	    private Dictionary<int,ConfigDataCharacter> m_configDataCharacter = new Dictionary<int,ConfigDataCharacter>();
	    private Dictionary<int,ConfigDataCommand> m_configDataCommand = new Dictionary<int,ConfigDataCommand>();
	    private Dictionary<int,ConfigDataInputDefault> m_configDataInputDefault = new Dictionary<int,ConfigDataInputDefault>();
	    private Dictionary<int,ConfigDataStage> m_configDataStage = new Dictionary<int,ConfigDataStage>();
		#endregion
		
		 partial void InitAllConfigDataTableName(){
			m_allConfigTableNames.Clear();
			m_allConfigTableNames.Add("Camera");
			m_allConfigTableNames.Add("Character");
			m_allConfigTableNames.Add("Command");
			m_allConfigTableNames.Add("InputDefault");
			m_allConfigTableNames.Add("Stage");
		}
		
		#region 访问函数
	    public Dictionary<int,ConfigDataCamera> GetAllConfigDataCamera () {
			return m_configDataCamera;
		}
	    public Dictionary<int,ConfigDataCharacter> GetAllConfigDataCharacter () {
			return m_configDataCharacter;
		}
	    public Dictionary<int,ConfigDataCommand> GetAllConfigDataCommand () {
			return m_configDataCommand;
		}
	    public Dictionary<int,ConfigDataInputDefault> GetAllConfigDataInputDefault () {
			return m_configDataInputDefault;
		}
	    public Dictionary<int,ConfigDataStage> GetAllConfigDataStage () {
			return m_configDataStage;
		}
        
	    public ConfigDataCamera GetConfigDataCamera (int id) {
			ConfigDataCamera data;
			(m_configDataCamera).TryGetValue(id, out data);
			return data;
		}
	    public ConfigDataCharacter GetConfigDataCharacter (int id) {
			ConfigDataCharacter data;
			(m_configDataCharacter).TryGetValue(id, out data);
			return data;
		}
	    public ConfigDataCommand GetConfigDataCommand (int id) {
			ConfigDataCommand data;
			(m_configDataCommand).TryGetValue(id, out data);
			return data;
		}
	    public ConfigDataInputDefault GetConfigDataInputDefault (int id) {
			ConfigDataInputDefault data;
			(m_configDataInputDefault).TryGetValue(id, out data);
			return data;
		}
	    public ConfigDataStage GetConfigDataStage (int id) {
			ConfigDataStage data;
			(m_configDataStage).TryGetValue(id, out data);
			return data;
		}
        #endregion	
	
	    #region 反序列化
	    private void DeserializeConfigDataCamera (AssetObject scriptableObj) {
		    var data = scriptableObj.m_bytes;
		    MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            object obj = bf.Deserialize(stream);
			List<ConfigDataCamera> list = (List<ConfigDataCamera>) obj;
			foreach(var configData in list){
				(m_configDataCamera).Add(configData.ID, configData);
			}
	    }
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
	    private void DeserializeConfigDataCommand (AssetObject scriptableObj) {
		    var data = scriptableObj.m_bytes;
		    MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            object obj = bf.Deserialize(stream);
			List<ConfigDataCommand> list = (List<ConfigDataCommand>) obj;
			foreach(var configData in list){
				(m_configDataCommand).Add(configData.ID, configData);
			}
	    }
	    private void DeserializeConfigDataInputDefault (AssetObject scriptableObj) {
		    var data = scriptableObj.m_bytes;
		    MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            object obj = bf.Deserialize(stream);
			List<ConfigDataInputDefault> list = (List<ConfigDataInputDefault>) obj;
			foreach(var configData in list){
				(m_configDataInputDefault).Add(configData.ID, configData);
			}
	    }
	    private void DeserializeConfigDataStage (AssetObject scriptableObj) {
		    var data = scriptableObj.m_bytes;
		    MemoryStream stream = new MemoryStream();
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            BinaryFormatter bf = new BinaryFormatter();
            object obj = bf.Deserialize(stream);
			List<ConfigDataStage> list = (List<ConfigDataStage>) obj;
			foreach(var configData in list){
				(m_configDataStage).Add(configData.ID, configData);
			}
	    }
		
	    partial void InitAllDeserializeFuncs(){
			m_deserializeFuncDics.Clear();
		    m_deserializeFuncDics.Add("ConfigDataCamera", DeserializeConfigDataCamera);
		    m_deserializeFuncDics.Add("ConfigDataCharacter", DeserializeConfigDataCharacter);
		    m_deserializeFuncDics.Add("ConfigDataCommand", DeserializeConfigDataCommand);
		    m_deserializeFuncDics.Add("ConfigDataInputDefault", DeserializeConfigDataInputDefault);
		    m_deserializeFuncDics.Add("ConfigDataStage", DeserializeConfigDataStage);
	    }
		
		#endregion
	}
}