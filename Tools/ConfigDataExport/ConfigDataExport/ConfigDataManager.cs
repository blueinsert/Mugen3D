using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.CodeDom;

namespace bluebean.CSVParser
{
    public delegate object GetValueByStr(string valeuStr);

    public class ForeignKeyCheckInfo
    {
        public ConfigDataColumnInfo m_columnInfo;
        public int m_row;
        public string m_value;
        public int m_indexInList;
        public ConfigDataColumnInfo m_mainColumnInfo;
        //todo
    }

    class ConfigDataManager
    {
        private static ConfigDataManager m_instance;
        public static ConfigDataManager Instance
        {
            get
            {
                return m_instance;
            }
        }
        public static void CreateInstance()
        {
            m_instance = new ConfigDataManager();
        }

        public const string NameSpace = "bluebean.ConfigData";

        private Dictionary<string, CsvReader> m_csvDic = new Dictionary<string, CsvReader>();

        private Dictionary<string, ConfigData> m_configDataDic = new Dictionary<string, ConfigData>();

        private Dictionary<string, Type> m_basicTypeDic = new Dictionary<string, Type>();

        private Dictionary<string, GetValueByStr> m_valueParserDic = new Dictionary<string, GetValueByStr>();

        private Dictionary<string, CodeTypeDeclaration> m_subTypeDeclarationDic = new Dictionary<string, CodeTypeDeclaration>();

        private Dictionary<string, Type> m_subTypeDic = new Dictionary<string, Type>();

        private Dictionary<string, CodeTypeDeclaration> m_configDataTypeDeclarationDic = new Dictionary<string, CodeTypeDeclaration>();

        private Dictionary<string, Type> m_configDataTypeDic = new Dictionary<string, Type>();

        private ForeignKeyCheckInfo m_checking = new ForeignKeyCheckInfo();

        private void CollectTypeStrToTypeDic()
        {
            m_basicTypeDic.Add("int16", typeof(Int16));
            m_basicTypeDic.Add("int32", typeof(Int32));
            m_basicTypeDic.Add("int64", typeof(Int64));
            m_basicTypeDic.Add("uint16", typeof(UInt16));
            m_basicTypeDic.Add("uint32", typeof(UInt32));
            m_basicTypeDic.Add("uint64", typeof(UInt64));
            m_basicTypeDic.Add("double", typeof(Double));
            m_basicTypeDic.Add("string", typeof(String));
            m_basicTypeDic.Add("bool", typeof(Boolean));
            m_basicTypeDic.Add("float", typeof(float));
        }

        private void CollectGetValueByStrDic()
        {
            m_valueParserDic.Add("int16", GetInt16);
            m_valueParserDic.Add("int32", GetInt32);
            m_valueParserDic.Add("int64", GetInt64);
            m_valueParserDic.Add("uint16", GetUInt16);
            m_valueParserDic.Add("uint32", GetUInt32);
            m_valueParserDic.Add("uint64", GetUInt64);
            m_valueParserDic.Add("double", GetDouble);
            m_valueParserDic.Add("string", GetString);
            m_valueParserDic.Add("bool", GetBool);
            m_valueParserDic.Add("float", GetFloat);
        }

        private object GetInt16(string str)
        {
            return Int16.Parse(str);
        }

        private object GetInt32(string str)
        {
            return Int32.Parse(str);
        }

        private object GetInt64(string str)
        {
            return Int64.Parse(str);
        }

        private object GetUInt16(string str)
        {
            return UInt16.Parse(str);
        }

        private object GetUInt32(string str)
        {
            return UInt32.Parse(str);
        }

        private object GetUInt64(string str)
        {
            return UInt64.Parse(str);
        }

        private object GetDouble(string str)
        {
            return Double.Parse(str);
        }

        private object GetFloat(string str)
        {
            return float.Parse(str);
        }

        private object GetBool(string str)
        {
            return bool.Parse(str);
        }

        private object GetString(string str)
        {
            return str;
        }

        /// <summary>
        /// 根据类型字符串得到类型(基础数据类型)
        /// </summary>
        /// <param name="typeStr"></param>
        /// <returns></returns>
        public Type TypeStr2Type(string typeStr)
        {
            typeStr = typeStr.ToLower();
            if (m_basicTypeDic.ContainsKey(typeStr))
            {
                return m_basicTypeDic[typeStr];
            }
            return null;
        }

        /// <summary>
        /// 根据类型字符串和字符串表示的值获得该值(基本类型)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public object GetValueByStr(string type, string value)
        {
            type = type.ToLower();
            if (m_valueParserDic.ContainsKey(type))
            {
                return m_valueParserDic[type](value);
            }
            return null;
        }

        private ConfigDataManager()
        {
            CollectTypeStrToTypeDic();
            CollectGetValueByStrDic();
        }

