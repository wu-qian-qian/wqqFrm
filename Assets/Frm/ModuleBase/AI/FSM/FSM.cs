using System;
using System.Collections.Generic;
using UnityEngine;

public class FSM : IFSM
{
    private List<GameObject> _Targets;
    private Dictionary<Type, AbState> stateStock;
    private Animator _Animator;
    private GameObject _Player;
    private AbState RunningState { get; set; }

    public Action<GameObject> actAdd;
    public Action<GameObject> actRemove;
    public FSM(Animator animator, GameObject player)
    {
        _Player = player;
        _Animator = animator;
        _Targets = new List<GameObject>();
        stateStock = new Dictionary<Type, AbState>();
        actAdd = SeekAdd;
        actRemove = SeekRemove;
    }
    public void Add<T>() where T : AbState
    {
        Type type = typeof(T);
        if (stateStock.ContainsKey(type))
        {
            Debug.LogError("状态已注册");
            return;
        }
        AbState state =(AbState)Activator.CreateInstance(type);
        state.InjectCallBack(new Action<Type, object[]>(Change));
        stateStock.Add(type, state);
        state.Init();
    }
    public void SeekAdd(GameObject go)
    {
        if (!_Targets.Contains(go))
        {
            _Targets.Add(go);
            RunningState._Targets.Add(go);
        }
    }
    public void SeekRemove(GameObject go)
    {
        if (_Targets.Contains(go))
        {
            _Targets.Remove(go);
            RunningState._Targets.Remove(go);
        }
    }
    private void Change(Type type,params object[] obj)
    {
        if(!stateStock.TryGetValue(type,out AbState state))
        {
            Debug.LogError("该状态未注册");
            //TODO可以调用Add进行注册
            return;
        }
        if (RunningState == state)
            return;
        if (RunningState != null)
            state.OnExit();
        RunningState = state;
        RunningState.OnEnter(obj);
    }
    public void Change<T>(params object[] obj) where T : AbState => Change(typeof(T),obj);

    public void Remove<T>() where T : AbState
    {
        Type type = typeof(T);

        if (!stateStock.TryGetValue(type, out AbState state))
        {
            Debug.LogWarning("State does not exist:" + type.Name);
            return;
        }

        if (state == RunningState)
            RunningState = null;

        stateStock.Remove(type);
    }

    public void Clear()
    {
        RunningState = null;
        stateStock.Clear();
    }

    

    public void Update() => RunningState?.OnUpdate();
}

