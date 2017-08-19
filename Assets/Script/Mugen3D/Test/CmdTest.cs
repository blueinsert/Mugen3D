using UnityEngine;
using System.Collections;
using Mugen3D;
public class CmdTest : MonoBehaviour
{
    CmdManager m = new CmdManager();
    // Use this for initialization
    void Start()
    {
        m.LoadCmdFile(Application.dataPath + "/" + "test.cmd");
    }

    // Update is called once per frame
    void Update()
    {
        uint keycode = GetInputKeycode();

        m.Update(keycode);
        string commandName = m.GetActiveCommandName(); 
        if(commandName!="none")
            Debug.Log("current active command:" + commandName);
    }

    uint GetInputKeycode()
    {
        uint keycode = 0;
        string keyInfo = "";
        foreach (var pair in KeycodeMapConfig.P1)
        {
            if (Input.GetKey(pair.Value))
            {
                keycode = keycode | Utility.GetKeycode(pair.Key);
                keyInfo += pair.Value.ToString() + "+";
            }
        }
        //Debug.Log("keycode:"+keycode);
      
       //Debug.Log(keyInfo);
        return keycode;
    }
}