        #region 类型收集

        #region SubType
        private string GetSubTypeName(ConfigData configData, ConfigDataColumnInfo columnInfo)
        {
            var subTypeName = columnInfo.GetSubTypeName();
            if (string.IsNullOrEmpty(subTypeName))
            {
                subTypeName = configData.Name + columnInfo.m_name;
            }
            return subTypeName;
        }

        private void CollectSubTypeDeclaration(ConfigData configData)
        {
            foreach (var pair in configData.ColumnInfoDic)
            {
                var columnInfo = pair.Value;
                //收集子类型
                if (columnInfo.IsListType() && columnInfo.m_subTypeParamTypes.Length > 1)
                {
                    var subTypeName = GetSubTypeName(configData, columnInfo);
                    if (!m_subTypeDeclarationDic.ContainsKey(subTypeName))
                    {
                        var subType = columnInfo.BuildSubTypeDeclaration(subTypeName);
                        m_subTypeDeclarationDic.Add(subTypeName, subType);
                    }
                }
            }
        }

        private void CollectSubTypes()
        {
            var codeGenerater = new ConfigDataCodeGenerator();
            codeGenerater.SetNamespace(NameSpace);
            codeGenerater.ConstructCompileUnit(m_subTypeDeclarationDic);
            Assembly assembly;
            codeGenerater.GetAeeembly(out assembly);
            m_subTypeDic.Clear();
            foreach (var pair in m_subTypeDeclarationDic)
            {
                var subTypeName = pair.Key;
                var type = assembly.GetType(NameSpace + ".SubType" + subTypeName);
                m_subTypeDic.Add(subTypeName, type);
            }
        }

        public Type GetSubType(string subTypeName)
        {
            if (m_subTypeDic.ContainsKey(subTypeName))
            {
                return m_subTypeDic[subTypeName];
            }
            return null;
        }

        #endregion

        #region ConfigDataType

        public Type GetColumnType(ConfigData configData, ConfigDataColumnInfo columnInfo)
        {
            Type type;
            if (columnInfo.IsListType())
            {
                Type subType;
                if (columnInfo.m_subTypeParamTypes.Length > 1)
                {
                    string subTypeName = GetSubTypeName(configData, columnInfo);
                    subType = GetSubType(subTypeName);
                }
                else
                {
                    subType = TypeStr2Type(columnInfo.m_subTypeParamTypes[0]);
                }
                var listType = typeof(List<>).MakeGenericType(subType);
                type = listType;
            }
            else
            {
                type = TypeStr2Type(columnInfo.m_type);
            }
            return type;
        }

        private void CollectConfigDataTypeDeclaration(ConfigData configData)
        {
            var typeDeclaration = configData.BuildCodeTypeDeclaration();
            m_configDataTypeDeclarationDic.Add(configData.Name, typeDeclaration);
        }

        private void CollectConfigDataTypes()
        {
            var typeDeclarationDic = new Dictionary<string, CodeTypeDeclaration>();
            foreach (var pair in m_subTypeDeclarationDic)
            {
                typeDeclarationDic.Add(pair.Key, pair.Value);
            }
            foreach (var pair in m_configDataTypeDeclarationDic)
            {
                typeDeclarationDic.Add(pair.Key, pair.Value);
            }
            var codeGenerater = new ConfigDataCodeGenerator();
            codeGenerater.SetNamespace(NameSpace);
            codeGenerater.ConstructCompileUnit(typeDeclarationDic);
            Assembly assembly;
            codeGenerater.GetAeeembly(out assembly);
            m_configDataTypeDic.Clear();
            foreach (var pair in m_configDataTypeDeclarationDic)
            {
                var configDataTypeName = pair.Key;
                var type = assembly.GetType(NameSpace + ".ConfigData" + configDataTypeName);
                m_configDataTypeDic.Add(configDataTypeName, type);
            }
        }

        public Type GetConfigDataType(string name)
        {
            if (m_configDataTypeDic.ContainsKey(name))
            {
                return m_configDataTypeDic[name];
            }
            return null;
        }

        #endregion

        #endregion

        private CsvReader LoadCSV(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            if (m_csvDic.ContainsKey(filePath))
            {
                return m_csvDic[filePath];
            }
            CsvReader csv = new CsvReader();
            csv.ParseFromFile(filePath);
            m_csvDic.Add(filePath, csv);
            return csv;
        }

        private ConfigData LoadConfigDataAtPath(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            ConfigData configData = new ConfigData(filePath);
            CsvReader csv = LoadCSV(filePath);
            configData.Load(csv);
            m_configDataDic.Add(configData.Name, configData);
            return configData;
        }

