using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class YamlTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Mugen3D.Action action = new Mugen3D.Action();
        action.animName = "idle";
        action.animNo = 0;
        action.loopStartIndex = 5;
        action.frames = new List<Mugen3D.ActionFrame>();
        Mugen3D.ActionFrame frame1 = new Mugen3D.ActionFrame();
        frame1.normalizeTime = 0.3f;
        frame1.clsns = new List<Mugen3D.Clsn>();
        Mugen3D.Clsn clsn1 = new Mugen3D.Clsn(1, 0, 1, 1, 0);
        Mugen3D.Clsn clsn2 = new Mugen3D.Clsn();
        frame1.clsns.Add(clsn1);
        frame1.clsns.Add(clsn2);
        action.frames.Add(frame1);

        YamlDotNet.Serialization.Serializer serializer = new Serializer();
        StringWriter strWriter = new StringWriter();

        serializer.Serialize(strWriter, action);

        using (TextWriter writer = File.CreateText("action.yml"))
        {
            writer.Write(strWriter.ToString());
        }

        StringReader strReader;
        using (TextReader reader = File.OpenText("action.yml"))
        {
             strReader = new StringReader(reader.ReadToEnd());
        }
        var deserializer = new DeserializerBuilder()
               .WithNamingConvention(new CamelCaseNamingConvention())
               .Build();
        var action2 = deserializer.Deserialize<Mugen3D.Action>(strReader);
        print(action2.animName);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
