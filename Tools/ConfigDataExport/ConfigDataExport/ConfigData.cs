﻿using System;
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
        public ConfigData m_configData;
        public int m_index;
        public string m_exportType;//line 3
        public string m_name;//line 4
        public string m_desc;//line 5
        public string m_type;//line 6
        #region list类型有关
        public string[] m_subTypeParamNames;//line 7
        public string[] m_subTypeParamDescs;//line 8
        public string[] m_subTypeParamTypes;//line 9
        public string[] m_subTypeParamDefaultValues;//line 10
        #endregion
        public string[] m_foreignKeys;//line 11
        public string m_multiplyLanguage;

        public bool IsListType()
        {
            return m_type.StartsWith("LIST");
        }

        public string GetSubTypeName()
        {
            if (!IsListType())
                return "";
            var splits = m_type.Split(new char[] { ':' });
            if (splits.Length == 2)
            {
                return splits[1];
            }
            return "";
        }

        public CodeTypeDeclaration BuildSubTypeDeclaration(string typeName)
        {
            if (!(IsListType() && m_subTypeParamTypes.Length > 1))
            {
                return null;
            }
            CodeTypeDeclaration typeDefineClass = new CodeTypeDeclaration("SubType"+typeName);

            typeDefineClass.CustomAttributes.Add(new CodeAttributeDeclaration(
                new CodeTypeReference(typeof(SerializableAttribute))));
            typeDefineClass.IsClass = true;
            typeDefineClass.TypeAttributes = System.Reflection.TypeAttributes.Public;
            for (int i = 0; i < m_subTypeParamTypes.Length; i++)
            {
                string paramName = m_subTypeParamNames[i];
                string paramTypeStr = m_subTypeParamTypes[i];
                string fieldName = "m_" + paramName;
                Type paramType = ConfigDataManager.Instance.TypeStr2Type(paramTypeStr);
                CodeMemberField field = new CodeMemberField(paramType, fieldName);
                field.Attributes = MemberAttributes.Private;
                typeDefineClass.Members.Add(field);
                CodeMemberProperty property = new CodeMemberProperty();
                property.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                property.Name = paramName;
                property.Type = new CodeTypeReference(paramType);
                property.HasGet = true;
                property.HasSet = true;
                property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName)));
                property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName), new CodePropertySetValueReferenceExpression()));
                typeDefineClass.Members.Add(property);
            }
            return typeDefineClass;
        }

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

        public ConfigDataType Type
        {
            get
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
        public string Name
        {
            get
            {
                return m_name;
            }
        }

        public const int DataStartRow = 12;
        public const int EnumStartRow = 5;
        public const char ListElemSplit = '/';
        public const char ListElemParamSplit = ',';

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
            if (!Enum.TryParse<ConfigDataType>(typeStr, out m_type))
            {
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
                        m_configData = this,
                        m_index = i,
                        m_exportType = csv.ReadCell(2, i),
                        m_name = csv.ReadCell(3, i),
                        m_desc = csv.ReadCell(4, i),
                        m_type = csv.ReadCell(5, i),
                        m_subTypeParamNames = csv.ReadCell(6, i).Split(new char[] { ',' }),
                        m_subTypeParamDescs = csv.ReadCell(7, i).Split(new char[] { ',' }),
                        m_subTypeParamTypes = csv.ReadCell(8, i).Split(new char[] { ',' }),
                        m_subTypeParamDefaultValues = csv.ReadCell(9, i).Split(new char[] { ',' }),
                        m_foreignKeys = csv.ReadCell(10, i).Split(new char[] { ',' }),
                    };
                    m_columnInfoDic.Add(i, columnInfo);
                }
            }
            else if (m_type == ConfigDataType.ENUM)
            {
                for (int i = 5; i < csv.Row; i++)
                {
                    m_enumDic.Add(csv.ReadCell(i, 1), int.Parse(csv.ReadCell(i, 0)));
                }
            }
        }

        public bool ContainColumn(string name)
        {
            foreach (var pair in m_columnInfoDic)
            {
                if (pair.Value.m_name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public ConfigDataColumnInfo GetColumnInfo(string name)
        {
            foreach (var pair in m_columnInfoDic)
            {
                if (pair.Value.m_name == name)
                {
                    return pair.Value;
                }
            }
            return null;
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
                    Type columnType = ConfigDataManager.Instance.GetColumnType(this, columnInfo);
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