        private void LoadAllConfigDataAtPath(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            var fileInfos = directoryInfo.GetFiles("*.csv", SearchOption.AllDirectories);
            foreach (var file in fileInfos)
            {
                LoadConfigDataAtPath(file.FullName);
            }
        }

        private ConfigData GetConfigDataByName(string name)
        {
            if (m_configDataDic.ContainsKey(name))
            {
                return m_configDataDic[name];
            }
            return null;
        }

        #region 检查外键约束

        /// <summary>
        /// 外键值是否在主表对应列存在
        /// </summary>
        /// <param name="configData"></param>
        /// <param name="mainColumnInfo"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsContainValueInMainConfigData(ConfigDataColumnInfo mainColumnInfo, string value)
        {
            var configData = mainColumnInfo.m_configData;
            int startRow = configData.Type == ConfigDataType.DATA ? ConfigData.DataStartRow : ConfigData.EnumStartRow;
            var csv = LoadCSV(configData.FilePath);
            for (int i = startRow; i < csv.Row; i++)
            {
                if (csv.ReadCell(i, mainColumnInfo.m_index) == value)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsContainValueInMainConfigData(string foreignKey, string value, out string error)
        {
            m_checking.m_value = value;
            error = "";
            string[] splits = foreignKey.Split(new char[] { '.' });
            if (splits.Length != 2)
            {
                error = string.Format("外键定义格式错误:{0}，在{1}.{2}", foreignKey, m_checking.m_columnInfo.m_configData.Name, m_checking.m_columnInfo.m_name);
                return false;
            }
            var mainTableName = splits[0];
            var mainColumnName = splits[1];
            var mainConfigData = GetConfigDataByName(mainTableName);
            if (mainConfigData == null)
            {
                error = string.Format("不能找到外键对应的主表：{0},在{1}.{2}", foreignKey, m_checking.m_columnInfo.m_configData.Name, m_checking.m_columnInfo.m_name);
                return false;
            }
            if (!mainConfigData.ContainColumn(mainColumnName))
            {
                error = string.Format("不能找到外键对应的主表的列：{0},在{1}.{2}", foreignKey, m_checking.m_columnInfo.m_configData.Name, m_checking.m_columnInfo.m_name);
                return false;
            }
            var mainColumnInfo = mainConfigData.GetColumnInfo(mainColumnName);
            if (mainColumnInfo.m_type != m_checking.m_columnInfo.m_type)
            {
                error = string.Format("外键列类型与主表列不一致：{0},在{1}.{2}", foreignKey, m_checking.m_columnInfo.m_configData.Name, m_checking.m_columnInfo.m_name);
                return false;
            }
            m_checking.m_mainColumnInfo = mainColumnInfo;
            if (!IsContainValueInMainConfigData(mainColumnInfo, value))
            {
                error = string.Format("无法找到值{0}在主表{1}.{2},来源{3}.{4} 行号:{5}", value, mainConfigData.Name, mainColumnInfo.m_name, m_checking.m_columnInfo.m_configData.Name, m_checking.m_columnInfo.m_name,m_checking.m_row); ;
                return false;
            }
            return true;
        }

        private bool CheckForeignKey(ConfigDataColumnInfo columnInfo, out string error)
        {
            error = "";
            if (columnInfo.m_foreignKeys == null || columnInfo.m_foreignKeys.Length == 0)
                return true;
            if (columnInfo.m_foreignKeys.Length == 1 && columnInfo.m_foreignKeys[0] == "NULL")
            {
                return true;
            }
            var configData = columnInfo.m_configData;
            var csv = LoadCSV(configData.FilePath);
            int startRow = configData.Type == ConfigDataType.DATA ? ConfigData.DataStartRow : ConfigData.EnumStartRow;
            for (int i = startRow; i < csv.Row; i++)
            {
                m_checking.m_row = i;
                string value = csv.ReadCell(i, columnInfo.m_index);
                if (!columnInfo.IsListType())
                {

                    return IsContainValueInMainConfigData(columnInfo.m_foreignKeys[0], value, out error);
                }
                else
                {
                    string[] listElemArray = value.Split(new char[] { ConfigData.ListElemSplit });
                    int paramCount = columnInfo.m_subTypeParamTypes.Length;
                    for (int k=0;k< listElemArray.Length;k++)
                    {
                        var listElemValueStr = listElemArray[k];
                        string[] paramList = listElemValueStr.Split(new char[] { ConfigData.ListElemParamSplit });
                        if (paramList.Length != paramCount)
                        {
                            error = string.Format("{0}与参数定义中个数不相符 在{1}.{2}",listElemValueStr,configData.Name,columnInfo.m_name);
                            return false;
                        }
                        for (int j = 0; j < paramCount; j++)
                        {
                            string paramValue = paramList[j];
                            string foreignKey = columnInfo.m_foreignKeys[j];
                            m_checking.m_indexInList = k;
                            return IsContainValueInMainConfigData(foreignKey, paramValue, out error);
                        }
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 检查外键约束合法性
        /// </summary>
        /// <param name="configData"></param>
        private bool CheckForeignKey(ConfigData configData, out string error)
        {
            error = "";
            foreach (var pair in configData.ColumnInfoDic)
            {
                m_checking.m_columnInfo = pair.Value;
                if (!CheckForeignKey(pair.Value, out error))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        /// <summary>
        /// 使用反射产生对应表的List数据
        /// </summary>
        /// <param name="configTableDataTypeDefine"></param>
        /// <param name="configData"></param>
        /// <returns></returns>
        private System.Object FillDynamicListWithConfigData(Type type, ConfigData configData)
        {
            var listType = typeof(List<>).MakeGenericType(type);
            var list = Activator.CreateInstance(listType);
            var addMethod = listType.GetMethod("Add");
            var csv = LoadCSV(configData.FilePath);
            if (csv.Row > 12)
            {
                for (int i = 12; i < csv.Row; i++)
                {
                    var data = Activator.CreateInstance(type);
                    foreach (var pair in configData.ColumnInfoDic)
                    {
                        var columnIndex = pair.Key;
                        var columnInfo = pair.Value;
                        try
                        {
                            var filedInfo = type.GetField("m_" + columnInfo.m_name, BindingFlags.NonPublic | BindingFlags.Instance);
                            string valueStr = csv.ReadCell(i, columnIndex);
                            var value = GetValueByStr(columnInfo.m_type, valueStr);
                            filedInfo.SetValue(data, value);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(string.Format("FillDynamicListWithTableData while process table {0} column {1}, msg:{2}", configData.Name, columnInfo.m_name, e.Message));
                        }
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
        public void SerializeConfigData(Assembly assembly, ConfigData configData, string outPath, string format)
        {
            if (configData.Type == ConfigDataType.ENUM)
            {
                return;
            }
            Type type = assembly.GetType("bluebean" + ".ConfigData" + configData.Name); ;
            var listObj = FillDynamicListWithConfigData(type, configData);
            byte[] data;
            string suffix;
            switch (format)
            {
                case "bin":
                    data = SerializationHelper.SerializeToBinary(listObj);
                    suffix = ".bin";
                    break;
                case "json":
                    data = SerializationHelper.SerializeToJson(listObj);
                    suffix = ".json";
                    break;
                default:
                    data = SerializationHelper.SerializeToBinary(listObj);
                    suffix = ".bin";
                    break;
            }
            FileStream fs = new FileStream(outPath + "/" + configData.Name + suffix, FileMode.Create);
            fs.Write(data, 0, data.Length);
            fs.Close();
        }

        public void ProcessSingleFile(string filePath, string outPath, string format)
        {

        }

        public void ProcessFolder(string inPath, string outPath, string format)
        {
            //加载ConfigData
            LoadAllConfigDataAtPath(inPath);
            //检查外键约束
            foreach (var pair in m_configDataDic)
            {
                string error;
                if (!CheckForeignKey(pair.Value, out error))
                {
                    Console.WriteLine(error);
                    Console.WriteLine("Check ForeignKey Failed!");
                    return;
                }
            }
            //收集类型
            foreach (var pair in m_configDataDic)
            {
                CollectSubTypeDeclaration(pair.Value);
            }
            CollectSubTypes();
            foreach (var pair in m_configDataDic)
            {
                CollectConfigDataTypeDeclaration(pair.Value);
            }
            CollectConfigDataTypes();
            ConfigDataCodeGenerator codeGenerator = new ConfigDataCodeGenerator();
            codeGenerator.SetCodeFileName("ConfigDataTypeDefine.cs");
            codeGenerator.SetNamespace("bluebean.ConfigData");
            codeGenerator.SetOutPath(outPath + "/Code");
            var typeDeclarationDic = new Dictionary<string, CodeTypeDeclaration>();
            foreach (var pair in m_subTypeDeclarationDic)
            {
                typeDeclarationDic.Add(pair.Key, pair.Value);
            }
            foreach (var pair in m_configDataTypeDeclarationDic)
            {
                typeDeclarationDic.Add(pair.Key, pair.Value);
            }
            codeGenerator.ConstructCompileUnit(typeDeclarationDic);
            //序列化表格资源
            /*
            Assembly assembly;
            codeGenerator.GetAeeembly(out assembly);
            foreach(var pair in m_configDataDic)
            {
                SerializeConfigData(assembly, pair.Value, outPath + "/Data", format);
            }
            */
            //输出代码文件
            codeGenerator.GenerateCode();
        }
    }
}
