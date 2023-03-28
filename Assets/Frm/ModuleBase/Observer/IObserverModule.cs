using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Object = UnityEngine.Object;
    public interface IObserverModule
    { 
    void Add<T>(T type, Action<object[]> callBack) where T : struct;
    void Remove<T>(T type, Action<object[]> callBack) where T : struct;
    void Clear();
    void Dispatch<T>(T type, params object[] args) where T : struct;
    //TODO 做一个专门用来消息沟通的观察者使用多线程因为这个沟通涉及到网络所以需要多线程
 //   void asnyDispatch<T>(T type, params object[] args) where T : struct;
 //   void Add<T>(T type, Action<object[]> callBack) where T : struct;
}
