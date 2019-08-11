using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.Mugen3D.ClientGame
{
    public class ResourceLoader
    {

        public static T Load<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

        public static T[] LoadAll<T>(string path) where T : Object
        {
            return Resources.LoadAll<T>(path);
        }

         public static Object Load(string path)
        {
            return Load<Object>(path);
        }
  
         public static void UnloadUnusedAssets()
         {
             Resources.UnloadUnusedAssets();
         }

         public static string LoadText(string path)
         {
             TextAsset text = Load<TextAsset>(path);
             return text.text;
         }

    }
}
