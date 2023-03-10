using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class GameController : ASingletonBase<GameController>,iGameCtroller
    {
    public IocContainer container { get; private set; }
    /// <summary>
    /// 游戏场景
    /// </summary>
    public absIntermediaries GameInter { get; private set; }
    /// <summary>
    /// 这个为不可以销毁游戏物体
    /// </summary>
    public iIntermediaries DontGameInter { get; private set; }
    public void Start()
    {
        container = IocContainer.instance;
        //注册游戏各个场景模块
        container.Register<iIntermediaries, IntermediariesMain>("Main");
        container.Register<iIntermediaries, IntermediariesMain>("DontGame");
    }
     public void Init()
    {
        GameInter = (absIntermediaries)container.Resolve<iIntermediaries>("Main");
        DontGameInter = container.Resolve<iIntermediaries>("DontGame");
    }

    public void FixUpdata()
    {
       
    }

    public void Updata()
    {
        DontGameInter.Updata();
    }

    public void Change(string interName,params Object[] obj)
    {      
        GameInter.Exit(Enter);
    }

    private void Enter(params Object[] obj)
    {
        //GameInter = container.Resolve<iIntermediaries>("xxxx");  需要切换的场景名;
        GameInter.Enter(obj);
    }
}
