using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Mugen3D;

public class ExpressionSolveTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Tokenizer t = new Tokenizer();
        List<Token> tokens = t.GetTokens(Application.dataPath + "/" + "ExpressTest.txt");
        Utility.PrintTokens(tokens.ToArray());
      
        Expression expression = new Expression(tokens,false);
        //Utility.PrintTokens(expression.Infix2PostFix(t.Tokens).ToArray());
        VirtualMachine vm = new VirtualMachine();
        Debug.Log("result:" + vm.Execute(expression));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void PrintTokens() { 
    }
}
