using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrontIntermediares : absDrontIntermediares
{
    public DrontIntermediares()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        Coroutine coroutine = null;
        coroutine = GameLogin.StartCoroutine(Enumerator(() => GameLogin.StopCoroutine(coroutine)));
    }
    public override void Updata()
    {
        
    }

    IEnumerator Enumerator(Action callback)
    {
        Debug.Log("开启一个协程");
        yield return new WaitForSeconds(2);
        Debug.Log("协程执行完毕");
        yield return null;
        Debug.Log("协程关闭");
        callback();
        Debug.Log("协程已经关闭");
        yield return null;
        Debug.Log("再次跑了一帧");
    }
}
