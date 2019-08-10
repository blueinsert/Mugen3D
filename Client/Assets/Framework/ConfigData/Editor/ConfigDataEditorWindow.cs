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
    /// todo
    /// 1.主键ID的非重复检查
    /// 2.外键值存在性的检查
    /// 3.复合数据类型的支持，List,数组等
    /// </summary>
    public class ConfigDataEditorWindow : EditorWindow
    {
        [MenuItem("Framework/ConfigData/BuildAll")]
        public static void BuidAll()
        {
            Instance.StartBuildAll();
        }

        [MenuItem("Framework/ConfigData/OpenWindow")]
        public static void OpenConfigDataEditorWindow()
        {
            Instance.Show();
        }

        #region 单例模式
        private static ConfigDataEditorWindow m_instance;
        public static ConfigDataEditorWindow Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = GetWindow<ConfigDataEditorWindow>(typeof(ConfigDataEditorWindow).Name);
                    m_instance.maxSize = new Vector2(480, 320);
                }
                return m_instance;
            }
        }
        #endregion

        /// <summary>
        /// 配置表信息字典
        /// </summary>
        private Dictionary<string, ConfigDataTableInfo> m_configDataTableInfoDic = new Dictionary<string, ConfigDataTableInfo>();
        private CoroutineScheduler m_coroutineManager = new CoroutineScheduler();
        private ConfigDataSetting m_setting;
        private ConfigDataTypeDefineCodeGenerator m_typeDefineCodeGenerator;
        private Assembly m_typeDefineAssembly;

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
            if (GUILayout.Button("BuildAll"))
            {
                m_coroutineManager.StartCorcoutine(BuildAll());
            }
            if (GUILayout.Button("RunTest"))
            {
                RunTest();
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

        /*
        /// <summary>
        /// 获取外键的实际数据类型
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        private Type GetForeignKeyType(string tableName, string columnName, string typeStr)
        {
            Type resultType = null;
            string[] tuple = typeStr.Split(new char[] { '.' });
            if (tuple.Length != 2)
            {
                throw new Exception(string.Format("the column {0} at table {1} is a foreign key, but typeStr is not a tuple", columnName, tableName));
            }
            string foreignTableName = tuple[0];
            string foreignColumnName = tuple[1];
            if (!m_configDataTableInfoDic.ContainsKey(foreignTableName))
            {
                throw new Exception(string.Format("can't find the foreign table {0} for the column {1} at table {2} ", foreignTableName, columnName, tableName));
            }
            var foreignTabelInfo = m_configDataTableInfoDic[foreignTableName];
            ConfigDataTableColumnInfo foreignColumnInfo = null;
            foreach (var columnInfo in foreignTabelInfo.ColumnInfoList)
            {
                if (columnInfo.ColumnName == name)
                {
                    foreignColumnInfo = columnInfo;
                    break;
                }
            }
            if (foreignColumnInfo == null || foreignColumnInfo.m_columnTypeParseState != ConfigDataTableColumnTypeParseState.Determine)
            {
                throw new Exception(string.Format("can't find the foreign column {0} for the column {1} at table {2}", foreignColumnName, columnName, tableName));
            }
            resultType = foreignColumnInfo.ColumnType;
            return resultType;
        }
        */

        /*
        private Type GetColumnType(string tableName, string columnName, string typeStr)
        {
            if (typeStr.StartsWith("List<") && typeStr.EndsWith(">"))
            {
                //List数据类型
                var unDetermineGenericType = typeof(List<>);
                List<Type> typeParams = new List<Type>();
                int index1 = typeStr.IndexOf("List<");
                int index2 = typeStr.LastIndexOf(">");
                string subStr = typeStr.Substring(index1 + "List<".Length + 1, index2);
                string[] typeParamStrs = subStr.Split(new char[] { ',' });
                for (int i = 0; i < typeParamStrs.Length; i++)
                {
                    typeParams.Add(GetColumnType(tableName, columnName, typeParamStrs[i]));
                }
                return unDetermineGenericType.MakeGenericType(typeParams.ToArray());
            }
            else if (typeStr.IndexOf(".") != -1)
            {
                //外键类型
                return GetForeignKeyType(tableName, columnName, typeStr);
            }
            else
            {
                //基础类型
                return TypeSolver.GetType(typeStr);
            }
        }
        */

        /// <summary>
        /// 解析列类型
        /// </summary>
        /// <returns></returns>
        private IEnumerator SolveColumnDataType()
        {
            //第一次遍历，解析基本数据类型
            foreach (var tableInfo in m_configDataTableInfoDic.Values)
            {
                foreach (var columnInfo in tableInfo.ColumnInfoList)
                {
                    var columnType = ConfigDataColumnTypeSolver.GetType(columnInfo.TypeStr);
                    columnInfo.m_columnTypeParseState = ConfigDataTableColumnTypeParseState.UnDetermine;
                    if (columnType != null)
                    {
                        columnInfo.SetColumnType(columnType);
                        columnInfo.m_columnTypeParseState = ConfigDataTableColumnTypeParseState.Determine;
                    }
                    yield return null;
                }
            }
            /*
            //第二次遍历, 解析外键类型
            foreach (var tableInfo in m_configDataTableInfoDic.Values)
            {
                foreach (var columnInfo in tableInfo.ColumnInfoList)
                {
                    if(columnInfo.m_columnTypeParseState != ConfigDataTableColumnTypeParseState.UnDetermine)
                    {
                        continue;
                    }
                    string typeStr = columnInfo.TypeStr;
                    Type columnType = GetColumnType(columnInfo.TableName, columnInfo.ColumnName, typeStr);
                    columnInfo.m_columnTypeParseState = ConfigDataTableColumnTypeParseState.Fail;
                    if (columnType != null)
                    {
                        columnInfo.SetColumnType(columnType);
                        columnInfo.m_columnTypeParseState = ConfigDataTableColumnTypeParseState.Determine;
                    }
                    
                }
            }
            */
        }

        /*
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
                        //columnInfo.RealTypeStr = foreignColumnInfo.TypeStr;
                    }
                }
                yield return null;
            }
        }
        */

        /// <summary>
        /// 产生ConfigDataLoaderAutoGen.cs
        /// </summary>
        private void GenerateConfigDataLoaderAutoGenFile()
        {
            string outputFolder = PathHelper.GetFullPath(m_setting.m_autoGenCodeOutputPath);
            if (Directory.Exists(outputFolder))
            {
                Directory.Delete(outputFolder, true);
            }
            Directory.CreateDirectory(outputFolder);

            var allConfigDataInfos = new List<ConfigDataTableInfo>(m_configDataTableInfoDic.Values);

            //产生ConfigDataLoaderAutoGen.cs，运行时读取功能
            {
                string template = File.ReadAllText(PathHelper.GetFullPath(m_setting.m_codeTempletePath) + "/ConfigDataLoaderAutoGen.cs.txt");
                string result = NVelocityHelper.Parse(template, "AllConfigDataInfos", allConfigDataInfos);
                File.WriteAllText(outputFolder + "/ConfigDataLoaderAutoGen.cs", result);
            }
            AssetDatabase.Refresh();
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
                        var value = ConfigDataColumnTypeSolver.GetValue(tableInfo.ReadCell(i, columnInfo.ColumnNo), columnInfo.TypeStr);
                        filedInfo.SetValue(data, value);
                    }
                    addMethod.Invoke((object)list, new object[] { data });
                }
            }
            return list;
        }

        /// <summary>
        /// 序列化配置表数据
        /// </summary>
        /// <returns></returns>
        public IEnumerator SerializeTableData()
        {
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
                if (tableInfo.TableType == ConfigDataTabelType.EnumTable)
                {
                    continue;
                }
                Type tableDefineType = m_typeDefineAssembly.GetType(m_setting.m_codeNameSpace + ".ConfigData" + tableInfo.TableName); ;
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
                //将bytes[]包装进AssetObject
                AssetObject bytesScriptableObjectMD5 = ScriptableObject.CreateInstance<AssetObject>();
                bytesScriptableObjectMD5.m_bytes = data;
                bytesScriptableObjectMD5.m_MD5 = "undefined";
                AssetDatabase.CreateAsset(bytesScriptableObjectMD5, m_setting.m_serializedTableDataOutputPath + "/ConfigData" + tableInfo.TableName + ".asset");
                count++;
                yield return null;
            }
            AssetDatabase.Refresh();
        }

        private IEnumerator BuildAll()
        {
            m_configDataTableInfoDic.Clear();
            var allFilePaths = GetAllConfigDataFilePath();
            EditorUtility.DisplayProgressBar("BuildAll", "ReadAllConfigData", 0.3f);
            yield return ReadAllConfigData(allFilePaths);//导入所有表数据，构建TableInfo字典
            EditorUtility.DisplayProgressBar("BuildAll", "SolveColumnDataType", 0.4f);
            yield return SolveColumnDataType();//求解数据列类型
            m_typeDefineCodeGenerator = new ConfigDataTypeDefineCodeGenerator(m_configDataTableInfoDic, m_setting);
            EditorUtility.DisplayProgressBar("BuildAll", "ConstructCompileUnit", 0.5f);
            m_typeDefineCodeGenerator.ConstructCompileUnit();
            EditorUtility.DisplayProgressBar("BuildAll", "GetAeeembly", 0.6f);
            m_typeDefineAssembly = m_typeDefineCodeGenerator.GetAeeembly();//ConfigDataTypeDefine.cs对应的assembly
            EditorUtility.DisplayProgressBar("BuildAll", "SerializeTableData", 0.7f);
            yield return SerializeTableData();//产生序列化资源文件
            EditorUtility.DisplayProgressBar("BuildAll", "Generate ConfigDataLoaderAutoGen.cs", 0.8f);
            GenerateConfigDataLoaderAutoGenFile();//产生ConfigDataLoaderAutoGen.cs
            EditorUtility.DisplayProgressBar("BuildAll", "Generate ConfigDataTypeDefine.cs", 0.9f);
            m_typeDefineCodeGenerator.GenerateCode();//产生ConfigDataTypeDefine.cs
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
            yield return SerializeTableData();
        }

        public void StartBuildAll()
        {
            m_coroutineManager.StartCorcoutine(BuildAll());
        }

        /// <summary>
        /// Step3: RunTest
        /// </summary>
        private void RunTest()
        {
            if(AssetLoader.Instance == null)
            {
                AssetLoader.CreateInstance();
            }
            m_coroutineManager.StartCorcoutine(ConfigDataLoader.Instance.LoadAllConfigData((res) =>
            {
                if (res)
                    ShowNotification(new GUIContent("RunTest OK"));
            }));
        }
    }
}
