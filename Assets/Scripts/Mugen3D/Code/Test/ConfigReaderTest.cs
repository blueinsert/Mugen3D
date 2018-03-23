using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;

public class ConfigReaderTest : MonoBehaviour {
    public TextAsset configFile;
	// Use this for initialization
	void Start () {
        var config = new ConfigReader().GetConfig(configFile.text);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
