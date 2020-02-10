using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Text;

namespace bluebean.CSVParser
{
    /// <summary>
    /// 数据表类型
    /// </summary>
    public enum ConfigDataType
    {
        /// <summary>
        /// 枚举表
        /// </summary>
        ENUM,
        /// <summary>
        /// 数据表
        /// </summary>
        DATA,
    }

    /// <summary>
    /// 配置数据表列信息
    /// </summary>
    public class ConfigDataColumnInfo
    {
        public string m_exportType;
        public string m_name;
        public string m_desc;
        public string m_type;
        public string m_foreignKey;
    }

    public class ConfigData
    {
        public string FilePath
        {
            get
            {
                return m_filePath;
            }
        }

        public ConfigDataType Type { get
            {
                return m_type;
            } 
        }
        public Dictionary<int, ConfigDataColumnInfo> ColumnInfoDic
        {
            get
            {
                return m_columnInfoDic;
            }
        }
        public string Name { get
            {
                return m_name;
            } 
        }

        private string m_filePath;

        /// <summary>
        /// 表名
        /// </summary>
        private string m_name;
        /// <summary>
        /// 说明
        /// </summary>
        private string m_desc;

        ConfigDataType m_type;

        private Dictionary<int, ConfigDataColumnInfo> m_columnInfoDic = new Dictionary<int, ConfigDataColumnInfo>();

        /// <summary>
        /// 枚举表信息
        /// </summary>
        private Dictionary<string, int> m_enumDic = new Dictionary<string, int>();

        public ConfigData(string filePath)
        {
            m_filePath = filePath;
        }

        public void Load(CsvReader csv)
        {
            m_desc = csv.ReadCell(0, 0);
            string typeStr = csv.ReadCell(1, 0);
            if (!Enum.TryParse<ConfigDataType>(typeStr, out m_type)) { 
                //todo
            }
            m_name = csv.ReadCell(1, 1);

            if (m_type == ConfigDataType.DATA)
            {
                //收集表头信息
                m_columnInfoDic.Clear();
                var exportTypeLine = csv.ReadLine(2);
                for (int i = 0; i < exportTypeLine.Length; i++)
                {
                    if (exportTypeLine[i] == "NONE")
                    {
                        continue;
                    }
                    ConfigDataColumnInfo columnInfo = new ConfigDataColumnInfo()
                    {
                        m_exportType = csv.ReadCell(2, i),
                        m_name = csv.ReadCell(3, i),
                        m_desc = csv.ReadCell(4, i),
                        m_type = csv.ReadCell(5, i),
                        m_foreignKey = csv.ReadCell(10, i),
                    };
                    m_columnInfoDic.Add(i, columnInfo);
                }
            }else if(m_type == ConfigDataType.ENUM)
            {
                for(int i = 5; i < csv.Row; i++)
                {
                    m_enumDic.Add(csv.ReadCell(i, 1), int.Parse(csv.ReadCell(i, 0)));
                }
            }
        }

        public bool ContainColumn(string name)
        {
            foreach(var pair in m_columnInfoDic)
            {
                if(pair.Value.m_name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public string GetColumnType(string name)
        {
            foreach (var pair in m_columnInfoDic)
            {
                if (pair.Value.m_name == name)
                {
                    return pair.Value.m_type;
                }
            }
            return "";
        }

        /// <summary>
        /// 构建一个表对应的类的CodeTypeDeclaration,用于产生代码
        /// </summary>
        /// <param name="configData"></param>
        /// <returns></returns>
        public CodeTypeDeclaration BuildCodeTypeDeclaration()
        {
            CodeTypeDeclaration typeDefineClass = new CodeTypeDeclaration("ConfigData" + m_name);
            if (m_type == ConfigDataType.DATA)
            {
                typeDefineClass.CustomAttributes.Add(new CodeAttributeDeclaration(
                    new CodeTypeReference(typeof(SerializableAttribute))));
                typeDefineClass.IsClass = true;
                typeDefineClass.TypeAttributes = System.Reflection.TypeAttributes.Public;
                foreach (var pair in m_columnInfoDic)
                {
                    var columnInfo = pair.Value;
                    string fieldName = "m_" + columnInfo.m_name;
                    Type columnType = ConfigDataManager.Instance.TypeStr2Type(columnInfo.m_type);
                    CodeMemberField field = new CodeMemberField(columnType, fieldName);
                    field.Attributes = MemberAttributes.Private;
                    typeDefineClass.Members.Add(field);
                    CodeMemberProperty property = new CodeMemberProperty();
                    property.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                    property.Name = columnInfo.m_name;
                    property.Type = new CodeTypeReference(columnType);
                    property.HasGet = true;
                    property.HasSet = true;
                    property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName)));
                    property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName), new CodePropertySetValueReferenceExpression()));
                    typeDefineClass.Members.Add(property);
                }
            }
            else if (m_type == ConfigDataType.ENUM)
            {
                typeDefineClass.IsEnum = true;
                typeDefineClass.TypeAttributes = System.Reflection.TypeAttributes.Public;
                foreach (var pair in m_enumDic)
                {
                    typeDefineClass.Members.Add(new CodeSnippetTypeMember(string.Format("{0} = {1}", pair.Key, pair.Value)));
                }

            }
            return typeDefineClass;
        }
    }
}
