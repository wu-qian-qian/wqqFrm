using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbState : IState
{
    public List<GameObject> _Targets;
    private Action<Type, object[]> callBack;
    public void InjectCallBack(Action<Type, object[]> callback)
    {
        this.callBack = callback;
    }
    public void Change<T>(params object[] obj) where T : IState
    {
        callBack.Invoke(typeof(T), obj);
    }

    public abstract void Init();
    public abstract void OnEnter(params object[] args);
    public abstract void OnUpdate();
    public abstract void OnExit();
}

