using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class IocContainer:ASingletonBase<IocContainer>
{
    
    private const BindingFlags Flag = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
    /// <summary>
    /// Type 注册的类型；string 注册的名字 Container该类注册的socke中的容器类
    /// </summary>
    private Dictionary<Type, Dictionary<string, Container>> socke;
    /// <summary>
    /// 每种类型只能有一个容器字典
    /// </summary>
    public IocContainer() => socke = new Dictionary<Type, Dictionary<string, Container>>();
   
    /// <summary>
    /// 将该容器的类型注册成实例
    /// </summary>
    /// <param name="container"></param>
    public void Inject(Container container)
    {
        object obj = container.obj;
        Type type = container.obj.GetType();
        FieldInfo[] fieldInfos = type.GetFields(Flag);
        for (int i = 0; i < fieldInfos.Length; i++)
        {
            container.fieldInfos.Add(fieldInfos[i]);
            //获取带有Dependency特性的标签
            DependencyAttribute dependency = fieldInfos[i].GetCustomAttribute<DependencyAttribute>();
            if (dependency == null)
                continue;
            Type fieldType = fieldInfos[i].FieldType;
            object target = Resolve(fieldType.GetType(),dependency.key);
            if (target == null)
            {
                Debug.LogError("该特性标记类型未注册！！！");
                continue;
            }
            //给特性标记实例赋值
            fieldInfos[i].SetValue(obj, target);
        }
    }
    private void Register<T>(string key,Container container)
    {
        Type type = typeof(T);
        if(socke.TryGetValue(type,out Dictionary<string, Container> contentInfos))
        {
            if (!contentInfos.ContainsKey(key))
            {
                contentInfos.Add(key, container);
                return;
            }
            return;
        }
        socke.Add(type, new Dictionary<string, Container>() { {key,container } });
    }
    /// <summary>
    /// 接受2个参数：类型；键值
    /// </summary>
    /// <typeparam name="T">type的父类</typeparam>
    /// <param name="type">一个类型</param>
    /// <param name="key">该类型的名字</param>
    public void Register<T>(Type type, string key = "") => Register<T>(key,new Container(type,key));
    /// <summary>
    /// 接收一个参数为键值
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="key"></param>
    public void Register<T1,T2>(string key = "") where T1 : class where T2 : T1 => Register<T1>(key, new Container(typeof(T2),key));
    /// <summary>
    /// 直接由实例祖册适合没有父类的容器;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance"></param>
    /// <param name="isInjected">用来判断自己是否有标记的类型，</param>
    /// <param name="key"></param>
    public void Register<T>(T instance,string key = "") => Register<T>(key,new Container(instance.GetType(),key ,instance));

    /// <summary>
    /// 获取该注册类型的实例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public T Resolve<T>(string key="") where T : class
    {
        Type type = typeof(T);
        return (T)Resolve(type,key);
    }
    public object Resolve(Type type, string key="")
    {
        if(!socke.TryGetValue(type,out Dictionary<string,Container> containers))
        {
            Debug.LogError("该类型未注册！！！！");
            return null;
        }
        if(!containers.TryGetValue(key,out Container container))
        {
            Debug.Log("该容器未注册！！！！！");
            return null;
        }
        object obj = container.obj;
        if (container.isInjected)
            return obj;
        Inject(container);
        container.isInjected = true;
        return obj;
        
    }
    /// <summary>
    /// 获取单个容器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public Container GetContainer<T>(string key = "")
    {
        Type type = typeof(T);
        if (!socke.TryGetValue(type, out Dictionary<string, Container> containers))
        {
            Debug.LogError("该类型未注册，获取容器失败！！！");
            return null;
        }
        if (!containers.TryGetValue(key, out Container container))
        {
            Debug.Log("该容器key值不存在获取容器失败！！！！");
            return null;
        }
        return container;
    }
    /// <summary>
    /// 获取该类型有容器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Container[] GetContainers<T>()
    {
        Type type = typeof(T);
        if (!socke.TryGetValue(type, out Dictionary<string, Container> containers))
        {
            Debug.LogError("该类型未注册，获取容器失败！！！");
            return null;
        }
        return new List<Container>(containers.Values).ToArray();

    }

    /// <summary>
    /// 移除单个容器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    public void RemoveContainer<T>(string key="")
    {
        Type type = typeof(T);
        if (!socke.TryGetValue(type, out Dictionary<string, Container> containers))
        {
            Debug.LogError("该类型未注册，容器移除失败！！！");
            return;
        }
        if (!containers.TryGetValue(key, out Container container))
        {
            Debug.Log("该容器key值不存在，容器移除失败！！！！");
            return ;
        }
        containers.Remove(key);
    }

    /// <summary>
    /// 移除该类型所有容器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void RemmoveContainer<T>()
    {
        Type type = typeof(T);
        if (!socke.TryGetValue(type, out Dictionary<string, Container> containers))
        {
            Debug.LogError("该类型未注册，移除失败！！！");
            return;
        }
        socke.Remove(type);
    }
    public void Clear() => socke.Clear();
}
