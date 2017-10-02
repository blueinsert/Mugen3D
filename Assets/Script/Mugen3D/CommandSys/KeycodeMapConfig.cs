using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mugen3D;
public class KeycodeMapConfig
{
    public static Dictionary<KeyNames, KeyCode> P1 = new Dictionary<KeyNames, KeyCode> { 
                                                    {KeyNames.KEY_UP,KeyCode.W},
                                                    {KeyNames.KEY_DOWN,KeyCode.S},
                                                    {KeyNames.KEY_LEFT,KeyCode.A},
                                                    {KeyNames.KEY_RIGHT,KeyCode.D},
                                                    {KeyNames.KEY_BUTTON_A,KeyCode.J},
                                                    {KeyNames.KEY_BUTTON_B,KeyCode.K},
                                                    {KeyNames.KEY_BUTTON_C,KeyCode.L},
                                                    {KeyNames.KEY_BUTTON_X,KeyCode.U},
                                                    {KeyNames.KEY_BUTTON_Y,KeyCode.I},
                                                    {KeyNames.KEY_BUTTON_Z,KeyCode.O},
                                                    {KeyNames.KEY_BUTTON_PAUSE,KeyCode.Alpha2},
                                                    {KeyNames.KEY_BUTTON_START,KeyCode.Alpha1}};

}
