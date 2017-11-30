using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Mugen3D;

public class TokenerTest : MonoBehaviour
{
    public TextAsset testText;

    void Start()
    {
        Tokenizer t = new Tokenizer();
        List<Token> tokens = t.GetTokens(testText);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < tokens.Count; i++)
        {
            string value = tokens[i].value;
            sb.Append("{" + value + "," + tokens[i].type.ToString() + "}" + "\n");
        }
        Debug.Log(sb.ToString());
    }

}
