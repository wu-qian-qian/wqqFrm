using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

/// <summary>
/// �������һ�����ߣ��������� Type �����͵�ʵ�� ;��ȷ��type���ظ�����
/// ���磺��aģ����Ҫһ��ResModule������Դ��bģ��Ҳ��ҪResmodule���أ�ֱ��new�Ļ��ͻ����2��ʵ���������Ǳ�ǣ�����Ioc����Contentinfo������ֻ�����һ��ʵ��
/// </summary>
public class Container
{
    /// <summary>
    /// �жϸ������Ƿ��б�ǵ��ֶ�����
    /// </summary>
    public bool isInjected { get; set; }
    /// <summary>
    /// ��Ҫ��������Mvc����ʹ���ͷ�
    /// </summary>
    public string key { get; private set; }
    public Type type { private set; get; }
    /// <summary>
    /// �������Ҫ�����������Ǳ���������
    /// </summary>
    public object obj { get { return ins ?? (ins = Activator.CreateInstance(type)); } }
    /// <summary>
    /// ��Ҫ��������Mvc��������ע��        
    /// </summary>
    public List<FieldInfo> fieldInfos;  //�����洢type��һЩ�ֶ�����
    private object ins { get; set; }
    public Container(Type type, string key,  object ins=null)
    {
        this.type = type;
        this.ins = ins;
        this.key = key;
        fieldInfos = new List<FieldInfo>();
    }
}
