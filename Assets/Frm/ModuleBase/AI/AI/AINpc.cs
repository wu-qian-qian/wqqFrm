using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// AI系统
/// </summary>
public class AINpc : AbsAINpc
{
    public Animator animator;
    protected override void Awake()
    {
        _Fsm = new FSM(animator,this.gameObject);
    }

    protected override void Start()
    {
        _Target = new List<GameObject>();
        Type type = _Fsm.GetType();
        _ActAdd = type.GetMethod("SeekAdd");
        _ActRemove = type.GetMethod("SeekRemove");
 
    }
    
}
