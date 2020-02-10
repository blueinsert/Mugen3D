using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace bluebean.CSVParser
{
    public delegate object GetValueByStr(string valeuStr);

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

        private Dictionary<string, CsvReader> m_csvDic = new Dictionary<string, CsvReader>();

        private Dictionary<string, ConfigData> m_configDataDic = new Dictionary<string, ConfigData>();

        public Dictionary<string, Type> m_typeDic = new Dictionary<string, Type>();

        public Dictionary<string, GetValueByStr> m_valueParserDic = new Dictionary<string, GetValueByStr>();

        private void CollectTypeStrToTypeDic()
        {
            m_typeDic.Add("int16", typeof(Int16));
            m_typeDic.Add("int32", typeof(Int32));
            m_typeDic.Add("int64", typeof(Int64));
            m_typeDic.Add("uint16", typeof(UInt16));
            m_typeDic.Add("uint32", typeof(UInt32));
            m_typeDic.Add("uint64", typeof(UInt64));
            m_typeDic.Add("double", typeof(Double));
            m_typeDic.Add("string", typeof(String));
            m_typeDic.Add("bool", typeof(Boolean));
            m_typeDic.Add("float", typeof(float));
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

        public Type TypeStr2Type(string typeStr)
        {
            typeStr = typeStr.ToLower();
            if (m_typeDic.ContainsKey(typeStr))
            {
                return m_typeDic[typeStr];
            }
            return null;
        }

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

        private ConfigData LoadConfigData(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            if (m_configDataDic.ContainsKey(filePath))
            {
                return m_configDataDic[filePath];
            }
            ConfigData configData = new ConfigData(filePath);
            CsvReader csv = LoadCSV(filePath);
            configData.Load(csv);
            m_configDataDic.Add(filePath, configData);
            return configData;
        }


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
            FileStream fs = new FileStream(outPath + "/" + configData.Name + suffix,FileMode.Create);
            fs.Write(data, 0, data.Length);
            fs.Close();
        }

        public void ProcessSingleFile(string filePath, string outPath, string format)
        {
            var configData = LoadConfigData(filePath);
            ConfigDataCodeGenerator codeGenerator = new ConfigDataCodeGenerator(m_configDataDic);
            codeGenerator.SetCodeFileName("ConfigDataTypeDefine.cs");
            codeGenerator.SetNamespace("bluebean");
            codeGenerator.SetOutPath(outPath);
            codeGenerator.ConstructCompileUnit();
            Assembly assembly;
            codeGenerator.GetAeeembly(out assembly);
            SerializeConfigData(assembly, configData, outPath, format);
            codeGenerator.GenerateCode();

        }
    }
}
