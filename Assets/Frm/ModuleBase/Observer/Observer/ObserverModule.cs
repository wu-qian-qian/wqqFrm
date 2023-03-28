using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


public class ObserverModule : IObserverModule
    {
    private Dictionary<ValueType, Action<object[]>> socke;
    public ObserverModule() => socke = new Dictionary<ValueType, Action<object[]>>();
    public void Add<T>(T type, Action<object[]> callBack) where T : struct
        {
        if(!socke.ContainsKey(type))
            {
            socke.Add(type, callBack);
            return;
            }
        foreach (var method in socke[type].GetInvocationList())
            {
            if (method != callBack)
                continue;
            return;
            }
        socke[type] += callBack;
        }

    public void asnyDispatch<T>(T type, params object[] args) where T : struct
        {
        if (!socke.ContainsKey(type)) return;
        if (socke[type] == null)
            {
            socke.Remove(type);
            return;
            }
        Task.Run(() => { socke[type](args);Thread.Sleep(100); });
        }

    public void Clear()
        {
        socke.Clear();
        }

    public void Dispatch<T>(T type, params object[] args) where T : struct
        {
        if (!socke.ContainsKey(type)) return;
        if (socke[type] == null)
            {
            socke.Remove(type);
            return;
            }

        socke[type](args);
        }

    public void Remove<T>(T type, Action<object[]> callBack) where T : struct
        {
        if (callBack == null) return;
        if (!socke.ContainsKey(type)) return;
        if((socke[type] -= callBack) != null)
                return;
        socke.Remove(type);
        }
    }

