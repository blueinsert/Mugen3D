using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace bluebean.UGFramework.ConfigData
{
    /// <summary>
    /// 配置数据表列信息
    /// </summary>
    public class ConfigDataTableColumnInfo
    {
        public string TableName { get; private set; }
        public string ColumnName { get; private set; }
        public int ColumnNo { get; private set; }
        public string TypeStr { get; private set; }
        public string RealTypeStr { get; set; }

        public ConfigDataTableColumnInfo(string tableName, string columnName, string typeStr, int columnNo)
        {
            TableName = tableName;
            ColumnName = columnName;
            TypeStr = typeStr;
            RealTypeStr = typeStr;
            ColumnNo = columnNo;
        }
    }

    /// <summary>
    /// 数据表类型
    /// </summary>
    public enum ConfigDataTabelType
    {
        /// <summary>
        /// 枚举表
        /// </summary>
        EnumTable,
        /// <summary>
        /// 数据表
        /// </summary>
        DataTable,
    }

    /// <summary>
    /// 配置数据表信息
    /// </summary>
    public class ConfigDataTableInfo
    {
        public List<ConfigDataTableColumnInfo> ColumnInfoList { get { return m_columnInfoList; } }
        public string TableName { get { return m_tableName; } }
        public ConfigDataTabelType? TableType { get { return m_tableType; } }
        public Dictionary<string, int> EnumTupleDic { get { return m_enumTupleDic; } }

        /// <summary>
        /// 数据表文件路径
        /// </summary>
        private string m_filePath;
        /// <summary>
        /// 表名
        /// </summary>
        private string m_tableName;
        /// <summary>
        /// 表类型
        /// </summary>
        private ConfigDataTabelType? m_tableType = null;
        /// <summary>
        /// 列信息列表
        /// </summary>
        private readonly List<ConfigDataTableColumnInfo> m_columnInfoList = new List<ConfigDataTableColumnInfo>();
        
        /// <summary>
        /// 原始数据读取借口
        /// </summary>
        public IRawDataReader m_rawDataReader = new CsvReader();
        
        /// <summary>
        /// 枚举表 枚举名-值 字典
        /// </summary>
        private Dictionary<string, int> m_enumTupleDic = new Dictionary<string, int>();

        public int Row
        {
            get { return m_rawDataReader.Row; }
        }

        public int Column
        {
            get { return m_rawDataReader.Column; }
        }

        public string[] ReadLine(int n)
        {
            return m_rawDataReader.ReadLine(n);
        }

        public string ReadCell(int x, int y)
        {
            return m_rawDataReader.ReadCell(x, y);
        }

        public ConfigDataTableColumnInfo GetColumnInfoByName(string name)
        {
            foreach (var columnInfo in m_columnInfoList)
            {
                if (columnInfo.ColumnName == name)
                {
                    return columnInfo;
                }
            }
            return null;
        }

        /// <summary>
        /// 解析表信息
        /// </summary>
        private void ParseTableInfo()
        {
            //第0行，第0列定义了表的名字和类型
            string content = m_rawDataReader.ReadCell(0, 0);
            string[] tuple = content.Split(new char[] { ':' });
            if (tuple.Length != 2)
            {
                ThrowException("table at [0,0] is not a tuple");
            }
            string tableName = tuple[0];
            string tableTypeName = tuple[1];
            if (!ConfigDataHelper.IsValidVariableName(tableName))
            {
                ThrowException(string.Format("'{0}' is not a valid table name", tableName));
            }
            m_tableName = tableName;
            m_tableType = (ConfigDataTabelType)Enum.Parse(typeof(ConfigDataTabelType), tableTypeName);
            if (m_tableType == null)
            {
                ThrowException(string.Format("{0} is not a valid table type", tableTypeName));
            }
        }

        /// <summary>
        /// 解析列信息
        /// </summary>
        private void ParseColumnDef()
        {
            //第2行定义了列数据的变量名和类型
            int columnDefRow = 2;
            var columnDefStrList = m_rawDataReader.ReadLine(columnDefRow - 1);
            m_columnInfoList.Clear();
            for (int i = 0; i < columnDefStrList.Length; i++)
            {
                var columnDefStr = columnDefStrList[i];
                //如果该列定义以“#”开头，忽略该列的导出
                if (columnDefStr.StartsWith("#"))
                {
                    continue;
                }
                string[] tuple = columnDefStr.Split(new char[] { ':' });
                if (tuple.Length != 2)
                {
                    ThrowException(string.Format("the column def {0} at[{1},{2}] is not a tuple", columnDefStr, columnDefRow, i));
                }
                var columnName = tuple[0];
                var columnTypeStr = tuple[1];
                if (!ConfigDataHelper.IsValidVariableName(columnName))
                {
                    ThrowException(string.Format("the column variable name {0} at[{1},{2}] is not valid", columnName, columnDefRow, i));
                }
                if (string.IsNullOrEmpty(tuple[1]))
                {
                    ThrowException(string.Format("the column's type str {0} at[{1},{2}] is null", columnTypeStr, columnDefRow, i));
                }
                m_columnInfoList.Add(new ConfigDataTableColumnInfo(m_tableName, columnName, columnTypeStr, i));
            }
        }

        /// <summary>
        /// 检查表的必要列信息是否缺失
        /// </summary>
        private void CheckNecessaryColumnInfos()
        {
            if (m_tableType == ConfigDataTabelType.EnumTable)
            {
                if (m_columnInfoList.Count < 2)
                {
                    ThrowException("enum table's column < 2");
                }
                if (m_columnInfoList[0].ColumnName != "ID" || m_columnInfoList[0].TypeStr != "string")
                {
                    ThrowException("the 1st column of enum table should be named as 'ID' and type is 'string'");
                }
                if (m_columnInfoList[1].ColumnName != "Value" || m_columnInfoList[1].TypeStr != "int")
                {
                    ThrowException("the 2nd column of enum table should be named as 'Value' and type is 'int'");
                }
            }
            else if (m_tableType == ConfigDataTabelType.DataTable)
            {
                if (m_columnInfoList.Count < 2)
                {
                    ThrowException("data table's column < 2");
                }
                if (m_columnInfoList[0].ColumnName != "ID" || m_columnInfoList[0].TypeStr != "int")
                {
                    ThrowException("the 1st column of data table should be named as 'ID' and type is 'int'");
                }
            }
        }

        /// <summary>
        /// 提取枚举表信息
        /// </summary>
        private void ParseEnumTableInfo()
        {
            m_enumTupleDic.Clear();
            if (m_rawDataReader.Row > 2)
            {
                for (int i = 2; i < m_rawDataReader.Row; i++)
                {
                    string enumName = m_rawDataReader.ReadCell(i, 0);
                    int value = ConfigDataValueConverter.ParseInt(m_rawDataReader.ReadCell(i, 1));
                    if (m_enumTupleDic.ContainsKey(enumName))
                    {
                        ThrowException("enum table has contained the same key:" + enumName);
                    }
                    m_enumTupleDic.Add(enumName, value);
                }
            }
        }

        public void Initialize(string filePath)
        {
            m_filePath = filePath;
            try
            {
                m_rawDataReader.ParseFromFile(filePath);
                ParseTableInfo();
                ParseColumnDef();
                CheckNecessaryColumnInfos();
                if (m_tableType == ConfigDataTabelType.EnumTable)
                {
                    ParseEnumTableInfo();
                }
            }
            catch
            {
                throw;
            }
        }

        private void ThrowException(string msg)
        {
            throw new Exception(string.Format("parse file at{0}, error:{1}", m_filePath, msg));
        }

    }
}

