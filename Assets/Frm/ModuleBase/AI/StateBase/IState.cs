using System;

public interface IState
{
    void InjectCallBack(Action<Type,object[]> callback);

    void Init();

    void OnEnter(params object[] obj);

    void OnUpdate();

    void Change<T>(params object[] obj) where T : IState;
}
