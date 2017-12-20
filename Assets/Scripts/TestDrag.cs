using UnityEngine;  
using System.Collections;  
using UnityEditor;  
  
public class TestDrag : EditorWindow {  
  
    string path;  
    Rect rect;  
  
    [MenuItem("Window/TestDrag")]  
    static void Init()  
    {  
        EditorWindow.GetWindow(typeof(TestDrag));  
    }  
    int toggleNum = 5;
    bool[] toggleIsOn = new bool[5];

    void SetToggleGroug(int index) {
        for(int i = 0; i < toggleNum; i++) {
            if(i != index) {
                toggleIsOn[i] = false;
            }
        }
    }

    void OnGUI()  
    {  
       // EditorGUILayout.BeginToggleGroup("groud", true);
       EditorGUILayout.BeginVertical();
        for(int i = 0; i < toggleNum; i++) {
            EditorGUILayout.BeginHorizontal();
            toggleIsOn[i] = EditorGUILayout.Toggle(toggleIsOn[i]);
            EditorGUILayout.LabelField("label");
            EditorGUILayout.EndHorizontal();
            if(toggleIsOn[i]) {
                SetToggleGroug(i);
            }
        }
        EditorGUILayout.EndVertical();
        //EditorGUILayout.EndToggleGroup();
        
        EditorGUILayout.LabelField("路径");  
        //获得一个长300的框  
        rect = EditorGUILayout.GetControlRect(GUILayout.Width(300));  
        //将上面的框作为文本输入框  
        path = EditorGUI.TextField(rect, path);  
  
        //如果鼠标正在拖拽中或拖拽结束时，并且鼠标所在位置在文本输入框内  
        if ((Event.current.type == EventType.DragUpdated  
          || Event.current.type == EventType.DragExited)  
          && rect.Contains(Event.current.mousePosition))  
        {  
            if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)  
            {  if(DragAndDrop.objectReferences[0] is Texture) {
                    //改变鼠标的外表  
                   DragAndDrop.visualMode = DragAndDropVisualMode.Generic;  
                    path = DragAndDrop.objectReferences[0].name; 
                    Texture texture = DragAndDrop.objectReferences[0] as Texture;
                    
                }
                    
            }  
        }  
    }  

     void OnDestroy()
    {
        Debug.Log("Destroyed...");
    }
}  