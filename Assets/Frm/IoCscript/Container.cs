using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

/// <summary>
/// 这仅仅是一个工具，用来创建 Type 中类型的实例 ;还确保type的重复创建
/// 比如：我a模块需要一个ResModule加载资源，b模块也需要Resmodule加载，直接new的话就会产生2个实例，而我们标记，再由Ioc加载Contentinfo容器就只会产生一个实例
/// </summary>
public class Container
{
    /// <summary>
    /// 判断该容器是否有标记的字段类型
    /// </summary>
    public bool isInjected { get; set; }
    /// <summary>
    /// 主要用来区分Mvc类型使其释放
    /// </summary>
    public string key { get; private set; }
    public Type type { private set; get; }
    /// <summary>
    /// 当外界需要容器的类型是变量被创建
    /// </summary>
    public object obj { get { return ins ?? (ins = Activator.CreateInstance(type)); } }
    /// <summary>
    /// 主要用来保存Mvc类型用来注册        
    /// </summary>
    public List<FieldInfo> fieldInfos;  //用来存储type的一些字段类型
    private object ins { get; set; }
    public Container(Type type, string key,  object ins=null)
    {
        this.type = type;
        this.ins = ins;
        this.key = key;
        fieldInfos = new List<FieldInfo>();
    }
}
