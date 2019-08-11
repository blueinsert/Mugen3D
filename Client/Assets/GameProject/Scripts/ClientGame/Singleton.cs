
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T> {

    private static T mInstance = null;

    public static T Instance {
		get {
	        if (mInstance == null) {
	#if UNITY_EDITOR
	            if (FindObjectsOfType(typeof(T)).Length > 1) {
	                Debug.Log("Mutiple instances exist of type " + typeof(T).ToString());
                }
	#endif
	            mInstance = FindObjectOfType(typeof(T)) as T;
	            if (mInstance == null) {
	                GameObject go = new GameObject();
	                go.name = "__" + typeof(T).Name;
	                mInstance = go.AddComponent(typeof(T)) as T;
                    mInstance.DontDestroySingleton();
	            }
	        }
	        return mInstance;
		}
    }

    public static bool IsExist() {
        return mInstance != null;
    }

    protected virtual void OnDestroy() {
        if (mInstance == this) {
            mInstance = null;
        }
    }

    protected void DontDestroySingleton() {
        DontDestroyOnLoad(gameObject);
    }
}
