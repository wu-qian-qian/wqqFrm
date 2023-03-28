

using System;
using System.Collections.Concurrent;
using UnityEngine;
using Object = UnityEngine.Object;
public abstract class absIntermediaries : iIntermediaries
{
    /// <summary>
    /// 子系统观察者
    /// </summary>
    [Dependency("Main")] IObserverModule observerKid;
    /// <summary>
    /// 同级系统观察者
    /// </summary>
    [Dependency] IObserverModule observer;
    protected ConcurrentQueue<MessageInfo> messages;
    /// <summary>
    /// controller通过订阅该事件来跟其他的controller来沟通
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
    /// 获取上个流程需要保存的数据
    /// </summary>
    /// <param name="obj"></param>
    public virtual void Enter(params System.Object[] obj)
    {
    }
    /// <summary>
    /// 通过调用act，并且传入数据(下一关场景可能需要的数据)
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
