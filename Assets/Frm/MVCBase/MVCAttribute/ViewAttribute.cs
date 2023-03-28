using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class ViewAttribute:Attribute
{
    public string key = "";
    public ViewAttribute(string key)
    {
        this.key = key;
    }
}
