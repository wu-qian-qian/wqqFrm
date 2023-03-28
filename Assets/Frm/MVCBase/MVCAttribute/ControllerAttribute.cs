using System;
[AttributeUsage(AttributeTargets.Field,AllowMultiple =true,Inherited =true)]
public class ControllerAttribute : Attribute
{
    public string key = "";
    public ControllerAttribute(string key)
    {
        this.key = key;
    }
}
