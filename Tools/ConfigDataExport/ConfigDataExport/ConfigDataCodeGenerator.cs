using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.IO;
using System.Linq;

namespace bluebean.CSVParser
{
    public class ConfigDataCodeGenerator
    {
        private Dictionary<string, CodeTypeDeclaration> m_typeDeclarationDic;

        private CodeCompileUnit m_codeUnit;

        private string m_codeFileName;
        private string m_outPath;
        private string m_namespace; 

        public ConfigDataCodeGenerator()
        {
        }

        public void SetNamespace(string namespaceStr)
        {
            m_namespace = namespaceStr;
        }

        public void SetOutPath(string outPath)
        {
            m_outPath = outPath;
        }

        public void SetCodeFileName(string fileName)
        {
            m_codeFileName = fileName;
        }

        /// <summary>
        /// 将m_codeUnit代表的代码输出指定文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string GenerateCSharpCode(string filePath)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            string sourceFile = filePath;
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (StreamWriter sw = new StreamWriter(sourceFile, false))
            {
                IndentedTextWriter tw = new IndentedTextWriter(sw, "    ");
                provider.GenerateCodeFromCompileUnit(m_codeUnit, tw,
                    new CodeGeneratorOptions());
                tw.Close();
            }
            return sourceFile;
        }

        /// <summary>
        /// Step 1: ConstructCompileUnit
        /// </summary>
        public void ConstructCompileUnit(Dictionary<string, CodeTypeDeclaration> typeDeclarationDic)
        {
            m_typeDeclarationDic = typeDeclarationDic;
            m_codeUnit = new CodeCompileUnit();
            CodeNamespace nameSpace = new CodeNamespace(m_namespace);
            m_codeUnit.Namespaces.Add(nameSpace);
            foreach (var pair in m_typeDeclarationDic)
            {
                nameSpace.Types.Add(pair.Value);
            }
        }

        /// <summary>
        /// Step 2.1:从m_codeUnit中产生对应的Assembly,用于运行时序列化
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public bool GetAeeembly(out Assembly assembly)
        {
            assembly = null;
            var compiler = new CSharpCodeProvider();
            var comPara = new CompilerParameters();
            comPara.GenerateExecutable = false;
            comPara.GenerateInMemory = true;
            comPara.OutputAssembly = "";
            var assemblies = AppDomain.CurrentDomain
                            .GetAssemblies()
                            .Where(a => !a.IsDynamic)
                            .Select(a => a.Location);

            comPara.ReferencedAssemblies.AddRange(assemblies.ToArray());
            comPara.OutputAssembly = "Assembly-CSharp";
            var result = compiler.CompileAssemblyFromDom(comPara, m_codeUnit);
            if (result.Errors.HasErrors)
            {
                foreach(var error in result.Errors)
                {
                    Console.WriteLine(error.ToString());
                }
                return false;
            }
            assembly = result.CompiledAssembly;
            return true;
        }

        /// <summary>
        /// Step 2.2: 产生代码
        /// </summary>
        public void GenerateCode()
        {
            GenerateCSharpCode(m_outPath + "/" + m_codeFileName);
        }
    }
}
