using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public partial class  GameController : ASingletonBase<GameController>,iGameCtroller
    {
    string ProsName = "";
    bool Running = false;
    IMvcModule MvcBase;
    public IocContainer container { get; private set; }
    /// <summary>
    /// 游戏场景
    /// </summary>
    public absIntermediaries GameInter { get; private set; }
    /// <summary>
    /// 这个为不可以销毁游戏物体
    /// 更具游戏实际情况
    /// </summary>
    public absDrontIntermediares DontGameInter { get; private set; }
    public void Start()
    {
        container = IocContainer.instance;
        //注册游戏各个场景模块
        container.Register<iIntermediaries, IntermediariesMain>("Main");
        container.Register<iIntermediaries, DrontIntermediares>("DontGame");
        container.Register<IObserverModule, ObserverModule>();
        container.Register<IMvcModule, MvcModule>();
        Running = true;
    }

    public void Init()
    {
        ProsName = "Main";
        GameInter = (absIntermediaries)container.Resolve<iIntermediaries>(ProsName);
        DontGameInter =(absDrontIntermediares)container.Resolve<iIntermediaries>("DontGame");
        MvcBase = container.Resolve<MvcModule>();
        MvcBase.InJectController(container.GetContainer<iIntermediaries>("DontGame"));
        Enter(null);
    }
  
}
