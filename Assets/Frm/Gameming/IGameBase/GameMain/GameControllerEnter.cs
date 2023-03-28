using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class GameController
{
    public void FixUpdata()
    {

    }

    public void Updata()
    {
        if (!Running) return;
        DontGameInter?.Updata();
        GameInter?.Updata();
    }

    public void Change(string interName, params Object[] obj)
    {
        ProsName = interName;
        GameInter.Exit(Enter);
        //如果涉及到多场景，可调用
        //DontGameInter 中的遮罩读条UI
    }
    /// <summary>
    /// 这里是通过回调来触发流程的切换，以及把上一流程需要保留的数据发送给Running流程
    /// </summary>
    /// <param name="obj"></param>
    private void Enter(params Object[] obj)
    {
        if (ProsName == string.Empty) return;

        //GameInter = container.Resolve<iIntermediaries>(ProsName);  需要切换的场景名;
        //注册下一流程的mvc
        MvcBase.InJectController(container.GetContainer<iIntermediaries>(ProsName));
        GameInter.Enter(obj);
        ProsName = string.Empty;
    }

    public void SetState()
    {
        Running = true;
    }
}