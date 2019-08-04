using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;
using System.Threading;

namespace bluebean.UGFramework.ConfigData
{
    /// <summary>
    /// todo 完成
    /// 1.主键ID的非重复检查
    /// 2.外键值存在性的检查
    /// </summary>
    public class ConfigDataEditorWindow : EditorWindow
    {
        [MenuItem("ConfigData/OpenWindow")]
        public static void OpenConfigDataEditorWindow()
        {
            var win = GetWindow<ConfigDataEditorWindow>(typeof(ConfigDataEditorWindow).Name);
            win.minSize = new Vector2(480, 320);
            win.Show();
            
        }

        /// <summary>
        /// 配置表信息字典
        /// </summary>
        private  Dictionary<string, ConfigDataTableInfo> m_configDataTableInfoDic = new Dictionary<string, ConfigDataTableInfo>();
        private CoroutineScheduler m_coroutineManager = new CoroutineScheduler();
        private  ConfigDataSetting m_setting;

        void OnEnable()
        {
            m_setting = ConfigDataSetting.Instance;  
        }

        private void OnDestroy()
        {
            Debug.Log("ConfigDataEditorWindow OnDestroy");
        }

        private void Update()
        {
            m_coroutineManager.Tick();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("1.GenerateAutoGenCodeFiles"))
            {
                m_coroutineManager.StartCorcoutine(GenerateAutoGenCodeFiles());
            }
            if (GUILayout.Button("2.GenerateSerializeTableData"))
            {
                m_coroutineManager.StartCorcoutine(SerializeTableData());
            }
            if (GUILayout.Button("3.RunTest"))
            {
                RunTest();
            }
            if (GUILayout.Button("[Error]BuildAll"))
            {
                m_coroutineManager.StartCorcoutine(BuildAll());
            }
        }

