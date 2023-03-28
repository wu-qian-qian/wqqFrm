using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class IocContainer:ASingletonBase<IocContainer>
{
    
    private const BindingFlags Flag = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
    /// <summary>
    /// Type ע������ͣ�string ע������� Container����ע���socke�е�������
    /// </summary>
    private Dictionary<Type, Dictionary<string, Container>> socke;
    /// <summary>
    /// ÿ������ֻ����һ�������ֵ�
    /// </summary>
    public IocContainer() => socke = new Dictionary<Type, Dictionary<string, Container>>();
   
    /// <summary>
    /// ��������������ע���ʵ��
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
            //��ȡ����Dependency���Եı�ǩ
            DependencyAttribute dependency = fieldInfos[i].GetCustomAttribute<DependencyAttribute>();
            if (dependency == null)
                continue;
            Type fieldType = fieldInfos[i].FieldType;
            object target = Resolve(fieldType.GetType(),dependency.key);
            if (target == null)
            {
                Debug.LogError("�����Ա������δע�ᣡ����");
                continue;
            }
            //�����Ա��ʵ����ֵ
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
    /// ����2�����������ͣ���ֵ
    /// </summary>
    /// <typeparam name="T">type�ĸ���</typeparam>
    /// <param name="type">һ������</param>
    /// <param name="key">�����͵�����</param>
    public void Register<T>(Type type, string key = "") => Register<T>(key,new Container(type,key));
    /// <summary>
    /// ����һ������Ϊ��ֵ
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="key"></param>
    public void Register<T1,T2>(string key = "") where T1 : class where T2 : T1 => Register<T1>(key, new Container(typeof(T2),key));
    /// <summary>
    /// ֱ����ʵ������ʺ�û�и��������;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance"></param>
    /// <param name="isInjected">�����ж��Լ��Ƿ��б�ǵ����ͣ�</param>
    /// <param name="key"></param>
    public void Register<T>(T instance,string key = "") => Register<T>(key,new Container(instance.GetType(),key ,instance));

    /// <summary>
    /// ��ȡ��ע�����͵�ʵ��
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
            Debug.LogError("������δע�ᣡ������");
            return null;
        }
        if(!containers.TryGetValue(key,out Container container))
        {
            Debug.Log("������δע�ᣡ��������");
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
    /// ��ȡ��������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    public Container GetContainer<T>(string key = "")
    {
        Type type = typeof(T);
        if (!socke.TryGetValue(type, out Dictionary<string, Container> containers))
        {
            Debug.LogError("������δע�ᣬ��ȡ����ʧ�ܣ�����");
            return null;
        }
        if (!containers.TryGetValue(key, out Container container))
        {
            Debug.Log("������keyֵ�����ڻ�ȡ����ʧ�ܣ�������");
            return null;
        }
        return container;
    }
    /// <summary>
    /// ��ȡ������������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Container[] GetContainers<T>()
    {
        Type type = typeof(T);
        if (!socke.TryGetValue(type, out Dictionary<string, Container> containers))
        {
            Debug.LogError("������δע�ᣬ��ȡ����ʧ�ܣ�����");
            return null;
        }
        return new List<Container>(containers.Values).ToArray();

    }

    /// <summary>
    /// �Ƴ���������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    public void RemoveContainer<T>(string key="")
    {
        Type type = typeof(T);
        if (!socke.TryGetValue(type, out Dictionary<string, Container> containers))
        {
            Debug.LogError("������δע�ᣬ�����Ƴ�ʧ�ܣ�����");
            return;
        }
        if (!containers.TryGetValue(key, out Container container))
        {
            Debug.Log("������keyֵ�����ڣ������Ƴ�ʧ�ܣ�������");
            return ;
        }
        containers.Remove(key);
    }

    /// <summary>
    /// �Ƴ���������������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public void RemmoveContainer<T>()
    {
        Type type = typeof(T);
        if (!socke.TryGetValue(type, out Dictionary<string, Container> containers))
        {
            Debug.LogError("������δע�ᣬ�Ƴ�ʧ�ܣ�����");
            return;
        }
        socke.Remove(type);
    }
    public void Clear() => socke.Clear();
}
