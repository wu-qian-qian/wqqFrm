using System.Collections;
using UnityEngine;
using System;
using System.Collections.Concurrent;

public abstract class absDrontIntermediares : iIntermediaries
{
    [Dependency("Dont")] IObserverModule observer;
    [Dependency] IObserverModule observerKid;
    /// <summary>
    /// controller通过订阅该事件来跟其他的controller来沟通
    /// </summary>
    public static event Action<MessageInfo> Interaction_even;
    private ConcurrentQueue<MessageInfo> messages;
    public virtual void FixUpdata()
    {
       
    }

    public virtual void Init()
    {
        messages = new ConcurrentQueue<MessageInfo>();
        observer.Add(MessageEnumEvenDont.Registerbro, ReceiveMessage);
        observerKid.Add(MessageEnumEvenDont.Registerkid, ReceiveMessage);
    }

    private void ReceiveMessage(params object[] obj)
    {
        ReceiveMessage((MessageInfo)obj[0]);
    }

    public abstract void Updata();

    public virtual void GroupMessage()
    {
        messages.TryDequeue(out MessageInfo message);
        if (message == null) return;
        Interaction_even?.Invoke(message);
    }

    public virtual void ReceiveMessage(MessageInfo info)
    {
        messages.Enqueue(info);
    }
}
