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
        Debug.Log("����һ��Э��");
        yield return new WaitForSeconds(2);
        Debug.Log("Э��ִ�����");
        yield return null;
        Debug.Log("Э�̹ر�");
        callback();
        Debug.Log("Э���Ѿ��ر�");
        yield return null;
        Debug.Log("�ٴ�����һ֡");
    }
}
