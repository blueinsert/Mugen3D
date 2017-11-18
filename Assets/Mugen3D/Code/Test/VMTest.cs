using UnityEngine;
using System.Collections;
using Mugen3D;
public class VMTest : MonoBehaviour {
    Instruction[] addIns = {
            new Instruction(OpCode.PushValue,1),
            new Instruction(OpCode.PushValue,-2),
            new Instruction(OpCode.AddOP,0),
        };
    Instruction[] mulIns = {
            new Instruction(OpCode.PushValue,2),
            new Instruction(OpCode.PushValue,3),
            new Instruction(OpCode.MulOP,0),
        };
	// Use this for initialization
	void Start () {
        VirtualMachine vm = new VirtualMachine();
        vm.InitFuncTable();
        double result = vm.Execute(addIns);
        Debug.Log("add result:"+result);
        result = vm.Execute(mulIns);
        Debug.Log("mul result:"+result);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
