using NVelocity;
using NVelocity.App;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace bluebean.UGFramework
{
    /// <summary>
    /// 模板实用类
    /// </summary>
    public static class NVelocityHelper
    {
        public static String Parse(String template, String key, System.Object parameters)
        {
            StringWriter writer = null;
            try
            {
                VelocityEngine ve = new VelocityEngine();
                ve.Init();
                // 取得velocity的上下文context
                VelocityContext context = new VelocityContext();
                // 把数据填入上下文
                context.Put(key, parameters);
                // 输出流
                writer = new StringWriter();
                // todo 转换输出, 去除velocity代码缩进的空格符
                //template = Regex.Replace(template, @"[ ]*(?<value>#if|#end|#foreach|#elseif)", "${value}", RegexOptions.Singleline);
                ve.Evaluate(context, writer, "", template);
                return writer.ToString();
            }
            catch (Exception e)
            { 
                throw e;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }

        }
    }
}

