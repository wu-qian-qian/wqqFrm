using System;



    public interface IFSM
    {
    void Add<T>() where T : AbState;
    void Remove<T>() where T : AbState;
    void Change<T>(params object[] obj) where T : AbState;
    void Update();
    void Clear();
    }

