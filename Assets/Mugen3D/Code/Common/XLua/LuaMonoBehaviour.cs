
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LuaMonoBehaviour : MonoBehaviour {
    public delegate void MonoBehaviourEvent(XLua.LuaTable self);
    public delegate void LuaBehaviourInit(XLua.LuaTable self, Transform tran);
    private MonoBehaviourEvent mLuaUpdate;
    private MonoBehaviourEvent mLuaFixedUpdate;
    private MonoBehaviourEvent mLuaLateUpdate;
    private MonoBehaviourEvent mLuaOnDisable;
    private MonoBehaviourEvent mLuaOnEnable;
    private MonoBehaviourEvent mLuaOnDestroy;
    private MonoBehaviourEvent mLuaOnGUI;
    protected LuaBehaviourInit mLuaInit;

    protected XLua.LuaTable m_luaBehaviour;

    public XLua.LuaTable LuaView {
        get {
            return m_luaBehaviour;
        }
    }

    public void SetLuaModule(XLua.LuaTable module) {
        m_luaBehaviour   = module;
        mLuaUpdate      = module.Get<string,MonoBehaviourEvent>("Update");
        mLuaFixedUpdate = module.Get<string,MonoBehaviourEvent>("FixedUpdate");
        mLuaLateUpdate  = module.Get<string,MonoBehaviourEvent>("LateUpdate");
        mLuaOnDisable   = module.Get<string,MonoBehaviourEvent>("OnDisable");
        mLuaOnEnable    = module.Get<string,MonoBehaviourEvent>("OnEnable");
        mLuaOnDestroy   = module.Get<string,MonoBehaviourEvent>("OnDestroy");
        mLuaOnGUI       = module.Get<string,MonoBehaviourEvent>("OnGUI");
        mLuaInit        = module.Get<string, LuaBehaviourInit>("Init");

    }   

    public virtual void Init() {
        if (mLuaInit != null)
        {
            mLuaInit(m_luaBehaviour, this.transform);
        }
    }

    protected virtual void Update() {
        if (mLuaUpdate != null) {
            mLuaUpdate(m_luaBehaviour);
        }
    }

    void FixedUpdate() {
        if (mLuaFixedUpdate != null) {
            mLuaFixedUpdate(m_luaBehaviour);
        }
    }

    void LateUpdate() {
        if (mLuaLateUpdate != null) {
            mLuaLateUpdate(m_luaBehaviour);
        }
    }

    void OnDisable() {
        if (mLuaOnDisable != null) {
            mLuaOnDisable(m_luaBehaviour);
        }
    }

    void OnEnable() {
        if (mLuaOnEnable != null) {
            mLuaOnEnable(m_luaBehaviour);
        }
    }

    protected virtual void OnDestroy() {
        if (mLuaOnDestroy != null) {
            mLuaOnDestroy(m_luaBehaviour);
        }
        mLuaUpdate = null;
        mLuaOnDestroy = null;
        m_luaBehaviour.Dispose();
        m_luaBehaviour = null;
    }

    void OnGUI() {
        if (mLuaOnGUI != null) {
            mLuaOnGUI(m_luaBehaviour);
        }
    }
}

