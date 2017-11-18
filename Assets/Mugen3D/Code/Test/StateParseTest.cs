using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;

public class StateParseTest : MonoBehaviour {
    public TextAsset testTest;
	// Use this for initialization
	void Start () {
        Tokenizer t = new Tokenizer();
        List<Token> tokens = t.GetTokens(testTest);
        StateParse p = new StateParse();
        p.Parse(tokens);
        MyDictionary<int, PlayerStateDef> states = p.States;
        Debug.Log(states.ToString());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
