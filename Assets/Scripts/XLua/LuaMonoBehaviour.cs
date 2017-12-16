
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LuaMonoBehaviour : MonoBehaviour {
    public delegate void MonoBehaviourEvent(XLua.LuaTable self);
    public delegate void LuaBehaviourInit(XLua.LuaTable self, Transform transform);
    private MonoBehaviourEvent mLuaStart;
    private MonoBehaviourEvent mLuaUpdate;
    private MonoBehaviourEvent mLuaFixedUpdate;
    private MonoBehaviourEvent mLuaLateUpdate;
    private MonoBehaviourEvent mLuaOnDisable;
    private MonoBehaviourEvent mLuaOnEnable;
    private MonoBehaviourEvent mLuaOnDestroy;
    private MonoBehaviourEvent mLuaOnGUI;
    private LuaBehaviourInit mLuaInit;

    protected XLua.LuaTable mLuaBehaviour;

    public XLua.LuaTable LuaBehaviour {
        get {
            return mLuaBehaviour;
        }
    }

    public void SetLuaModule(XLua.LuaTable module) {
        mLuaBehaviour   = module;
        mLuaStart       = module.Get<string,MonoBehaviourEvent>("Start");
        mLuaUpdate      = module.Get<string,MonoBehaviourEvent>("Update");
        mLuaFixedUpdate = module.Get<string,MonoBehaviourEvent>("FixedUpdate");
        mLuaLateUpdate  = module.Get<string,MonoBehaviourEvent>("LateUpdate");
        mLuaOnDisable   = module.Get<string,MonoBehaviourEvent>("OnDisable");
        mLuaOnEnable    = module.Get<string,MonoBehaviourEvent>("OnEnable");
        mLuaOnDestroy   = module.Get<string,MonoBehaviourEvent>("OnDestroy");
        mLuaOnGUI       = module.Get<string,MonoBehaviourEvent>("OnGUI");
        mLuaInit        = module.Get<string, LuaBehaviourInit>("Init");
    }

    void Start() {
        if (mLuaInit != null)
        {
            mLuaInit(mLuaBehaviour, transform);
        }
        if (mLuaStart != null) {
            mLuaStart(mLuaBehaviour);
        }
    }

    void Update() {
        if (mLuaUpdate != null) {
            mLuaUpdate(mLuaBehaviour);
        }
    }

    void FixedUpdate() {
        if (mLuaFixedUpdate != null) {
            mLuaFixedUpdate(mLuaBehaviour);
        }
    }

    void LateUpdate() {
        if (mLuaLateUpdate != null) {
            mLuaLateUpdate(mLuaBehaviour);
        }
    }

    void OnDisable() {
        if (mLuaOnDisable != null) {
            mLuaOnDisable(mLuaBehaviour);
        }
    }

    void OnEnable() {
        if (mLuaOnEnable != null) {
            mLuaOnEnable(mLuaBehaviour);
        }
    }

    void OnDestroy() {
        if (mLuaOnDestroy != null) {
            mLuaOnDestroy(mLuaBehaviour);
        }
        mLuaBehaviour = null;
        mLuaUpdate = null;
        mLuaOnDestroy = null;
        mLuaStart = null;
    }

    void OnGUI() {
        if (mLuaOnGUI != null) {
            mLuaOnGUI(mLuaBehaviour);
        }
    }
}

