using System;
public interface IMvcModule
{
    void InJectController(Container container);

    void Remove<T>(string key) where T : class, IMvcBase;
    void Remove(Type type,string key);
}
