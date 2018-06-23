
using UnityEngine;
using System.Collections;

public class LuaMgr : MonoBehaviour {

    private static LuaMgr mInstance;

    private float mLastGCTime = 0;
    private const float GC_INTERVAL = 60f;

    private LuaMgr() { }

    public static LuaMgr Instance {
        get {
            if(mInstance == null) {
                Init();
            }
            return mInstance;
        }
    }

    public XLua.LuaEnv Env { get; private set; }

    private static void Init() {
        if(mInstance == null) {
            var go = new GameObject("__LuaMgr");
            DontDestroyOnLoad(go);
            mInstance = go.AddComponent<LuaMgr>();
            mInstance.Env = new XLua.LuaEnv();
            mInstance.Env.AddLoader(LuaMgr.LuaLoader);
        }
    }

    public static void Deinit() {
      
    }
 
    private void Update() {
        if(Time.time - mLastGCTime > GC_INTERVAL) {
            Env.Tick();
            mLastGCTime = Time.time;
        }
    }

    private static byte[] LuaLoader(ref string filename) {
        string path = filename.Replace(".", "/");
        //var code = Resources.Load("Lua/" + path + ".lua", typeof(TextAsset)) as TextAsset;
        var code = Resources.Load(path + ".lua", typeof(TextAsset)) as TextAsset;
        if(code != null) {
            return code.bytes;
        }
        return null;
    }
}

