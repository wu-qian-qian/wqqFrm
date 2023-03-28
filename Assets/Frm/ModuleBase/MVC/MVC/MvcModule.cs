

using System;
using System.Reflection;

public class MvcModule : IMvcModule
{
    private static IocContainer _Container;
    public MvcModule() => _Container = IocContainer.instance;

    public void InJectController(Container container)
    {
        foreach (var item in container.fieldInfos)
        {
            ControllerAttribute controller = item.GetCustomAttribute<ControllerAttribute>();
            if (controller == null)
                continue;
            Type fielType = item.FieldType;

            if (!typeof(IMvcBase).IsAssignableFrom(fielType))
                continue;
            //对Controller
            item.SetValue(container.obj, Load(fielType,controller.key));
        }
    }
    /// <summary>
    /// 对Mvc进行注册与实例
    /// </summary>
    /// <param name="type"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public object Load(Type type, string key)
    {
        //给予名字
        string finlKey = type.Name + key;
        Type mvcType = typeof(IMvcBase);
        Container container = _Container.GetContainer<IMvcBase>(finlKey);
        if (container != null)
            return container.obj;
        //第一次调用是对Controller进行注册
        _Container.Register<IMvcBase>(mvcType, finlKey);
        IMvcBase obj = (IMvcBase)_Container.Resolve(mvcType, finlKey);
        //这里判断该实例是否为Controller类型不是就直接返回实例，因为View与Module不存在Mvc的下一级，而其他字段的标记获取实例Resolve做出了处理
        if (!typeof(IController).IsAssignableFrom(mvcType))
        {
            obj.Init();
            return obj;
        }
        container = _Container.GetContainer<IMvcBase>(finlKey);
        //对Con
        foreach (var item in container.fieldInfos)
        {
            Type fielType = item.FieldType;
            ModuleAttribute module = item.GetCustomAttribute<ModuleAttribute>();
            if (module != null)
            {
                if (!typeof(IModule).IsAssignableFrom(fielType))
                    continue;
                //对Module
                item.SetValue(container.obj, Load(fielType, module.key));
                continue;
            }
            ViewAttribute view = item.GetCustomAttribute<ViewAttribute>();
            if (module == null)
                continue;
                if (!typeof(IView).IsAssignableFrom(fielType))
                    continue;
                //对View进行
                item.SetValue(container.obj, Load(fielType, view.key));
        }
        obj.Init();
        return obj;
    }
    public void Remove<T>(string key)where T:class ,IMvcBase => Remove(typeof(T), key);


    public void Remove(Type type, string key)
    {
        if (!typeof(IMvcBase).IsAssignableFrom(type))
            return;
        string finalKey = type.Name + key;
        foreach (Container item in _Container.GetContainers<IMvcBase>())
        {
            if (item.key != finalKey)
                continue;
            _Container.RemoveContainer<IMvcModule>(finalKey);
            break;
        }
    }
}

