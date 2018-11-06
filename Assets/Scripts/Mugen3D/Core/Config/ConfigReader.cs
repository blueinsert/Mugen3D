using System.Collections;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Mugen3D.Core
{
    public class ConfigReader
    {
        protected static Deserializer deserializer = new DeserializerBuilder()
               .WithNamingConvention(new CamelCaseNamingConvention())
               .Build();

        public static T Parse<T>(string content)
        {
            StringReader strReader = new StringReader(content);
            var result = deserializer.Deserialize<T>(strReader);
            return result;
        }
    }//class 
}
