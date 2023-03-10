

using System;

public abstract class absIntermediaries : iIntermediaries
{
    public  void Change(string interName)
    {
        GameController.instance.Change(interName);
    }

    public virtual void Enter(params Object[] obj)
    {
    }

    public virtual void Exit(Action<Object[]> act)
    {
        act.Invoke(null);
    }

    public virtual void FixUpdata()
    {
       
    }

    public virtual void Init()
    {
     
    }

    public virtual void Updata()
    {
        
    }
}
