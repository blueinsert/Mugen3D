using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = bluebean.UGFramework.Log.Debug;

namespace bluebean.UGFramework
{

    /// <summary>
    /// 路径辅助类
    /// </summary>
    public static class PathHelper
    {
        /// <summary>
        /// 去掉文件扩展名
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public static string RemoveExtension(string assetPath)
        {
            int dotIndex = assetPath.LastIndexOf(".");
            if (dotIndex == -1)
            {
                Debug.LogError("RemoveExtension error: can't find '.'");
                return null;
            }
            string path = assetPath.Substring(0, dotIndex);
            return path;
        }

        /// <summary>
        /// 解析出相对Resources的路径
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public static string GetSubPathInResources(string assetPath)
        {
            int index = assetPath.LastIndexOf("Resources");
            if (index == -1)
            {
                Debug.LogError("GetSubPathInResources error: can't find 'Resources'");
                return null;
            }
            index += "Resources".Length;
            string path = assetPath.Substring(index);
            return path;
        }

        /// <summary>
        /// 获取完整路径
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public static string GetFullPath(string assetPath)
        {
            if (!assetPath.StartsWith("Assets"))
            {
                Debug.LogError(string.Format("GetFullPath assetPath:{0} is not start with 'Assets'", assetPath));
                return null;
            }
            assetPath = assetPath.Substring("Assets".Length);
            return Application.dataPath + assetPath;
        }

    }
}
