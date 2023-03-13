

using UnityEngine;

public abstract class absIntermediaries : iIntermediaries
{
    public  void Change(string interName)
    {
        GameController.instance.Change(interName);
    }

    public virtual void Enter(params System.Object[] obj)
    {
    }

    public virtual void Exit(System.Action<Object[]> act)
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
        Debug.Log("”Œœ∑‘À––123");
    }
}