        /// <summary>
        /// 获取所有配置文件路径
        /// </summary>
        /// <returns></returns>
        private string[] GetAllConfigDataFilePath()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(m_setting.m_configDataPath);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            var fileInfos = directoryInfo.GetFiles("*.csv", SearchOption.AllDirectories);
            string[] filePaths = new string[fileInfos.Length];
            if (fileInfos.Length > 0)
            {
                for (int i = 0; i < fileInfos.Length; i++)
                {
                    filePaths[i] = fileInfos[i].FullName;
                }
            }
            return filePaths;
        }

        /// <summary>
        /// 读取所有配置表
        /// </summary>
        /// <param name="filePathList"></param>
        private IEnumerator ReadAllConfigData(string[] filePathList)
        {
            foreach (var filePath in filePathList)
            {
                var configDataTableInfo = new ConfigDataTableInfo();
                configDataTableInfo.Initialize(filePath);
                m_configDataTableInfoDic.Add(configDataTableInfo.TableName, configDataTableInfo);
                yield return null;
            }
        }

        /// <summary>
        /// 解析外键列类型
        /// </summary>
        private IEnumerator ParseForeignKeyColumnType()
        {
            foreach (var tableInfo in m_configDataTableInfoDic.Values)
            {
                foreach (var columnInfo in tableInfo.ColumnInfoList)
                {
                    //遍历所有列定义，如果列名使用“."分割，说明是外键数据类型
                    string typeStr = columnInfo.TypeStr;
                    if (typeStr.IndexOf(".") != -1)
                    {
                        string[] tuple = typeStr.Split(new char[] { '.' });
                        if (tuple.Length != 2)
                        {
                            throw new Exception(string.Format("the column {0} at table {1} is a foreign key, but typeStr is not a tuple", columnInfo.ColumnName, tableInfo.TableName));
                        }
                        string foreignTableName = tuple[0];
                        string foreignColumnName = tuple[1];
                        if (!m_configDataTableInfoDic.ContainsKey(foreignTableName))
                        {
                            throw new Exception(string.Format("can't find the foreign table {0} for the column {1} at table {2} ", foreignTableName, columnInfo.ColumnName, tableInfo.TableName));
                        }
                        var foreignTabelInfo = m_configDataTableInfoDic[foreignTableName];
                        var foreignColumnInfo = foreignTabelInfo.GetColumnInfoByName(foreignColumnName);
                        if (foreignColumnInfo == null)
                        {
                            throw new Exception(string.Format("can't find the foreign column {0} for the column {1} at table {2}", foreignColumnName, columnInfo.ColumnName, tableInfo.TableName));
                        }
                        //设置正确的typeStr
                        columnInfo.RealTypeStr = foreignColumnInfo.TypeStr;
                    }
                }
                yield return null;
            }
        }

        /// <summary>
        /// Step1. 产生自动化代码
        /// </summary>
        private IEnumerator GenerateAutoGenCodeFiles()
        {
            m_configDataTableInfoDic.Clear();
            yield return ReadAllConfigData(GetAllConfigDataFilePath());
            yield return ParseForeignKeyColumnType();

            string outputFolder = PathHelper.GetFullPath(m_setting.m_autoGenCodeOutputPath);
            if (Directory.Exists(outputFolder))
            {
                Directory.Delete(outputFolder, true);
            }
            Directory.CreateDirectory(outputFolder);

            var allConfigDataInfos = new List<ConfigDataTableInfo>(m_configDataTableInfoDic.Values);
        
            //1.产生ConfigDataTypeDefine.cs，类型定义文件
            {
                string template = File.ReadAllText(PathHelper.GetFullPath(m_setting.m_codeTempletePath) + "/ConfigDataTypeDefine.cs.txt");
                string result = NVelocityHelper.Parse(template, "AllConfigDataInfos", allConfigDataInfos);
                File.WriteAllText(outputFolder + "/ConfigDataTypeDefine.cs", result);
            }
            yield return null;
            //2.产生ConfigDataLoaderAutoGen.cs，运行时读取功能
            {
                string template = File.ReadAllText(PathHelper.GetFullPath(m_setting.m_codeTempletePath) + "/ConfigDataLoaderAutoGen.cs.txt");
                string result = NVelocityHelper.Parse(template, "AllConfigDataInfos", allConfigDataInfos);
                File.WriteAllText(outputFolder + "/ConfigDataLoaderAutoGen.cs", result);
            }
            AssetDatabase.Refresh();
            ShowNotification(new GUIContent("GenerateAutoGenCodeFiles Done"));
        }

        /// <summary>
        /// 使用反射产生对应表的List数据
        /// </summary>
        /// <param name="configTableDataTypeDefine"></param>
        /// <param name="tableInfo"></param>
        /// <returns></returns>
        private System.Object FillDynamicListWithTableData(Type configTableDataTypeDefine, ConfigDataTableInfo tableInfo)
        {
            var listType = typeof(List<>).MakeGenericType(configTableDataTypeDefine);
            var list = Activator.CreateInstance(listType);
            var addMethod = listType.GetMethod("Add");
            if (tableInfo.Row > 2)
            {
                for (int i = 2; i < tableInfo.Row; i++)
                {
                    var data = Activator.CreateInstance(configTableDataTypeDefine);
                    foreach (var columnInfo in tableInfo.ColumnInfoList)
                    {
                        var filedInfo = configTableDataTypeDefine.GetField("m_" + columnInfo.ColumnName, BindingFlags.NonPublic | BindingFlags.Instance);
                        var value = ConfigDataValueConverter.GetValue(tableInfo.ReadCell(i, columnInfo.ColumnNo), columnInfo.RealTypeStr);
                        filedInfo.SetValue(data, value);
                    }
                    addMethod.Invoke((object)list, new object[] { data });
                }
            }
            return list;
        }

        /// <summary>
        /// Step2. 序列化配置表数据
        /// </summary>
        /// <returns></returns>
        public IEnumerator SerializeTableData()
        {
            m_configDataTableInfoDic.Clear();
            yield return ReadAllConfigData(GetAllConfigDataFilePath());
            yield return ParseForeignKeyColumnType();

            string outputPath = PathHelper.GetFullPath(m_setting.m_serializedTableDataOutputPath);
            if (Directory.Exists(outputPath))
            {
                Directory.Delete(outputPath, true);
            }
            Directory.CreateDirectory(outputPath);
            int tableCount = m_configDataTableInfoDic.Count;
            int count = 0;
            foreach (var tableInfo in m_configDataTableInfoDic.Values)
            {
                Type tableDefineType = null;
                if (tableInfo.TableType == ConfigDataTabelType.DataTable)
                {
                    tableDefineType = ClassLoader.GetType(m_setting.m_codeNameSpace + ".ConfigData" + tableInfo.TableName);
                }
                if (tableDefineType != null)
                {
                    var listObj = FillDynamicListWithTableData(tableDefineType, tableInfo);
                    byte[] data;
                    switch (m_setting.m_configDataSerializeType)
                    {
                        case ConfigDataSerializeType.Bin:
                            data = SerializationHelper.SerializeToBinary(listObj);
                            break;
                        case ConfigDataSerializeType.Xml:
                            data = SerializationHelper.SerializeToXml(listObj);
                            break;
                        default:
                            data = SerializationHelper.SerializeToBinary(listObj);
                            break;
                    }
                    AssetObject bytesScriptableObjectMD5 = ScriptableObject.CreateInstance<AssetObject>();
                    bytesScriptableObjectMD5.m_bytes = data;
                    bytesScriptableObjectMD5.m_MD5 = "undefined";
                    AssetDatabase.CreateAsset(bytesScriptableObjectMD5, m_setting.m_serializedTableDataOutputPath + "/ConfigData" + tableInfo.TableName + ".asset");
                }
                count++;
                EditorUtility.DisplayProgressBar("SerializeTableData", "progress", count / (float)tableCount);
                yield return null;
            }
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();
            ShowNotification(new GUIContent("SerializeTableData Done"));
        }

        /// <summary>
        /// Step3: RunTest
        /// </summary>
        private void RunTest()
        {
            m_coroutineManager.StartCorcoutine(ConfigDataLoader.Instance.LoadAllConfigData((res) => {
                if (res)
                    ShowNotification(new GUIContent("RunTest OK"));
            }));
        }

        public IEnumerator BuildAll() {
            m_configDataTableInfoDic.Clear();
            yield return ReadAllConfigData(GetAllConfigDataFilePath());
            yield return ParseForeignKeyColumnType();
            yield return GenerateAutoGenCodeFiles();
            AssetDatabase.Refresh();
            while (EditorApplication.isCompiling)
            {
                Debug.Log(DateTime.Now.Ticks + " IsCompiling");
                yield return null;
            }
            //todo error 并不能走到这一步
            Debug.Log(DateTime.Now.Ticks + " CompileEnd");
            yield return SerializeTableData();
        }
    }
}
