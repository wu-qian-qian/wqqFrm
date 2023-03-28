using System;

[AttributeUsage(AttributeTargets.Field,AllowMultiple =true,Inherited =false)]
public class ModuleAttribute : Attribute
{
    public string key = "";
    public ModuleAttribute(string key)
    {
        this.key = key;
    }
}
