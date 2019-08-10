using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using UnityEngine;
using Microsoft.CSharp;
using System.IO;

namespace bluebean.UGFramework.ConfigData
{
    public class ConfigDataTypeDefineCodeGenerator
    {
        private Dictionary<string, ConfigDataTableInfo> m_configDataTableInfoDic;
        private ConfigDataSetting m_setting;
        private CodeCompileUnit m_codeUnit;

        public ConfigDataTypeDefineCodeGenerator(Dictionary<string, ConfigDataTableInfo> configDataTableInfoDic, ConfigDataSetting setting)
        {
            m_configDataTableInfoDic = configDataTableInfoDic;
            m_setting = setting;
        }

        private string GenerateCSharpCode(string fileName, CodeCompileUnit compileunit)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            string sourceFile = fileName;
            string directory = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (StreamWriter sw = new StreamWriter(sourceFile, false))
            {
                IndentedTextWriter tw = new IndentedTextWriter(sw, "    ");
                provider.GenerateCodeFromCompileUnit(compileunit, tw,
                    new CodeGeneratorOptions());
                tw.Close();
            }
            return sourceFile;
        }

        private CodeTypeDeclaration BuildTableTypeClass(ConfigDataTableInfo tableInfo)
        {
            CodeTypeDeclaration typeDefineClass = new CodeTypeDeclaration("ConfigData" + tableInfo.TableName);
            if(tableInfo.TableType == ConfigDataTabelType.DataTable)
            {
                typeDefineClass.CustomAttributes.Add(new CodeAttributeDeclaration(
                    new CodeTypeReference(typeof(SerializableAttribute))));
                typeDefineClass.IsClass = true;
                typeDefineClass.TypeAttributes = System.Reflection.TypeAttributes.Public;
                foreach(var columnInfo in tableInfo.ColumnInfoList)
                {
                    string fieldName = "m_" + columnInfo.ColumnName;
                    CodeMemberField field = new CodeMemberField(columnInfo.ColumnType, fieldName);
                    field.Attributes = MemberAttributes.Private;
                    typeDefineClass.Members.Add(field);
                    CodeMemberProperty property = new CodeMemberProperty();
                    property.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                    property.Name = columnInfo.ColumnName;
                    property.Type = new CodeTypeReference(columnInfo.ColumnType);
                    property.HasGet = true;
                    property.HasSet = true;
                    property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName)));
                    property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName), new CodePropertySetValueReferenceExpression()));
                    typeDefineClass.Members.Add(property);
                }
            }
            else if(tableInfo.TableType == ConfigDataTabelType.EnumTable)
            {
                typeDefineClass.IsEnum = true;
                typeDefineClass.TypeAttributes = System.Reflection.TypeAttributes.Public;
                foreach (var pair in tableInfo.EnumTupleDic)
                {
                    typeDefineClass.Members.Add(new CodeSnippetTypeMember(string.Format("{0} = {1}", pair.Key, pair.Value)));
                }
                
            }
            return typeDefineClass;
        }

        public void ConstructCompileUnit()
        {
            m_codeUnit = new CodeCompileUnit();
            CodeNamespace nameSpace = new CodeNamespace(m_setting.m_codeNameSpace);
            m_codeUnit.Namespaces.Add(nameSpace);
            foreach (var pair in m_configDataTableInfoDic)
            {
                nameSpace.Types.Add(BuildTableTypeClass(pair.Value));
            }
        }

        public void GenerateCode()
        {
            string outputFolder = PathHelper.GetFullPath(m_setting.m_autoGenCodeOutputPath);
            GenerateCSharpCode(outputFolder + "/ConfigDataTypeDefine.cs", m_codeUnit);
        }

        public Assembly GetAeeembly()
        {
            var compiler = new CSharpCodeProvider();
            var comPara = new CompilerParameters();
            comPara.GenerateExecutable = false;
            comPara.GenerateInMemory = true;
            comPara.OutputAssembly = "Assembly-CSharp";
            var result = compiler.CompileAssemblyFromDom(comPara, m_codeUnit);
            return result.CompiledAssembly;
        }
    }
}
