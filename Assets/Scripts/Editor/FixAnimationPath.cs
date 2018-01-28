using UnityEditor;
using UnityEngine;
using System.Collections;

public class FixAnimationPath : EditorWindow
{
    /// <summary>
    /// 需要改变的物体
    /// </summary>
    private GameObject target;

    private string error;

    private AnimationClip ac;

    [MenuItem("Custom/Animation/Fix Animation Path")]
    static void FixAnimationPathMethod()
    {
        Rect wr = new Rect(0, 0, 500, 500);
        FixAnimationPath window = (FixAnimationPath)EditorWindow.GetWindowWithRect(typeof(FixAnimationPath), wr, true, "Fix Animation");
        window.Show();
    }

    bool DoFix()
    {
        //AnimationClip ac = Selection.activeObject as AnimationClip;

        if (ac == null)
            error = "AnimationClip缺失";
        if (target == null)
            error = "Target丢失";

        if (ac != null)
        {
            Debug.Log("Enter ac != null");
            GameObject root = target;
            //获取所有绑定的EditorCurveBinding(包含path和propertyName)
            EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(ac);

            for (int i = 0; i < bindings.Length; ++i)
            {
                EditorCurveBinding binding = bindings[i];

                GameObject bindObj = AnimationUtility.GetAnimatedObject(root, binding) as GameObject;

                if (bindObj == null)
                {
                    bindObj = FindInChildren(root, binding.path);
                    if (bindObj)
                    {
                        string newPath = AnimationUtility.CalculateTransformPath(bindObj.transform, root.transform);
                        Debug.Log("change " + binding.path + " to " + newPath);

                        AnimationCurve curve = AnimationUtility.GetEditorCurve(ac, binding);

                        //remove Old
                        AnimationUtility.SetEditorCurve(ac, binding, null);

                        binding.path = newPath;

                        AnimationUtility.SetEditorCurve(ac, binding, curve);
                    }
                }
            }
        }
        return true;
    }

    GameObject FindInChildren(GameObject obj, string goName)
    {
        Transform objTransform = obj.transform;

        GameObject finded = null;
        Transform findedTransform = objTransform.Find(goName);

        if (findedTransform == null)
        {
            for (int i = 0; i < objTransform.childCount; ++i)
            {
                finded = FindInChildren(objTransform.GetChild(i).gameObject, goName);
                if (finded)
                {
                    return finded;
                }
            }

            return null;
        }

        return findedTransform.gameObject;
    }

    void OnGUI()
    {
        Debug.Log("Fix AnimationClip");
        EditorGUILayout.LabelField("TargetRoot");
        target = EditorGUILayout.ObjectField(target, typeof(GameObject), true) as GameObject;

        EditorGUILayout.LabelField("AnimationClip");
        ac = EditorGUILayout.ObjectField(ac, typeof(AnimationClip), true) as AnimationClip;


        if (GUILayout.Button("Fix", GUILayout.Width(200)))
        {
            if (this.DoFix())
            {
                this.ShowNotification(new GUIContent("Change Complete"));
            }
            else
            {
                this.ShowNotification(new GUIContent("Change Error " + error));
            }
        }
    }

}