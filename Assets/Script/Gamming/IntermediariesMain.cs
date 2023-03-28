using UnityEngine;

public class IntermediariesMain : absIntermediaries
{

    #region 各个子系统  [Dependency] 标记的会自动实例

    #endregion

    public IntermediariesMain()
    {
        Debug.Log("创建成功");
        Init();
    }
    public override void Init()
    {
        base.Init();
    }

    public override void FixUpdata()
    {
        
    }

    public override void Updata()
    {
        if (messages.Count > 0)
            GroupMessage();
    }
}
