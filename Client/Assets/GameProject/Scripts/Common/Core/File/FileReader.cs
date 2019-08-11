using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bluebean.Mugen3D.Core
{
    public delegate string CustomFileReader(ref string filepath);

    public class FileReader
    {
        private static List<CustomFileReader> readers = new List<CustomFileReader>();
        public static void AddReader(CustomFileReader reader)
        {
            readers.Add(reader);
        }

        public static string Read(string fileName)
        {
            foreach (var reader in readers)
            {
                string content = reader(ref fileName);
                if (content != null)
                {
                    return content;
                }
            }
            return null;
        }
        
    }

}
