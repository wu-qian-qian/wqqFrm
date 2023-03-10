using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 标记特性
/// </summary>
[AttributeUsage(AttributeTargets.Field,AllowMultiple =false,Inherited =true)]
public class DependencyAttribute :Attribute
{
    public string key { get; private set; }
    public DependencyAttribute(string key="")
    {
        this.key = key;
    }
}
