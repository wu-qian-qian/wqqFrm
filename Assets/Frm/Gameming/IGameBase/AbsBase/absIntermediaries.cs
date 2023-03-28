

using System;
using System.Collections.Concurrent;
using UnityEngine;
using Object = UnityEngine.Object;
public abstract class absIntermediaries : iIntermediaries
{
    /// <summary>
    /// ��ϵͳ�۲���
    /// </summary>
    [Dependency("Main")] IObserverModule observerKid;
    /// <summary>
    /// ͬ��ϵͳ�۲���
    /// </summary>
    [Dependency] IObserverModule observer;
    protected ConcurrentQueue<MessageInfo> messages;
    /// <summary>
    /// controllerͨ�����ĸ��¼�����������controller����ͨ
    /// </summary>
    public static event Action<MessageInfo> Interaction_even;

    public virtual void Init()
    {
        messages = new ConcurrentQueue<MessageInfo>();
        observerKid.Add(MessageEnumEvenMain.Registerkid, ReceiveMessage);
        observer.Add(MessageEnumEvenMain.Registerbro, ReceiveMessage);
    }

    private void ReceiveMessage(params object[] obj)
    {
        ReceiveMessage((MessageInfo)obj[0]);
    }
    public  void Change(string interName)
    {
        GameController.instance.Change(interName);
    }

    /// <summary>
    /// ��ȡ�ϸ�������Ҫ���������
    /// </summary>
    /// <param name="obj"></param>
    public virtual void Enter(params System.Object[] obj)
    {
    }
    /// <summary>
    /// ͨ������act�����Ҵ�������(��һ�س���������Ҫ������)
    /// </summary>
    /// <param name="act"></param>
    public virtual void Exit(System.Action<Object[]> act)
    {
        act.Invoke(null);
    }

   
    public virtual void GroupMessage()
    {
        messages.TryDequeue(out MessageInfo message);
        if (message == null) return;
        Interaction_even?.Invoke(message);
    }

  

    public abstract void Updata();
    public abstract void FixUpdata();

    public void ReceiveMessage(MessageInfo info)
    {
        messages.Enqueue(info);
    }
}
